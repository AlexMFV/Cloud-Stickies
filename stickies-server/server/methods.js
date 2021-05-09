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

async function getUserID(req, res){
    try {
        const result = await db.getUserID(req.params.user)
        res.json(result);
    } catch (error) {
        error(res, e);
    }
}

async function createNote(req, res){
    try {
        const result = await db.createNote(req.body.conID, req.body.userID,req.body.note_id,
            req.body.noteText,req.body.noteTitle,req.body.noteColor,
            req.body.titleColor, req.body.dateCreated, req.body.baseFont,
            req.body.baseFontSize,req.body.baseFontColor,req.body.posX,
            req.body.posY,req.body.width,req.body.height,req.body.isClosed,
            req.body.isLocked);

        res.json(result);
    } catch (error) {
        error(res, e);
    }
}

async function updateNote(req, res){
    try {
        const result = await db.updateNote(req.body.note_id,
            req.body.noteText,req.body.noteTitle,req.body.noteColor,
            req.body.titleColor, req.body.baseFont,
            req.body.baseFontSize,req.body.baseFontColor,req.body.posX,
            req.body.posY,req.body.width,req.body.height,req.body.isClosed,
            req.body.isLocked);

        res.json(result);
    } catch (error) {
        error(res, e);
    }
}

function error(res, msg) {
    res.sendStatus(500);
    console.error(msg);
}

module.exports = { processLogin, processRegistration, checkUserExists, getUserID, createNote, updateNote }