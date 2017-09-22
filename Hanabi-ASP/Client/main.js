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
    info = null
    MainDiv = document.getElementById("main");
    PlayerPassword = "";
    PlayerId = -1;
    TableName = "";
    PlayerNick = "";
    TableId = -1;
    TablePassword = "";
    NowState = "LogIn";
    BuildLogIn("Player");
    tim = null;
}

function UpDateCallBack(newInfo) {
    if (NowState == "LogIn")
        return;
    var NewState;
    if (newInfo.TableId == -1)
        Newstate = "PickTable";
    else if (!newInfo.Table.GameStarted)
        NewState = "Prepare";
    else
        NewState = "Game";
    info = newInfo;
    if (NewState != NowState) {
        NowState = NewState;
        if (NowState == "PickTable") {
            BuildPickTable();
        }
        else if (NowState == "Prepare") {
            BuildPrepare();
        }
        else {
            BuildGame();
        }
    }
    else {
        if (NowState == "Prepare") {
            UpDatePrepare();
        }
        else if (NowState == "Game") {
            UpDateGame();
        }
    }
}

function UpDate() {
    $.get("../api/GetInfo", {
        idPlayer: PlayerId,
        PlayerPassword: PlayerPassword
    }, UpDateCallBack);
}

function LogInCallBack(id) {
    if (id == -1) {
        if (LogInState.type == "Player")
            LogInState.DescriptionP.innerText = "Incorrect nick or password!";
        else
            LogInState.DescriptionP.innerText = "Incorrect table's name or password!";
    }
    else {
        tim = setInterval(UpDate, 1000);
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
            "PlayerPassword": PlayerPassword,
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
    clearInterval(tim);
}

function JoinTableEvent() {
    PickTableState.Type = "Join";
    NowState = "LogIn";
    LogInState.type = "Table";
    BuildLogIn("Table");
    clearInterval(tim);
}

function UpdatePrepate()
{

}

function UpDateGame()
{

}

function BuildLogIn(type) {
    MainDiv.innerHTML = `
        <form class="Centr">
            <div class="form-group">
                <label for="email" class="control-label" id="NickLabel">Nick:</label>
                <input type="email" class="form-control" id="email" placeholder="Your nick"></input>
            </div>
            <div class="form-group">
                <label for="pwd">Password:</label>
                <input type="password" class="form-control" id="pswd" placeholder="Password"></input>
            </div>
            <button class="btn btn-success" id="LogInButton" style="width:49%">Log In</button>
    	<button class="btn pull-right" id="back" style="width:49%; display:none">Back</button></form>`;
    LogInState.SubmitButton = document.getElementById("LogInButton");
    LogInState.SubmitButton.onclick = LogInEvent;
    LogInState.InputNick = document.getElementById("email");
    if (type != "Player") {
        LogInState.InputNick.setAttribute("placeholder", "Table's name");
        var lab = document.getElementById("NickLabel");
        lab.innerText = "Table:";
    }
    LogInState.InputPassword = document.getElementById("pswd");
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

function BuildPrepare() {

}

function BuildGame() {

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