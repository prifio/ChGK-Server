﻿window.onload = init;
$.post = function (url, data, CallBack, dataType) {
	return jQuery.ajax({
		headers: {
			'Accept': 'application/json',
			'Content-Type': 'application/' + dataType
		},
		'type': 'POST',
		'url': url,
		'data': JSON.stringify(data),
		'dataType': dataType,
		'success': CallBack
	});
};

$.get = function (url, data, CallBack) {
	return jQuery.ajax({
		'type': 'GET',
		'url': url,
		'data': data,
		'response': 'xml',
		'success': CallBack
	});
};

function init() {
	LogInState = {
		type: "Player",
		InputNick: null,
		InputPassword: null,
		SubmitButton: null,
		Form: null,
		P: null
	};
	PickTableState = {
		CreateButton: null,
		JoinButton: null,
		Type: "Create"
	};
	PrepState = {
		OldPlayersList: [],
		PlayersList: null,
		OldSeats: [],
		Seats: [],
		IsAdmin: false,
		GameTypeP: null,
		StartButton: null
	};
	GameState = {
		StoryLen: 0,
		DropLen: 0,
		ScroeP: null,
		FallsP: null,
		HintsP: null,
		History: null,
		Drops: null,
		Table: [],
		OldTable: [],
		OldCards: [],
		YouSeat: 0,
		PlayersNick: [],
		PlayersCards: [],
		EndGameButton: null,
		DialogType: "None",
		DialogButtons: null,
		Descriprion: null,
		DialogPlayer: -1,
		CardsInDeck: null,
		EndGameP: null,
		Interesting: null
	};
	info = null;
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
	GetStringSuit = ["violet", "red", "blue", "orange", "green", "rainbow"];
	GameTypeString = ["Five color", "With rainbow", "Rainbow is every"];
}

function UpDateCallBack(newInfo) {
	var NewState;
	if (newInfo.IdTable == -1)
		NewState = "PickTable";
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
		LogInState.Form.setAttribute("class", "Center has-error");
		LogInState.P.setAttribute("style", "");
	}
	else {
		if (LogInState.type == "Player") {
			PlayerId = id;
			PlayerPassword = LogInState.InputPassword.value;
			PlayerNick = LogInState.InputNick.value;
		}
		else {
			TableId = id;
			TablePassword = LogInState.InputPassword.innerText;
			TableName = LogInState.InputNick.value;
		}
		tim = setInterval(UpDate, 1000);
		UpDate();
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
			"idPlayer": PlayerId,
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

function BackEvent() {
	NowState = "PickTable";
	BuildPickTable();
	tim = setInterval(UpDate, 1000);
}

function EndGameEvent() {
	$.post("../api/EndGame", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword
	}, function (_) { }, "json");
}

function PickHintEvent() {
	BuildPickPlayerForHint();
}

function PickDropEvent() {
	BuildPickCard(false);
}

function PickLayEvent() {
	BuildPickCard(true);
}

function PickPlayerEvent(num) {
	GameState.DialogPlayer = num;
	BuildPickTypeHint();
}

function DialogBackEvent() {
	if (GameState.DialogType == "Player")
		BuildPickTurn();
	else if (GameState.DialogType == "Card") {
		DestroyPickCard(GameState.YouSeat);
		BuildPickTurn();
	}
	else if (GameState.DialogType == "Type")
		BuildPickPlayerForHint();
	else if (GameState.DialogType == "Color" || GameState.DialogType == "Number")
		BuildPickTypeHint();
}

function PickColorEvent() {
	BuildColorHint();
}

function PickNumberEvent() {
	BuildNumberHint();
}

function GetHintColorEvent(color) {
	$.post("../api/HintColor", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		numPlayer: GameState.DialogPlayer,
		numColor: color
	}, function (_) { }, "json");
}

function GetHintNumberEvent(number) {
	$.post("../api/HintNumber", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		numPlayer: GameState.DialogPlayer,
		Number: number
	}, function (_) { }, "json");
}

function LayCardEvent(num) {
	$.post("../api/PlaceCard", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		numCard: num
	}, function (b) {
		if (b)
			DestroyPickCard(GameState.YouSeat);
	}, "json");
}

function DropCardEvent(num) {
	$.post("../api/DropCard", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		numCard: num
	}, function (b) {
		if (b)
			DestroyPickCard(GameState.YouSeat);
	}, "json");
}

function ChangeGameModeEvent(i) {
	$.post("../api/ChangeGameType", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		newGameType: i
	}, function (_) { }, "json");
}

function ChangeSeatsEvent(i) {
	$.post("../api/ChangeSeatsCount", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		SeatsCount: i
	}, function (_) { }, "json");
}

function StartGameEvent() {
	$.post("../api/StartGame", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword
	}, function (_) { }, "json");
}

