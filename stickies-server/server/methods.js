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

function error(res, msg) {
    res.sendStatus(500);
    console.error(msg);
}

module.exports = { processLogin }