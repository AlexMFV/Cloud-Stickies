const db = require('./dbconnection');

function processLogin(req, res){
    db.checkUserLogin(req.body.user, req.body.pass);
}


module.exports = { processLogin }