function KickEvent(id) {
	$.post("../api/KickPlayer", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		idKicksPlayer: id
	}, function (_) { }, "json");
}

function SitDownEvent(i) {
	$.post("../api/SitDown", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		numSeat: i
	}, function (_) { }, "json");
}

function StandUpEvent() {
	$.post("../api/StandUp", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword
	}, function (_) { }, "json");
}

function ForceUpEvent(i) {
	$.post("../api/ForceStandUp", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword,
		idStandingPlayer: i
	}, function (_) { }, "json");
}

function LeaveEvent() {
	$.post("../api/LeaveTable", {
		idPlayer: PlayerId,
		PlayerPassword: PlayerPassword
	}, function (_) { }, "json");
}

function UpDatePlayersList() {
	var IsNew = false;
	var PlayersArr = PrepState.OldPlayersList;
	var Keys = Object.keys(info.Table.Players);
	if (PlayersArr.length != Keys.length)
		IsNew = true;
	else
		for (var i = 0; i < Keys.length; ++i) {
			IsNew = IsNew || PlayersArr[i] != info.Table.Players[Keys[i]];
		}
	if (IsNew) {
		PrepState.PlayersList.innerHTML = "";
		PrepState.OldPlayersList = [];
		for (var i = 0; i < Keys.length; ++i) {
			var NowId = info.Table.Players[Keys[i]];
			var elem = document.createElement("div");
			var PText = CreateElementP(info.NickById[NowId]);
			if (!PrepState.IsAdmin && NowId == info.Table.IdAdmin)
				PText.innerText += "[ADMIN]";
			PText.setAttribute("style", "display:inline-block; font-size:2.5rem; margin-right: 7px;");
			elem.appendChild(PText);
			if (PrepState.IsAdmin && NowId != PlayerId) {
				var btn = document.createElement("button");
				btn.setAttribute("type", "button");
				btn.setAttribute("class", "btn btn-danger");
				btn.innerText = "Kick";
				btn.onclick = CreateNiceFun(KickEvent, NowId);
				elem.appendChild(btn);
			}
			PrepState.PlayersList.appendChild(elem);
			PrepState.OldPlayersList[i] = NowId;
		}
	}
}

function UpDateSeats() {
	var SeatsArr = info.Table.Seats;
	var OldArr = PrepState.OldSeats;
	if (SeatsArr.length != OldArr.length) {
		var elem = document.getElementById("Pl1");
		elem.innerHTML = "";
		var ind = MyDiv(SeatsArr.length + 1, 2);
		for (var i = 0; i < ind; i++)
			elem.appendChild(BuildSeats(i));
		elem = document.getElementById("Pl2");
		elem.innerHTML = "";
		for (var i = SeatsArr.length - 1; i >= ind; i--)
			elem.appendChild(BuildSeats(i));
		PrepState.OldSeats = [];
		for (var i = 0; i < SeatsArr.length; ++i)
			PrepState.OldSeats[i] = -1;
		OldArr = PrepState.OldSeats;
	}
	for (var i = 0; i < SeatsArr.length; ++i) {
		if (SeatsArr[i] != OldArr[i]) {
			var Nick = PrepState.Seats[i].children[0];
			var BtnDiv = PrepState.Seats[i].children[1];
			var Btn = BtnDiv.children[0];
			var NowId = SeatsArr[i];
			if (NowId == -1) {
				Nick.innerText = "Empty";
				Btn.innerText = "Sit down";
				Btn.setAttribute("style", "");
				Btn.onclick = CreateNiceFun(SitDownEvent, i);
			}
			else if (NowId == PlayerId) {
				Nick.innerText = info.NickById[PlayerId];
				Btn.innerText = "Stand up";
				Btn.setAttribute("style", "");
				Btn.onclick = StandUpEvent;
			}
			else if (PrepState.IsAdmin) {
				Nick.innerText = info.NickById[NowId];
				Btn.innerText = "Leave";
				Btn.setAttribute("style", "");
				Btn.onclick = CreateNiceFun(ForceUpEvent, NowId);
			}
			else {
				Nick.innerText = info.NickById[NowId];
				Btn.setAttribute("style", "display:none;");
			}
			PrepState.OldSeats[i] = NowId;
		}
	}
}

function UpDatePrepare() {
	UpDateSeats();
	UpDatePlayersList();
	PrepState.GameTypeP.innerText = GameTypeString[info.Table.CurrentGameType];
	if (PrepState.IsAdmin) {
		var IsGood = true;
		IsGood = IsGood && Object.keys(info.Table.Players).length == info.Table.Seats.length;
		for (var i = 0; i < info.Table.Seats.length; ++i)
			IsGood = IsGood && info.Table.Seats[i] != -1;
		if (IsGood)
			PrepState.StartButton.setAttribute("class", "btn btn-success");
		else
			PrepState.StartButton.setAttribute("class", "btn");
	}

}

