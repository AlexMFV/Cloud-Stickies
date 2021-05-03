import React from "react";
import '../home.css';
import { Button, FormGroup, Form } from 'react-bootstrap';
import { sha256 } from 'js-sha256';

//Functional Component 
const HomePage = () => {
  document.body.style.backgroundColor = "#4f4f4f";

  return (
    <>
      <div className="topBar">
        <div className="titleBar">
          <img className="logo" src="https://i.pinimg.com/originals/c2/68/06/c268064de640c8ec3c139432552bbd05.png"></img>
          <h2 className="appName">Cloud Stickies</h2>
        </div>
        <div className="controlBar">
          
        </div>
      </div>
    </>
  );
};

export default HomePage;

/* FIRST DESIGN
<div className="mainCanvas">
  <div className="topBar"></div>
  <div className="doubleCanvas">
    <div className="navBar"></div>
    <div className="noteContent"></div>
  </div>
</div>
*/