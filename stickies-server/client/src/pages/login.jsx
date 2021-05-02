import React from "react";
import '../login.css';
import { Button, FormGroup, Form } from 'react-bootstrap';
import { sha256 } from 'js-sha256';

//Functional Component 
const LoginPage = () => {
  document.body.style.backgroundColor = "#4f4f4f";

  async function processLogin() {
    const user = sha256(document.getElementById('txtUser').value);
    const pass = sha256(document.getElementById('txtPass').value);

    const data = { user, pass };

    //Debug
    console.log(data);

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
          alert("Login Successful, redirecting...");
          //redirectToIndex(); //?user=" + usr;
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
            <Form.Check type="checkbox" label="Remember me" />
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