function UpDateStory() {
	while (info.Table.Game.Story.length > GameState.StoryLen) {
		var NowEvent = info.Table.Game.Story[GameState.StoryLen];
		var elem = document.createElement("p");
		if (NowEvent.Type == 0) {
			elem.innerText = info.NickById[info.Table.Seats[NowEvent.PlayerFrom]] + " hinted " +
				info.NickById[info.Table.Seats[NowEvent.PlayerTo]] + "'s " +
				GetStringSuit[NowEvent.Color];
			elem.appendChild(GetSuit(NowEvent.Color));
			elem.innerHTML += " cards";
			elem.setAttribute("class", "text-success");
		}
		if (NowEvent.Type == 1) {
			elem.innerText = info.NickById[info.Table.Seats[NowEvent.PlayerFrom]] + " hinted " +
				info.NickById[info.Table.Seats[NowEvent.PlayerTo]] + "'s " + NowEvent.Number + " cards";
			elem.setAttribute("class", "text-success");
		}
		if (NowEvent.Type == 2) {
			elem.innerText = info.NickById[info.Table.Seats[NowEvent.PlayerFrom]] + " laid " +
				NowEvent.Number;
			elem.appendChild(GetSuit(NowEvent.Color));
			elem.innerHTML += " on the table";
			elem.setAttribute("class", "text-primary");
		}
		if (NowEvent.Type == 3) {
			elem.innerText = info.NickById[info.Table.Seats[NowEvent.PlayerFrom]] + " falled by " +
				NowEvent.Number;
			elem.appendChild(GetSuit(NowEvent.Color));
			elem.setAttribute("class", "text-danger");
		}
		if (NowEvent.Type == 4) {
			elem.innerText = info.NickById[info.Table.Seats[NowEvent.PlayerFrom]] + " dropped " +
				NowEvent.Number;
			elem.appendChild(GetSuit(NowEvent.Color));
			elem.setAttribute("class", "text-warning");
		}
		if (NowEvent.Type >= 2)
			UpDateInteresting();
		var is_down = GameState.History.scrollHeight <= GameState.History.scrollTop + GameState.History.offsetHeight;
		GameState.History.appendChild(elem);
		if (is_down)
			GameState.History.scrollTop = GameState.History.scrollHeight;
		GameState.StoryLen += 1;
	}
}

function UpDateDrop() {
	while (info.Table.Game.DropsCards.length > GameState.DropLen) {
		var card = info.Table.Game.DropsCards[GameState.DropLen];
		AddToDrop(card.Color, card.Number);
		GameState.DropLen += 1;
		if (GameState.DropLen % 3 == 0)
			GameState.Drops.appendChild(document.createElement("br"));
	}
}

function UpDateTable() {
	if (GameState.OldTable.length != GameState.Table.length) {
		OldTable = [];
		for (var i = 0; i < GameState.Table.length; ++i)
			GameState.OldTable[i] = -1;
	}
	for (var i = 0; i < GameState.Table.length; i++) {
		if (GameState.OldTable[i] != GameState.Table) {
			var elem = GameState.Table[i];
			var cnt = info.Table.Game.Table[i];
			var card = GetCard(i, cnt);
			if (cnt == 0)
				card.setAttribute("class", card.getAttribute("class") + " DisActive");
			if (cnt == 5)
				card.setAttribute("class", card.getAttribute("class") + " Win");
			elem.removeChild(elem.children[0]);
			elem.appendChild(card);
			GameState.OldTable[i] = GameState.Table[i];
		}
	}
}

