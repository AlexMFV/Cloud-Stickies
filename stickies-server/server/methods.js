const { restart } = require('nodemon');
const db = require('./dbconnection');

async function processLogin(req, res){
    try{
        const exists = await db.checkUserLogin(req.body.user, req.body.pass);

        //Add the user to the session, to remember sign in
        //if (exists)
            //req.session.userId = req.body.usr;

        res.json(exists);
    }
    catch(e){
        error(res, e);
    }
}

async function processRegistration(req, res){
    try{
        const success = await db.createUser(req.body.id, req.body.user, req.body.pass);

        //Add the user to the session, to remember sign in
        //if (exists)
            //req.session.userId = req.body.usr;

        res.json(success);
    }
    catch(e){
        error(res, e);
    }
}

async function checkUserExists(req, res){
    try {
        const result = await db.checkUserExists(req.params.user);
        res.json(result);
    }
    catch (e) {
        error(res, e);
    }
}

function error(res, msg) {
    res.sendStatus(500);
    console.error(msg);
}

module.exports = { processLogin, processRegistration, checkUserExists }