import React from "react";
import '../register.css';
import { Button, FormGroup, Form } from 'react-bootstrap';
import { sha256 } from 'js-sha256';

//Functional Component 
const RegisterPage = () => {
  document.body.style.backgroundColor = "#4f4f4f";

  async function processRegistration() {
    const user = sha256(document.getElementById('txtUser').value);
    const pass = sha256(document.getElementById('txtPass').value);
    const pass2 = sha256(document.getElementById('txtPass2').value);

    if(pass !== pass2){
      clearFields();
    }
    const data = { user, pass };
    console.log(data);
    const options = {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(data)
    };

    await fetch('/api/register', options).then(function (res) {
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

  function clearFields(){
    alert("Passwords do not match! Please try again!");
    //Clear all the fields
  }

  return (
    <>
      <div className="registerContainer">
        <div className="titleContainer">
          <h3 className="registerTitle">Cloud Stickies</h3>
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

          <Form.Group controlId="formPwd2">
            <Form.Label className="textboxText">Re-type Password</Form.Label>
            <Form.Control id="txtPass2" type="password" placeholder="Password again" />
          </Form.Group>
          <Form.Group controlId="formRemember">
            <Form.Check id="chkRemember" type="checkbox" label="Remember me" />
          </Form.Group>
        </div>
        <div className="buttonContainer">
          <Button className="registerButton" onClick={processRegistration} variant="success">Register</Button>
          <Button className="registerButton" href="/login" variant="info">Have an Account?</Button>
        </div>
      </div>
    </>
  );
};

export default RegisterPage;