function UpDatePlayer(num) {
	var CardsArr = info.Table.Game.Players[num].Cards;
	if (GameState.OldCards[num].length != CardsArr.length) {
		GameState.OldCards[num] = [];
		for (var i = 0; i < CardsArr.length; ++i) {
			GameState.OldCards[num][i] = {
				Color: -2
			};
		}
		for (var i = CardsArr.length; i < 5; ++i)
			GameState.PlayersCards[num].children[i].setAttribute("style", "display:none;");
	}
	for (var i = 0; i < CardsArr.length; ++i) {
		var CurCard = GameState.OldCards[num][i];
		var RColor = CardsArr[i].Color;
		if (RColor == -1 && CardsArr[i].KnowColor != -1)
			RColor = CardsArr[i].KnowColor;
		var RNumber = CardsArr[i].Number;
		if (RNumber == -1 && CardsArr[i].KnowNumber != -1)
			RNumber = CardsArr[i].KnowNumber;
		if (RColor != CurCard.Color || RNumber != CurCard.Number) {
			GameState.PlayersCards[num].replaceChild(GetCard(RColor, RNumber), GameState.PlayersCards[num].children[i]);
			GameState.OldCards[num][i] = {
				Color: RColor,
				Number: RNumber,
				KnowColor: -1,
				KnowNumber: -1
			};
			CurCard = GameState.OldCards[num][i];
		}
		if (CardsArr[i].KnowColor != CurCard.KnowColor || CardsArr[i].KnowNumber != CurCard.KnowNumber) {
			var KColor = CardsArr[i].KnowColor;
			var KNumber = CardsArr[i].KnowNumber;
			var NCard = GetCard(RColor, RNumber);
			if (KColor != -1 && KNumber == -1) {
				NCard.setAttribute("class", NCard.getAttribute("class") + " Color");
			}
			if (KColor == -1 && KNumber != -1) {
				NCard.setAttribute("class", NCard.getAttribute("class") + " Numb");
			}
			if (KColor != -1 && KNumber != -1) {
				NCard.setAttribute("class", NCard.getAttribute("class") + " NumbColor");
			}
			GameState.PlayersCards[num].replaceChild(NCard, GameState.PlayersCards[num].children[i]);
			GameState.OldCards[num][i].KnowNumber = KNumber;
			GameState.OldCards[num][i].KnowColor = KColor;
		}
	}
	if (info.Table.Game.CurrentPlayer == num)
		GameState.PlayersNick[num].setAttribute("class", "Name Active");
	else
		GameState.PlayersNick[num].setAttribute("class", "Name");
	while (GameState.PlayersNick[num].children.length > 0)
		GameState.PlayersNick[num].removeChild(GameState.PlayersNick[num].children[GameState.PlayersNick[num].children.length - 1]);
	if (info.Table.Game.CurrentPlayer == num) {
		var elem = document.createElement("i");
		elem.setAttribute("class", "glyphicon glyphicon-flag");
		GameState.PlayersNick[num].appendChild(elem);
	}
	if (info.Table.Game.LastPlayer == num) {
		var elem = document.createElement("i");
		elem.setAttribute("class", "glyphicon glyphicon-warning-sign");
		GameState.PlayersNick[num].appendChild(elem);
	}
}

function UpDateDialog() {
	if (GameState.YouSeat == info.Table.Game.CurrentPlayer && GameState.DialogType == "None" && !info.Table.Game.GameIsEnd) {
		BuildPickTurn();
	}
	else if (GameState.YouSeat != info.Table.Game.CurrentPlayer && GameState.DialogType != "None") {
		GameState.Descriprion.innerText = "";
		GameState.DialogButtons.innerHTML = "";
		GameState.DialogType = "None";
	}
}

function UpDateInteresting() {
	var NowElem = GameState.Interesting;
	NowElem.innerHTML = "";
	var DropArr = info.Table.Game.DropsCards;
	var cnt = 0;
	for (var i = 0; i < DropArr.length; ++i) {
		if (info.Table.Game.Table[DropArr[i].Color] < DropArr[i].Number) {
			NowElem.appendChild(GetCard(DropArr[i].Color, DropArr[i].Number));
			++cnt;
			if (cnt % 3 == 0)
				NowElem.appendChild(document.createElement("br"));
		}

	}
}

function UpDateGame() {
	UpDateStory();
	UpDateDrop();
	UpDateTable();
	GameState.FallsP.innerText = info.Table.Game.CountFall;
	GameState.HintsP.innerText = info.Table.Game.CountHints;
	GameState.ScroeP.innerText = info.Table.Game.Result;
	GameState.CardsInDeck.innerText = info.Table.Game.CardsInDeck;
	if (info.Table.Game.GameIsEnd)
		GameState.EndGameP.innerText = "Game over";
	else if (info.Table.Game.CardsInDeck == 0)
		GameState.EndGameP.innerText = "Last circle!";
	else if (info.Table.Game.CardsInDeck < 5)
		GameState.EndGameP.innerText = "Last circle is coming";
	else
		GameState.EndGameP.innerText = "";
	for (var i = 0; i < info.Table.Seats.length; ++i)
		UpDatePlayer(i);
	UpDateDialog();
}

function DestroyPickCard(num) {
	GameState.PlayersCards[num].removeAttribute("class");
	for (var i = 0; i < 5; ++i)
		GameState.PlayersCards[num].children[i].onclick = null;
}

function BuildSeats(num) {
	var pl = document.createElement("td");
	var nickElem = CreateElementP("Empty");
	nickElem.setAttribute("class", "Name");
	pl.appendChild(nickElem);
	var BtnDiv = document.createElement("div");
	var bt = CreateButton("Sit down");
	bt.setAttribute("class", "btn btn-primary");
	bt.onclick = CreateNiceFun(SitDownEvent, num);
	BtnDiv.appendChild(bt);
	BtnDiv.setAttribute("style", "width:100%;");
	pl.appendChild(BtnDiv);
	PrepState.Seats[num] = pl;
	return pl;
}

