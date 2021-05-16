import React, { useEffect } from "react";
import '../register.css';
import { Button, Form } from 'react-bootstrap';
import { sha256 } from 'js-sha256';
import Cookies from 'universal-cookie';
import { v4 as uuidv4 } from 'uuid';
import { useHistory } from 'react-router-dom';
import * as glob from '../globals';

//Functional Component 
const RegisterPage = () => {
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
          cookies.remove('logn');
        }
        else {
          glob.redirectToHome(history);
        }
      }
    }
    getCookie();
  }, [history]);

  async function processRegistration() {
    const id = uuidv4();
    const user = sha256(document.getElementById('txtUser').value);
    const pass = sha256(document.getElementById('txtPass').value);
    const pass2 = sha256(document.getElementById('txtPass2').value);

    if (pass !== pass2) {
      alert("Passwords do not match! Please try again!");
      clearFields();
    }
    else {
      const data = { id, user, pass };

      const userExists = await fetch('/api/user/' + user).then((res) => {
        return res.json();
      });

      if (!userExists) {
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

          res.json().then(function (success) {
            if (success) {
              const cookies = new Cookies();
              let id = uuidv4(); //Create UUID for the DB
              let cookieID = uuidv4(); //Create the cookie UUID
              glob.createCookies(cookies, id, cookieID, user, pass);
              glob.redirectToHome(history);
            }
            else {
              alert("There was an error creating the account, please try again later!");
            }
          });
        }).catch(function (err) {
          console.log('Fetch Error: ', err);
        });
      }
      else{
        alert('This username is already in use! Please choose another one!');
      }
    }
  }

  function clearFields(){
    document.getElementById('txtPass').value = "";
    document.getElementById('txtPass2').value = "";
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