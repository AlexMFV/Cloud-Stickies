/* LOGIN METHODS */

/*async*/ function checkLogin(req, res) {
    try {
        
    }
    catch (e) {
        error(res, e);
    }
}

/* REGISTER METHODS */
function registerUser(req, res){
    try{
        
    }
    catch(e){
        error(res, e);
    }
}

/**
 * Handles the error messages and sends a trace to the console.
 * @param {*} res Object of the response.
 * @param {*} msg Message to be displayed.
 */
function error(res, msg) {
    res.sendStatus(500);
    console.error(msg);
}

module.exports = {
    checkLogin,    
    error
}