function BuildPickCard(IsLay) {
	if (IsLay)
		GameState.Descriprion.innerText = "Pick card for lay";
	else
		GameState.Descriprion.innerText = "Pick card for drop";
	var elem = document.createElement("div");
	elem.setAttribute("class", "btn-group-gorizontal");
	var TimeName = ["oldest", "old", "middle", "new", "newest"];
	var yk = 0;
	var cnt = info.Table.Game.Players[GameState.YouSeat].Cards.length;
	var CardsArr = info.Table.Game.Players[GameState.YouSeat].Cards;
	for (var i = 0; i < cnt; i++) {
		var bt = CreateButton(TimeName[yk]);
		if (CardsArr[i].KnowNumber == -1)
			bt.innerText += "?";
		else
			bt.innerText += CardsArr[i].KnowNumber;
		bt.innerText += " ";
		bt.appendChild(GetSuit(CardsArr[i].KnowColor));
		if (IsLay)
			bt.onclick = CreateNiceFun(LayCardEvent, i);
		else
			bt.onclick = CreateNiceFun(DropCardEvent, i);
		elem.appendChild(bt);
		yk++;
		if (yk == 2 && cnt != 5)
			++yk;
	}
	GameState.DialogButtons.innerHTML = "";
	GameState.DialogButtons.appendChild(elem);
	var bc = CreateButton("Back");
	bc.onclick = DialogBackEvent;
	GameState.DialogButtons.appendChild(bc);
	GameState.PlayersCards[GameState.YouSeat].setAttribute("class", "ClickCard");
	for (var i = 0; i < CardsArr.length; ++i)
		if (IsLay)
			GameState.PlayersCards[GameState.YouSeat].children[i].onclick = CreateNiceFun(LayCardEvent, i);
		else
			GameState.PlayersCards[GameState.YouSeat].children[i].onclick = CreateNiceFun(DropCardEvent, i);
	GameState.DialogType = "Card";
}

function BuildNumberHint() {
	GameState.Descriprion.innerText = "Pick number for " + info.NickById[info.Table.Seats[GameState.DialogPlayer]];
	var elem = document.createElement("div");
	elem.setAttribute("class", "btn-group-gorizontal");
	for (var i = 1; i <= 5; i++) {
		var bt = CreateButton(i);
		bt.onclick = CreateNiceFun(GetHintNumberEvent, i);
		elem.appendChild(bt);
	}
	GameState.DialogButtons.innerHTML = "";
	GameState.DialogButtons.appendChild(elem);
	var bc = CreateButton("Back");
	bc.onclick = DialogBackEvent;
	GameState.DialogButtons.appendChild(bc);
	GameState.DialogType = "Number";
}

function BuildColorHint() {
	GameState.Descriprion.innerText = "Pick color for " + info.NickById[info.Table.Seats[GameState.DialogPlayer]];;
	var elem = document.createElement("div");
	elem.setAttribute("class", "btn-group-gorizontal");
	var cntColor = 5;
	if (info.Table.Game.CurrentGameType == 1)
		cntColor = 6;
	for (var i = 0; i < cntColor; i++) {
		var bt = CreateButton(GetStringSuit[i]);
		bt.appendChild(GetSuit(i));
		bt.onclick = CreateNiceFun(GetHintColorEvent, i);
		elem.appendChild(bt);
	}
	GameState.DialogButtons.innerHTML = "";
	GameState.DialogButtons.appendChild(elem);
	var bc = CreateButton("Back");
	bc.onclick = DialogBackEvent;
	GameState.DialogButtons.appendChild(bc);
	GameState.DialogType = "Color";
}

function BuildPickTypeHint() {
	GameState.Descriprion.innerText = "Pick type of hint";
	var elem = document.createElement("div");
	elem.setAttribute("class", "btn-group-gorizontal");
	var bt = CreateButton("Color");
	bt.onclick = PickColorEvent;
	elem.appendChild(bt);
	bt = CreateButton("Number");
	bt.onclick = PickNumberEvent;
	elem.appendChild(bt);
	GameState.DialogButtons.innerHTML = "";
	GameState.DialogButtons.appendChild(elem);
	var bc = CreateButton("Back");
	bc.onclick = DialogBackEvent;
	GameState.DialogButtons.appendChild(bc);
	GameState.DialogType = "Type";
}

