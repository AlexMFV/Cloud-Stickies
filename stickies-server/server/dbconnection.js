const mysql = require('mysql2');

var config = require('./config.json')
var con = mysql.createConnection(config);

async function checkUserLogin(user, pass){
    let rows = await callProcedureRows('checkUserLogin', [user, pass]);
    return rows.length > 0 ? true : false;
}

async function createUser(id, user, pass){
    try {
        let numRows = await callProcedureNonQuery('createUser', [id, user, pass]);

        if (numRows == 1)
            return true;
        return false;

    }
    catch(e){
        return false;
    }
}

async function createNote(conID, userID, note_id, noteText, noteTitle, noteColor, titleColor,
    dateCreated, baseFont, baseFontSize, baseFontColor, posX, posY, width, height, isClosed, isLocked){
    try {
        let numRows = await callProcedureNonQuery('createNote', [conID, userID, note_id, noteText, noteTitle, noteColor, titleColor,
            dateCreated, baseFont, baseFontSize, baseFontColor, posX, posY, width, height, isClosed, isLocked]);

        if(numRows == 1)
            return true;
        return false;

    } catch (e) {
        console.log(e);
        return false;
    }
}

async function updateNote(note_id, noteText, noteTitle, noteColor, titleColor,
    baseFont, baseFontSize, baseFontColor, posX, posY, width, height, isClosed, isLocked){
    try {
        let numRows = await callProcedureNonQuery('updateNote', [note_id, noteText, noteTitle, noteColor, titleColor,
            baseFont, baseFontSize, baseFontColor, posX, posY, width, height, isClosed, isLocked]);

        if(numRows > 0)
            return true;
        return false;

    } catch (e) {
        console.log(e);
        return false;
    }
}

async function checkUserExists(user){
    let count = await callProcedureFirstRow('checkUserExists', [user]);
    return count.result == 1 ? true : false;
}

async function getUserID(user){
    let userData = await callProcedureFirstRow('getUserID', [user]);
    return userData.user_id !== undefined ? userData.user_id : null;
}

/**
 * Gets all the rows returned by the called procedure.
 * @param {*} name Name of the procedure
 * @param {*} parameters Additional parameters for the procedure
 * @returns All rows returned from the database (JSON format)
 */
async function callProcedureRows(name, parameters){
    let [rows] = await con.promise().query(formatQuery(name, parameters));
    return rows[0];
}

/**
 * Gets the first row returned by the called procedure, ignores all other rows.
 * @param {*} name Name of the procedure
 * @param {*} parameters Additional parameters for the procedure
 * @returns First row returned from the database (JSON format)
 */
async function callProcedureFirstRow(name, parameters){
    let [rows] = await con.promise().query(formatQuery(name, parameters));
    return rows[0][0];
}

/**
 * Gets the numbers of rows affected by the called procedure.
 * @param {*} name Name of the procedure
 * @param {*} parameters Additional parameters for the procedure
 * @returns An integer, representing the number of affected rows.
 */
async function callProcedureNonQuery(name, parameters){
    let [rows] = await con.promise().query(formatQuery(name, parameters));
    console.log(rows);
    return rows.affectedRows;
}

function formatQuery(name, parameters){
    parameters = parameters.map((param) => `'${param}'`);
    return ('CALL <procName>(<parameters>);')
                        .replace('<procName>', name)
                        .replace('<parameters>', parameters.join(','));
}

module.exports = { checkUserLogin, createUser, checkUserExists, getUserID, createNote, updateNote }