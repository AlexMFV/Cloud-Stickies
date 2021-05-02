const mysql = require('mysql2');

var config = require('./config.json')
var con = mysql.createConnection(config);

async function checkUserLogin(user, pass){
    let rows = await callProcedureRows('checkUserLogin', [user, pass]);
    return rows.length > 0 ? true : false;
}

async function createUser(user, pass){
    let numRows = await callProcedureNonQuery('createUser', [user, pass]);
    return numRows == 1 ? true : false;
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
    return rows.affectedRows;
}

function formatQuery(name, parameters){
    parameters = parameters.map((param) => `'${param}'`);
    return ('CALL <procName>(<parameters>);')
                        .replace('<procName>', name)
                        .replace('<parameters>', parameters.join(','));
}

module.exports = { checkUserLogin, createUser }