function BuildPickPlayerForHint() {
	GameState.Descriprion.innerText = "Pick player for hint";
	var elem = document.createElement("div");
	elem.setAttribute("class", "btn-group-gorizontal");
	for (var i = 0; i < info.Table.Seats.length - 1; ++i) {
		var num = (i + GameState.YouSeat + 1) % info.Table.Seats.length;
		var bt = CreateButton(info.NickById[info.Table.Seats[num]]);
		bt.onclick = CreateNiceFun(PickPlayerEvent, num);
		elem.appendChild(bt);
	}
	GameState.DialogButtons.innerHTML = "";
	GameState.DialogButtons.appendChild(elem);
	var bc = CreateButton("Back");
	bc.onclick = DialogBackEvent;
	GameState.DialogButtons.appendChild(bc);
	GameState.DialogType = "Player";
}

function BuildPickTurn() {
	GameState.Descriprion.innerText = "Choose your action";
	var elem = document.createElement("div");
	elem.setAttribute("class", "btn-group-vertical");
	var bt
	if (info.Table.Game.CountHints > 0) {
		bt = CreateButton("Give a hint");
		bt.onclick = PickHintEvent;
		elem.appendChild(bt);
	}
	if (info.Table.Game.CountHints != 8) {
		bt = CreateButton("Drop card");
		bt.onclick = PickDropEvent;
		elem.appendChild(bt);
	}
	bt = CreateButton("Lay card");
	bt.onclick = PickLayEvent;
	elem.appendChild(bt);
	GameState.DialogButtons.innerHTML = "";
	GameState.DialogButtons.appendChild(elem);
	GameState.DialogType = "Turn";
}

function BuildLogIn(type) {
	MainDiv.innerHTML = `
        <form class ="Center" id="Form">
            <div class="form-group">
                <label class="control-label" id="NickLabel">Nick:</label>
                <input type="text" class="form-control" id="nick" placeholder="Your nick"></input>
            </div>
            <div class="form-group">
                <label class ="control-label">Password: </label>
                <input type="password" class="form-control" id="pswd" placeholder="Password"></input>
            </div>
            <p id = "ErrorP" class ="text-danger" style="display:none">Incorrect nick/name or password</p>
            <button type="button" class="btn btn-success" id="LogInButton">Log In</button>
    	<button type="button" class="btn pull-right" id="back" style="width:49%; display:none">Back</button></form>`;
	LogInState.SubmitButton = document.getElementById("LogInButton");
	LogInState.SubmitButton.onclick = LogInEvent;
	LogInState.InputNick = document.getElementById("nick");
	if (type != "Player") {
		LogInState.InputNick.setAttribute("placeholder", "Table's name");
		var lab = document.getElementById("NickLabel");
		lab.innerText = "Table:";
		var BackElem = document.getElementById("back");
		BackElem.setAttribute("style", style = "width:49%;");
		BackElem.onclick = BackEvent;
		if (PickTableState.Type == "Create")
			LogInState.SubmitButton.innerText = "Create";
		else if (PickTableState.Type == "Join")
			LogInState.SubmitButton.innerText = "Join";
	}
	LogInState.InputPassword = document.getElementById("pswd");
	LogInState.Form = document.getElementById("Form");
	LogInState.P = document.getElementById("ErrorP");
}

function BuildPickTable() {
	MainDiv.innerHTML = `
      <div class = "row">
      <div class = "col-xs-9"></div>
      <div class = "col-xs-2"><button class = "btn btn-success"><div id="CurNick" style = "font-weight:bold"></div></button>
      </div>
      <div class = "col-xs-1"></div>

      </div>
    <div style = "width: 100%;height: 65%; position:absolute; top: 35%">
    <div class = "row">
        <div class = "col-sm-4"></div>
        <div class = "col-sm-4">
        <div class = "button-group-vertical">
        <button type = "button" id="btnCreate" class = "btn btn-primary btn-block btn-lg">
            Create new table
        </button>
        <button class = "btn btn-primary btn-block btn-lg" id ="btnJoin">
            Join to exist table
        </button>
        </div>
        </div>
        <div class = "col-sm-4"></div>
    </div>
    </div>`;
	PickTableState.CreateButton = document.getElementById("btnCreate");
	PickTableState.CreateButton.onclick = CreateTableEvent;
	PickTableState.JoinButton = document.getElementById("btnJoin");
	PickTableState.JoinButton.onclick = JoinTableEvent;
	document.getElementById("CurNick").innerText = "Now play: " + PlayerNick;
}

