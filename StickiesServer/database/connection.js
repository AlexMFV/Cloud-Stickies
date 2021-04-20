const mysql = require('mysql');

const dbconfig = require('./config.json');
const connection = mysql.createConnection(dbconfig);

function openConnection(){
    connection.connect(function(err) {
        if(err) {
            console.error('Error connecting DB: ' + err.stack);
            return;
        }
    });
}

function closeConnection(){
    connection.end(function(err) {
        if(err) {
            console.error('Error closing connection DB: ' + err.stack);
            return;
        }
        console.log('Connection id: ' + connection.threadId);
    });
}

module.exports = {
    openConnection,
    closeConnection
}