/*App.js*/
import React, { Component } from "react";
import LoginPage from "./pages/login";
import RegisterPage from "./pages/register";
import "./App.css";
import 'bootstrap/dist/css/bootstrap.min.css';
//Import all needed Component for this tutorial
import {
  BrowserRouter as Router,
  Route,
  Switch,
  Link,
  Redirect
} from "react-router-dom";

class App extends Component {
  render() {
    return (
      <Router>
       {/*All our Routes goes here!*/}
       <Route exact path="/"
          render={() => {
            return (
              //this.state.isUserAuthenticated ?
              //<Redirect to="/home" /> :
              <Redirect to="/login" />  ) }} />
       <Route path="/login" component={LoginPage} />
       <Route path="/register" component={RegisterPage} />
      </Router>
    );
  }
}

export default App;