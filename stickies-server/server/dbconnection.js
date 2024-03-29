const mysql = require('mysql2');
var config = require('./config.json')

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

async function getNotesFromUser(userID){
    let rows = await callProcedureRows('getNotesFromUser', [userID]);
    return rows;
}

async function deleteNotesFromUser(userID, noteIDs){
    try {
        let idArray = noteIDs.split(',');
        var numRows = 0;

        for(var noteID of idArray){
            numRows += await callProcedureNonQuery('deleteNotesFromUser', [userID, noteID]);
        }

        //If the number of altered rows (notes deleted)
        //are the same as the number of noteID that were given
        if (numRows/2 == idArray.length)
            return true;
        return false;

    }
    catch(e){
        return false;
    }
}

async function createCookie(id, user, cookie, expire){
    try {
        let numRows = await callProcedureNonQuery('createCookie', [id, user, cookie, expire]);

        if (numRows == 1)
            return true;
        return false;
    }
    catch(e){
        return false;
    }
}

async function deleteCookie(user, cookie){
    try {
        let numRows = await callProcedureNonQuery('deleteCookie', [user, cookie]);

        if (numRows == 1)
            return true;
        return false;

    }
    catch(e){
        return false;
    }
}

async function checkCookie(user, cookie){
    let count = await callProcedureFirstRow('checkCookie', [user, cookie]);
    return count.result > 0 ? true : false;
}

async function checkCookieExpire(){
    try {
        let numRows = await callProcedureNonQuery('checkCookieExpire', []);

        if (numRows == 1)
            return true;
        return false;

    }
    catch(e){
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
    var con = mysql.createConnection(config);
    let [rows] = await con.promise().query(formatQuery(name, parameters));
    con.end();
    return rows[0];
}

/**
 * Gets the first row returned by the called procedure, ignores all other rows.
 * @param {*} name Name of the procedure
 * @param {*} parameters Additional parameters for the procedure
 * @returns First row returned from the database (JSON format)
 */
async function callProcedureFirstRow(name, parameters){
    var con = mysql.createConnection(config);
    let [rows] = await con.promise().query(formatQuery(name, parameters));
    con.end();
    return rows[0][0];
}

/**
 * Gets the numbers of rows affected by the called procedure.
 * @param {*} name Name of the procedure
 * @param {*} parameters Additional parameters for the procedure
 * @returns An integer, representing the number of affected rows.
 */
async function callProcedureNonQuery(name, parameters){
    var con = mysql.createConnection(config);
    let [rows] = await con.promise().query(formatQuery(name, parameters));
    con.end();
    return rows.affectedRows;
}

function formatQuery(name, parameters){
    parameters = parameters.map((param) => `'${param}'`);
    return ('CALL <procName>(<parameters>);')
                        .replace('<procName>', name)
                        .replace('<parameters>', parameters.join(','));
}

module.exports = { checkUserLogin, createUser, checkUserExists, getUserID, createNote,
    updateNote, createCookie, checkCookie, deleteCookie, checkCookieExpire, getNotesFromUser,
    deleteNotesFromUser }