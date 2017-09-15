window.onload = init;

function init() {
    LogInState = {
        type: "Player",
        InputNick: null,
        InputPassword: null,
        SubmitButton: null,
        DescriptionP: null
    };
    PickTableState = {
        CreateButton: null,
        JoinButton: null,
        Type: "Create"
    };
    MainDiv = document.getElementById("main");
    PlayerPassword = "";
    PlayerId = -1;
    TableName = "";
    PlayerNick = "";
    TableId = -1;
    TablePassword = "";
    NowState = "LogIn";
    BuildLogIn("Player");
}

function LogInCallBack(id) {
    if (id == -1) {
        if (LogInState.type == "Player")
            LogInState.DescriptionP.innerText = "Incorrect nick or password!";
        else
            LogInState.DescriptionP.innerText = "Incorrect table's name or password!";
    }
    else {
        if (LogInState.type == "Player") {
            PlayerId = id;
            PlayerPassword = LogInState.InputPassword.value;
            NowState = "PickTable";
            PlayerNick = LogInState.InputNick.value;
            BuildPickTable();
        }
        else {
            TableId = id;
            TablePassword = LogInState.InputPassword.innerText;
            TableName = LogInState.InputNick.value;
            //need add
        }
    }
}

function LogInEvent() {
    if (LogInState.type == "Player")
        $.post('../api/LogIn', {
            "Nick": LogInState.InputNick.value,
            "Password": LogInState.InputPassword.value
        }, LogInCallBack, 'json');
    else if (PickTableState.Type == "Create")
        $.post('../api/CreateTable', {
            "PlayerId": PlayerId,
            "PlayerPassword":PlayerPassword,
            "TableName": LogInState.InputNick.value,
            "TablePassword": LogInState.InputPassword.value
        }, LogInCallBack, 'json');
    else
        $.post('../api/JoinTable', {
            "idPlayer": PlayerId,
            "PlayerPassword": PlayerPassword,
            "TableName": LogInState.InputNick.value,
            "TablePassword": LogInState.InputPassword.value
        }, LogInCallBack, 'json');
}

function CreateTableEvent() {
    PickTableState.Type = "Create";
    NowState = "LogIn";
    LogInState.type = "Table";
    BuildLogIn("Table");
}

function JoinTableEvent() {
    PickTableState.Type = "Join";
    NowState = "LogIn";
    LogInState.type = "Table";
    BuildLogIn("Table");
}

function BuildLogIn(type) {
    ClearMainDiv();
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

function BuildPickTable() {
    ClearMainDiv();
    PickTableState.CreateButton = document.createElement("Button");
    PickTableState.CreateButton.innerText = "Create table";
    PickTableState.CreateButton.onclick = CreateTableEvent;
    PickTableState.JoinButton = document.createElement("Button");
    PickTableState.JoinButton.innerText = "Join to table";
    PickTableState.JoinButton.onclick = JoinTableEvent;
    MainDiv.appendChild(CreateComponentP("Now play " + PlayerNick));
    MainDiv.appendChild(PickTableState.CreateButton);
    MainDiv.appendChild(PickTableState.JoinButton);
}

function ClearMainDiv() {
    while (MainDiv.childElementCount > 0)
        MainDiv.lastChild.remove();
}

function CreateComponentP(text) {
    var elem = document.createElement("p");
    elem.innerText = text;
    return elem;
}