/*$.post('../api/main', {
    "X": 10,
    "Y": 20,
    "IsEmpty": false
}, function () {
    console.log(10);
}, 'json');
*/
window.onload = init;

function init()
{
    info = null;
    LogInState = {
        type: "Player",
        InputNick: null,
        InputPassword: null,
        SubmitButton: null,
        DescriptionP: null
    };
    MainDiv = document.getElementById("main");
    PlayerPassword = "";
    BuildLogIn("Player");
}

function LogInCallBack(id)
{
    if (id == -1) {
        if (LogInState.type == "Player")
            LogInState.DescriptionP.innerText = "Incorrect nick or password!";
        else
            LogInState.DescriptionP.innerText = "Incorrect table's name or password!";
    }
}

function LogInEvent()
{
    $.post('../api/LogIn', {
        "Nick": LogInState.InputNick.value,
        "Password": LogInState.InputPassword.value
    }, LogInCallBack, 'json');    
}

function BuildLogIn(type)
{
    while (MainDiv.childElementCount > 0)
        MainDiv.lastChild.remove();
    LogInState.type = type;
    MainDiv.appendChild(CreateComponentP("Log In"));
    LogInState.DescriptionP = document.createElement("p");
    if (type == "Player")
        LogInState.DescriptionP.innerText = "Enter your nick and password";
    else 
        LogInState.DescriptionP.innerText = "Enter table's name and password";
    LogInState.InputNick = document.createElement("input");
    LogInState.InputNick.setAttribute("type", "text");
    if (type == "Player")
        LogInState.InputNick.setAttribute("placeholder", "your nick");
    else
        LogInState.InputNick.setAttribute("placeholder", "table's name");
    LogInState.InputPassword = document.createElement("input");
    LogInState.InputPassword.setAttribute("type", "text");
    LogInState.InputPassword.setAttribute("placeholder", "password");
    LogInState.SubmitButton = document.createElement("button");
    LogInState.SubmitButton.innerText = "Log In";
    LogInState.SubmitButton.onclick = LogInEvent;
    MainDiv.appendChild(LogInState.DescriptionP);
    MainDiv.appendChild(LogInState.InputNick);
    MainDiv.appendChild(LogInState.InputPassword);
    MainDiv.appendChild(LogInState.SubmitButton);
}

function CreateComponentP(text) {
    var elem = document.createElement("p");
    elem.innerText = text;
    return elem;
}