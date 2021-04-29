const mysql = require('mysql2');

var config = require('./config.json')
var con = mysql.createConnection(config);

async function checkUserLogin(user, pass){
    let rows = await callProcedure('checkUserLogin', [user, pass]);
    
    if(rows[0].length > 0)
        return true;
    return false;
}

async function callProcedure(name, parameters){
    let [rows] = await con.promise().query(formatQuery(name, parameters));
    return rows;
}

function formatQuery(name, parameters){
    parameters = parameters.map((param) => `'${param}'`);
    return ('CALL <procName>(<parameters>);')
                        .replace('<procName>', name)
                        .replace('<parameters>', parameters.join(','));
}

module.exports = { checkUserLogin }