function BuildPrepare() {
	PrepState.OldPlayersList = [];
	PrepState.OldSeats = [];
	MainDiv.innerHTML = `
      <div class = "MainInf">
        <div style="height:50vh">
		  <div style="background: #e8eee0; border-radius: 10px 10px 0px 0px;">
              <div id = "PlList" style = "vertical-align: bottom; overflow: auto; height: 50vh">
              </div>              
          </div>
        </div>
          <div style="clear: both;"></div>
        <div>
         <div id="Adm">
		   <div class="dropdown">
			<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">Game type
			  <span class="caret"></span></button>
				<ul class="dropdown-menu" id="GtL">
				  <li><a href="#"></a></li>
			      <li><a href="#"></a></li>
				  <li><a href="#"></a></li>
				</ul>
			</div>
		    <div class="dropdown">
			<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown">Count of seats
			  <span class="caret"></span></button>
				<ul class="dropdown-menu" id="CntPlL">
				  <li><a href="#"></a></li>
			      <li><a href="#"></a></li>
				  <li><a href="#"></a></li>
				</ul>
			</div>
           <button class="btn btn-success" type="button" id="Start">Start game</button> 
		 </div>
		 <div id="Records">
          <div style="display:inline-block">
			<p style="color=red">Game type:</p>
			<p id="GtP" style="font-size=900; color=red"></p>
	        <button class="btn-warning" type="button" id="Leave">Leave table</button> 
	      </div>
        </div>
		</div>
      </div>
      <div class="Games">
        <div class="Chairs">
          <div class="PlayGame">
            <table><tbody>
                <tr id="Pl1">
                </tr></tbody></table>
            <table><tbody>
              <tr id="Pl2">
              </tr></tbody></table>
		  </div>
        </div>
      </div>
  </body>
</html>`;
	PrepState.PlayersList = document.getElementById("PlList");
	if (info.Table.IdAdmin == PlayerId) {
		PrepState.IsAdmin = true;
		var GameTypes = document.getElementById("GtL");
		for (var i = 0; i < 3; ++i) {
			GameTypes.children[i].children[0].onclick = CreateNiceFun(ChangeGameModeEvent, i);
			GameTypes.children[i].children[0].innerText = GameTypeString[i];
		}
		var CountPlayers = document.getElementById("CntPlL");
		for (var i = 0; i < 3; ++i) {
			CountPlayers.children[i].children[0].onclick = CreateNiceFun(ChangeSeatsEvent, i + 3);
			CountPlayers.children[i].children[0].innerText = i + 3;
		}
		PrepState.StartButton = document.getElementById("Start");
		PrepState.StartButton.onclick = StartGameEvent;
	}
	else {
		PrepState.IsAdmin = false;
		PrepState.StartButton = null;
		var elem = document.getElementById("Adm");
		elem.setAttribute("style", "display:none");
	}
	var LeaveButton = document.getElementById("Leave");
	LeaveButton.onclick = LeaveEvent;
	PrepState.GameTypeP = document.getElementById("GtP");
	PrepState.Seats = [];
}

function BuildPlayer(num) {
	var pl = document.createElement("td");
	var nickElem = CreateElementP(info.NickById[info.Table.Seats[num]]);
	nickElem.setAttribute("class", "Name");
	pl.appendChild(nickElem);
	var CardsDiv = document.createElement("div");
	CardsDiv.setAttribute("style", "width:100%;");
	for (var i = 0; i < 5; ++i) {
		var HideCard = CreateElementP("");
		HideCard.setAttribute("style", "diplay:none;");
		CardsDiv.appendChild(HideCard);
	}
	pl.appendChild(CardsDiv);
	GameState.PlayersCards[num] = CardsDiv;
	GameState.PlayersNick[num] = nickElem;
	GameState.OldCards[num] = [];
	return pl;
}

