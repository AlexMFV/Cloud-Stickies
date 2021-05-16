export async function createCookies(cookies, id, cookieID, userID, pass) {
    //Declare cookie values
    let username = document.getElementById("txtUser").value;
    let date;

    if (document.getElementById('chkRemember').checked) {
        //Cookie expire date
        let days = 7;
        date = new Date(Date.now() + days * 24 * 60 * 60 * 1000); //Cookie Date
    }
    else {
        date = new Date(Date.now() + 30 * 60 * 1000); //Cookie Date 1 Hour expire
    }

    let expire = getISOdate(date); //MySQL Date

    const cookieClient = { cookieID, userID, pass, username };
    const cookieServer = { id, userID, cookieID, expire };

    //Local Cookie
    cookies.set('logn', JSON.stringify(cookieClient), { path: '/', expires: date });

    const options = {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(cookieServer)
    };

    //Server Cookie
    await fetch('/api/cookie/create', options).then(function (res) {
        if (res.status !== 200) {
            console.log('There was a problem. Status Code: ' +
                res.status);
            return;
        }
    }).catch(function (err) {
        console.log('Fetch Error: ', err);
    });
}

export function redirectToHome(history) {
    history.push("/home");
}

export function redirectToLogin(history) {
    history.push("/login");
}

export function getISOdate(date) {
    return (date.getFullYear() +
        "/" + (date.getMonth() + 1) +
        "/" + date.getDate() +
        " " + date.getHours() +
        ":" + date.getMinutes() +
        ":" + date.getSeconds());
}