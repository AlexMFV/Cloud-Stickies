import React, { useEffect } from "react";
import '../login.css';
import { Button, Form } from 'react-bootstrap';
import { sha256 } from 'js-sha256';
import Cookies from 'universal-cookie';
import { v4 as uuidv4 } from 'uuid';
import { useHistory } from 'react-router-dom';
import * as glob from '../globals';

//Functional Component 
const LoginPage = () => {
  document.body.style.backgroundColor = "#4f4f4f";
  const history = useHistory();

  useEffect(() => {
    async function getCookie() {
      const cookies = new Cookies(); //Instantiate the cookie
      const cookie = cookies.get('logn');

      if (cookie !== undefined) {
        const userID = cookie["userID"];
        const cookieID = cookie["cookieID"];

        const response = await fetch('/api/cookie/check/' + userID + '/' + cookieID);
        const data = await response.json();

        if (!data) {
          //Delete cookie
          cookie.remove('logn');
        }
        else {
          glob.redirectToHome(history);
        }
      }
    }
    getCookie();
  }, [history]);

  async function processLogin() {
    console.log("Login Funciont");
    const user = sha256(document.getElementById('txtUser').value);
    const pass = sha256(document.getElementById('txtPass').value);

    const data = { user, pass };

    const options = {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(data)
    };

    await fetch('/api/login', options).then(function (res) {
      if (res.status !== 200) {
        console.log('There was a problem. Status Code: ' +
          res.status);
        return;
      }

      res.json().then(function (exists) {
        if (exists) {
          //Change this to Modal
          const cookie = new Cookies(); //Instantiate the cookie
          let id = uuidv4(); //Create UUID for the DB
          let cookieID = uuidv4(); //Create the cookie UUID

          glob.createCookies(cookie, id, cookieID, user, pass);
          glob.redirectToHome(history);
        }
        else {
          alert("Incorrect details, please try again!");
        }
      });
    }).catch(function (err) {
      console.log('Fetch Error: ', err);
    });
  }

  return (
    <>
      <div className="loginContainer">
        <div className="titleContainer">
          <h3 className="loginTitle">Cloud Stickies</h3>
        </div>
        <div className="inputContainer">
          <Form.Group controlId="formUser">
            <Form.Label className="textboxText">Username</Form.Label>
            <Form.Control id="txtUser" type="text" placeholder="Enter Username" />
          </Form.Group>

          <Form.Group controlId="formPwd">
            <Form.Label className="textboxText">Password</Form.Label>
            <Form.Control id="txtPass" type="password" placeholder="Password" />
          </Form.Group>
          <Form.Group controlId="formRemember">
            <Form.Check id="chkRemember" type="checkbox" label="Remember me" />
          </Form.Group>
        </div>
        <div className="buttonContainer">
          <Button className="loginButton" onClick={processLogin} variant="success">Login</Button>
          <Button className="loginButton" href="/register" variant="info">No Account?</Button>
        </div>
      </div>
    </>
  );
};

export default LoginPage;