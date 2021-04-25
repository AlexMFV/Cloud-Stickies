import React from "react";
import '../login.css';
import { Button, FormGroup, Form } from 'react-bootstrap';

//Functional Component 
const LoginPage = () => {
  return (
    <>
      <div className="loginContainer">
        <div className="titleContainer">
          <h3 className="loginTitle">Cloud Stickies</h3>
        </div>
        <div className="inputContainer">
          <Form.Group controlId="formUser">
            <Form.Label className="textboxText">Username</Form.Label>
            <Form.Control type="email" placeholder="Enter Username" />
          </Form.Group>

          <Form.Group controlId="formPwd">
            <Form.Label className="textboxText">Password</Form.Label>
            <Form.Control type="password" placeholder="Password" />
          </Form.Group>
          <Form.Group controlId="formRemember">
            <Form.Check type="checkbox" label="Remember me" />
          </Form.Group>
        </div>
        <div className="buttonContainer">
          <Button className="loginButton" variant="success">Login</Button>
          <Button className="loginButton" variant="info">No Account?</Button>
        </div>
      </div>
    </>
  );
};

export default LoginPage;