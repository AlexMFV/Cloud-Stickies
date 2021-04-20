async function loginAccount(){
  const usr = document.getElementById('txtUser').value;
  const pwd = document.getElementById('txtPass').value;

  const data = { usr, pwd };
  const options = {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(data) //Data needs to be encrypted before being sent to the server
  };

  await fetch('/api/login', options).then(function(res) {
    if (res.status !== 200) {
      console.log('There was a problem. Status Code: ' +
      res.status);
      return;
    }

    res.json().then(function(isCorrect) {
      if(isCorrect)
      alert("Logged in Successfully!");
      else
      alert("Incorrect login details!");
    })}).catch(function(err){
      console.log('Login error: ', err);
    });

    //  res.json().then(function(exists) {
    //    if(exists){
    //      alert("Login Successful, redirecting...");
    //      reloadIndex(); //?user=" + usr;
    //    }
    //    else{
    //      alert("Incorrect details, please try again!");
    //    }
    //  });
    //}).catch(function(err) {
    //  console.log('Fetch Error: ', err);
    //});

    return false;
}