function BuildGame() {
	GameState.StoryLen = 0;
	GameState.DropLen = 0;
	MainDiv.innerHTML = `
      <div class = "MainInf">
        <div style="height:50vh">
          <div class="tab-content"style="background: #e8eee0; border-radius: 10px 10px 0px 0px;">
              <div id = "History" class="tab-pane fade in active" style = "vertical-align: bottom; overflow: auto; height: 50vh">
              </div>
              <div id="DropCrt" class="tab-pane fade" style = "vertical-align: bottom; overflow: auto; height: 50vh">
			  </div>
			  <div id="IntDrop" class="tab-pane fade" style = "vertical-align: bottom; overflow: auto; height: 50vh">
              </div>
          </div>
          <ul class="nav nav-tabs" style="border-bottom: 1px solid #ddd;
    border-bottom-width: 1px;
    border-bottom-style: solid;
    border-bottom-color: rgb(221, 221, 221);
    margin-bottom: 1vh; display: block;">
            <li class="active" style="display: block;"><a data-toggle = "tab" href = "#History" >History</a></li>
            <li style="display: block;" ><a data-toggle = "tab" href = "#DropCrt">Dropped cards</a></li>
		    <li style="display: block;" ><a data-toggle = "tab" href = "#IntDrop">Intresting</a></li>
          </ul></div>
          <div style="clear: both;"></div>
        <div>
		  <div class="mobileDesc">
			<p class="desc" id=Desk></p>
		  </div>
          <div class="btn-group" id="Btns">
		  </div>
        <div id="Records">
          <div style="display:inline-block;">
			<p>Score:</p>
			<p id="Score"> 0 </p>
		  </div>
		  <div style="display:inline-block;">
			<p>Deck:</p>
			<p id="Deck"> 0 </p>
		  </div>
		  <div style="display:inline-block;">
		    <p>Falls:</p>
			<p id="falls">0</p>
		  </div>
		  <div style="display:inline-block">
				<p>Hints:</p>
				<p id="Hints">8</p>
		  </div>
		  <div style="display:inline-block;">
			<p id="EndDesc"></p>
		  </div>
        </div>
       </div>
      </div>
      <div class="Games">
        <div class="Chairs">
          <div class="PlayGame">
            <table><tbody>
                <tr id="Pl1">
                </tr></tbody></table>
            <table><tbody>
              <tr id="Pl2">
              </tr></tbody></table>
		  </div>
        </div>
        <div class="Table">
          <h2>Table</h2>
          <table>
            <tr id="TableResult">
            </tr>
          </table>
        </div>
        <div style="position:relative;height:5vh">
          <button id="EndGame" class="btn btn-danger" style="position:absolute; right:0;bottom:0">Stop Game</button>
        </div>
      </div>
  </body>
</html>`;
	GameState.ScroeP = document.getElementById("Score");
	GameState.FallsP = document.getElementById("falls");
	GameState.HintsP = document.getElementById("Hints");
	GameState.History = document.getElementById("History");
	GameState.Drops = document.getElementById("DropCrt");
	var Table = document.getElementById("TableResult");
	GameState.Table = [];
	GameState.OldTable = [];
	for (var i = 0; i < info.Table.Game.Table.length; i++) {
		var elem = document.createElement("td");
		var card = GetCard(i, 0);
		card.setAttribute("class", card.getAttribute("class") + " DisActive");
		elem.appendChild(GetCard(i, 0));
		Table.appendChild(elem);
		GameState.Table[i] = elem;
	}
	GameState.YouSeat = 0;
	while (info.Table.Seats[GameState.YouSeat] != PlayerId)
		++GameState.YouSeat;
	GameState.PlayersNick = [];
	GameState.PlayersCards = [];
	GameState.OldCards = [];
	var elem = document.getElementById("Pl1");
	var ind = MyDiv(info.Table.Players.length + 1, 2);
	for (var i = 0; i < ind; i++)
		elem.appendChild(BuildPlayer((i + GameState.YouSeat) % info.Table.Players.length));
	elem = document.getElementById("Pl2");
	for (var i = info.Table.Players.length - 1; i >= ind; i--)
		elem.appendChild(BuildPlayer((i + GameState.YouSeat) % info.Table.Players.length));
	if (info.Table.IdAdmin == PlayerId) {
		GameState.EndGameButton = document.getElementById("EndGame");
		GameState.EndGameButton.onclick = EndGameEvent;
	}
	else {
		var EndGame = document.getElementById("EndGame");
		EndGame.setAttribute("style", "display:none");
		GameState.EndGameButton = null;
	}
	GameState.DialogType = "None";
	GameState.DialogButtons = document.getElementById("Btns");
	GameState.Descriprion = document.getElementById("Desk");
	GameState.CardsInDeck = document.getElementById("Deck");
	GameState.EndGameP = document.getElementById("EndDesc");
	GameState.Interesting = document.getElementById("IntDrop");
}

function ClearMainDiv() {
	while (MainDiv.childElementCount > 0)
		MainDiv.lastChild.remove();
}

function CreateElementP(text) {
	var elem = document.createElement("p");
	elem.innerText = text;
	return elem;
}

function GetSuit(color) {
	var st = "glyphicon glyphicon-";
	var spans = ["cloud", "heart", "tint", "fire", "leaf", "star"];
	spans[-1] = "eye-close";
	st += spans[color];
	var elem = document.createElement("i");
	elem.setAttribute("class", st);
	return elem;
}

function GetCard(color, number) {
	var elem = document.createElement("p");
	if (number >= 0)
		elem.innerText = number;
	else
		elem.innerHTML = "?";
	elem.appendChild(GetSuit(color));
	var classes = ["Viol", "Red", "Blue", "Orange", "Green", "RB"];
	classes[-1] = "UnKn";
	elem.setAttribute("class", classes[color]);
	return elem;
}

function AddToDrop(color, number) {
	var elem = GetCard(color, number);
	GameState.Drops.appendChild(elem);
}

function CreateButton(text) {
	var elem = document.createElement("button");
	elem.setAttribute("class", "btn btn-success");
	elem.setAttribute("type", "button");
	elem.innerText = text;
	return elem;
}

function CreateNiceFun(CallBack, par) {
	var hlp = function (inp) {
		var num = inp;
		return function () {
			CallBack(num);
		}
	}
	return hlp(par);
}

function MyDiv(x, y) {
	return (x - x % y) / y;
}