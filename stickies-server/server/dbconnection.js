const mysql = require('mysql');

var config = require('./config.json')
var con = mysql.createConnection(config);

function checkUserLogin(user, pass){
    //console.log(callProcedure('checkUserLogin', [user, pass]));
    /*if(callProcedure('checkUserLogin', [user, pass])[0].length > 0)
        return true;
    return false;*/
    callProcedure('checkUserLogin', [user, pass]);
}

function callProcedure(name, parameters){
    const values = con.query(formatQuery(name, parameters), function (error, results, fields) {
        if (error) throw error;
        return results;
    });

    console.log(values);
}

function formatQuery(name, parameters){
    parameters = parameters.map((param) => `'${param}'`);
    return ('CALL <procName>(<parameters>);')
                        .replace('<procName>', name)
                        .replace('<parameters>', parameters.join(','));
}

module.exports = { checkUserLogin }