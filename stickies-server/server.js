const express = require('express');
const svlogin = require('./server/methods.js');
const func = require('./server/methods.js');
const morgan = require('morgan');

const app = express();
const port = process.env.PORT || 5000;

app.use(morgan('common'));
app.use(express.json());

app.post('/api/login', func.processLogin);
app.post('/api/register', func.processRegistration);
app.get('/api/user/:user', func.checkUserExists);
app.get('/api/getUserID/:user', func.getUserID);

app.listen(port, () => console.log(`Listening on port ${port}`));