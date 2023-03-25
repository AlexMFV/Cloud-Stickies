const express = require('express');
const svlogin = require('./server/methods.js');
const func = require('./server/methods.js');
const morgan = require('morgan');

const app = express();
const port = process.env.PORT || 5000;

app.use(morgan('common'));
app.use(express.json());

//LOGIN/REGISTER METHODS
app.get('/status', func.status);
app.post('/api/login', func.processLogin);
app.post('/api/register', func.processRegistration);

//USER METHODS
app.get('/api/user/:user', func.checkUserExists);
app.get('/api/getUserID/:user', func.getUserID);

//NOTE METHODS
app.post('/api/note/create', func.createNote);
app.post('/api/note/update', func.updateNote);
app.get('/api/note/get/:userID', func.getNotesFromUser);
app.post('/api/note/delete', func.deleteNotesFromUser);

//COOKIE METHODS
app.get('/api/cookie/check/:user/:cookie', func.checkCookie);
app.post('/api/cookie/create', func.createCookie);
app.delete('/api/cookie/delete/:user/:cookie', func.deleteCookie);

function run() { func.checkCookieExpire().catch((e) => console.log('An error ocurred while running checkCookieExpire()'))};

app.listen(port, () => console.log(`API listening on port: ${port}`));

run();