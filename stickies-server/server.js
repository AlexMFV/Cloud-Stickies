const express = require('express');
const svlogin = require('./server/methods.js');
const func = require('./server/methods.js');
const morgan = require('morgan');

const app = express();
const port = process.env.PORT || 5000;

app.use(morgan('common'));
app.use(express.json());

//LOGIN/REGISTER METHODS
app.post('/api/login', func.processLogin);
app.post('/api/register', func.processRegistration);

//USER METHODS
app.get('/api/user/:user', func.checkUserExists);
app.get('/api/getUserID/:user', func.getUserID);

//NOTE METHODS
app.post('/api/note/create', func.createNote);
app.post('/api/note/update', func.updateNote);

app.listen(port, () => console.log(`Listening on port ${port}`));