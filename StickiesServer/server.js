//Module declaration
const express = require('express');
const path = require('path');
const sha256 = require('js-sha256');
const session = require('express-session');

//File imports
const api = require('./api/api.js');
const dbCon = require('./database/connection');

//Variable declaration
const app = express();

//Defining public/static directories
app.use(express.static(path.join(__dirname, 'public')));
app.use(express.static(path.join(__dirname, 'views')));
app.use(express.json());
app.engine('.html', require('ejs').__express);
app.set('views', __dirname + '/views');
app.set('view engine', 'html');

dbCon.openConnection();

//GET Requests
app.get('/', function (req, res) {
  res.render('index');
});

//POST REQUESTS
app.post('/login', api.checkLogin);

//Server start
app.listen(8080);
console.log("Server listening on port 8080");