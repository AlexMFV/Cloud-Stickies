import React from "react";
import '../register.css';
import { Button, FormGroup, Form } from 'react-bootstrap';
//Functional Component 
const RegisterPage = () => {
  document.body.style.backgroundColor = "#4f4f4f";

  return (
    <>
      <div className="registerContainer">
        <div className="titleContainer">
          <h3 className="registerTitle">Cloud Stickies</h3>
        </div>
        <div className="inputContainer">
          <Form.Group controlId="formUser">
            <Form.Label className="textboxText">Username</Form.Label>
            <Form.Control type="text" placeholder="Enter Username" />
          </Form.Group>

          <Form.Group controlId="formPwd">
            <Form.Label className="textboxText">Password</Form.Label>
            <Form.Control type="password" placeholder="Password" />
          </Form.Group>

          <Form.Group controlId="formPwd2">
            <Form.Label className="textboxText">Re-type Password</Form.Label>
            <Form.Control type="password" placeholder="Password again" />
          </Form.Group>
          <Form.Group controlId="formRemember">
            <Form.Check type="checkbox" label="Remember me" />
          </Form.Group>
        </div>
        <div className="buttonContainer">
          <Button className="registerButton" variant="success">Register</Button>
          <Button className="registerButton" href="/login" variant="info">Have an Account?</Button>
        </div>
      </div>
    </>
  );
};

export default RegisterPage;