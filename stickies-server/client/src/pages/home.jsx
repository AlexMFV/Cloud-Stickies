import React, { useState, useEffect } from "react";
import '../home.css';
import Cookies from 'universal-cookie';
import { useHistory } from 'react-router-dom';
import * as glob from '../globals';
import StickyNotes from '../StickyNotes'

//Functional Component 
const HomePage = () => {
  document.body.style.backgroundColor = "#4f4f4f";
  const history = useHistory();
  const [stickies, setStickies] = useState([])

  function clickSticky(id){
    const newStickies = [...stickies];
    const sticky = newStickies.find(sticky => sticky.note_id === id)
    document.getElementById("noteContent").value = sticky.noteText;
    const newColor = "#" + sticky.noteColor.substring(3, sticky.noteColor.length);
    document.getElementById("noteContent").setAttribute("style", "background-color: " + newColor + ";");
  }

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
          glob.redirectToLogin(history);
          cookie.remove('logn');
        }

        getStickies(userID);
      }
      else {
        glob.redirectToLogin(history);
      }
    }
    
    async function getStickies(user){
      const response = await fetch('/api/getUserID/' + user);
      const userID = await response.json();

      const response2 = await fetch('/api/note/get/' + userID);
      const data = await response2.json();
      
      data.forEach(sticky => {
        setStickies(array => {
          return [...array, sticky];
        });
      });
    }

    getCookie();
  }, [history]);

  function logoutUser(){
    const cookies = new Cookies(); //Instantiate the cookie
    const cookie = cookies.get('logn');

    if (cookie !== undefined) {
      cookies.remove('logn');
      glob.redirectToLogin(history);
    }
  }

  return (
    <div className="conts">
      <div className="header">
        <div className="titleBar">
          <img className="logo" alt="Logo" src="https://i.pinimg.com/originals/c2/68/06/c268064de640c8ec3c139432552bbd05.png"></img>
          <h2 className="appName">Cloud Stickies</h2>
        </div>
        <div className="controlBar">
          <button className="control" onClick={logoutUser}>Logout</button>
        </div>
      </div>
      <div className="body">
        <div className="sidebar">
          <div className="subsidebar">
            <StickyNotes stickies={stickies} clickSticky={clickSticky}/>
          </div>
        </div>
        <textarea id="noteContent" value="" readOnly={true} className="content">
          No notes to read
        </textarea>
      </div>
      <div className="footer">Made by: Alex Valente (UP902282)</div>
    </div>
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