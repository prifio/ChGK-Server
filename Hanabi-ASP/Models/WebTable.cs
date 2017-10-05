using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanabi_ASP.Models
{
    public class WebTable
    {
        const int MaxPlayers = 5;
        const int MinPlayers = 3;
        public bool GameStarted { get; private set; }
        private Hanabi.IGame TableGame; 
        private int SeatCountp;
        public int SeatCount
        {
            get
            {
                return SeatCountp;
            }
            private set
            {
                if (value >= MinPlayers && value <= MaxPlayers)
                {
                    if (value < SeatCountp)
                        Seats.RemoveRange(value, SeatCountp - value);
                    else
                        for (int i = 0; i < value - SeatCountp; i++)
                        {
                            Seats.Add(-1);
                        }
                    SeatCountp = value;
                }
            }
        }
        public int IdAdmin { get; private set; }
        public string Password { get; private set; }
        public Hanabi.GameType CurrentGameType { get; private set; }
        public string Name { get; private set; }
        private HashSet<int> Players;
        private List<int> Seats;

        public WebTable(int idAdmin, string pass, string Name)
        {
            this.Name = Name;
            this.IdAdmin = idAdmin;
            Players = new HashSet<int>();
            Seats = new List<int>();
            SeatCount = 3;
            Players.Add(idAdmin);
            Seats[0] = idAdmin;
            GameStarted = false;
            TableGame = null;
            CurrentGameType = Hanabi.GameType.FiveColor;
            Password = pass;
        }
        public bool JoinPlayer(int id)
        {
            if (Players.Count >= MaxPlayers)
                return false;
            if (Players.Contains(id) || GameStarted)
                return false;
            Players.Add(id);
            return true;
        }
        public bool LeavePlayer(int id)
        {
            if (Players.Contains(id) && !GameStarted)
            {
                Players.Remove(id);
                for (int i = 0; i < SeatCount; ++i)
                    if (Seats[i] == id)
                        Seats[i] = -1;
                return true;
            }
            return false;
        }
        public bool PlayerSitDown(int id, int placeNumber)
        {
            if (placeNumber < 0 || placeNumber >= SeatCount || !Players.Contains(id) || GameStarted)
                return false;
            for (int i = 0; i < SeatCount; i++)
            {
                if (Seats[i] == id)
                    Seats[i] = -1;
            }
            Seats[placeNumber] = id;
            return true;
        }
        public bool PlayerStandUp(int id)
        {
            if (GameStarted)
                return false;
            var ans = false;
            for (int i = 0; i < SeatCount; ++i)
            {
                ans = ans || Seats[i] == id;
                if (Seats[i] == id)
                    Seats[i] = -1;
            }
            return ans;
        }
        public bool EndGame()
        {
            if (!GameStarted)
                return false;
            TableGame = null;
            GameStarted = false;
            return true;
        }
        public bool StartGame()
        {
            if (GameStarted)
                return false;
            var isGood = true;
            for (int i = 0; i < SeatCount; i++)
                isGood = isGood && Seats[i] != -1;
            isGood = isGood && Players.Count == SeatCount;
            if (!isGood)
                return false;
            GameStarted = true;
            TableGame = new Hanabi.Game(CurrentGameType, Players.Count);
            return true;
        }
        public bool PlayerJoined(int id)
        {
            return Players.Contains(id);
        }
        public int PlayerIdByPlace(int num)
        {
            if (num < 0 || num >= SeatCount)
                return -1;
            return Seats[num];
        }
        public bool UseHintColor(int id, int numPlayer, int numColor)
        {
            if (!GameStarted || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.UseHintColor(numPlayer, numColor);
        }
        public bool UseHintNumber(int id, int numPlayer, int Number)
        {
            if (!GameStarted || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.UseHintNumber(numPlayer, Number);
        }
        public bool PlaceCard(int id, int numCard)
        {
            if (!GameStarted || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.PlaceCard(numCard);
        }
        public bool DropCard(int id, int numCard)
        {
            if (!GameStarted || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.DropCard(numCard);
        }
        public int[] ListPlayers()
        {
            return Players.ToArray();
        }
        public bool ChangeGameType(int i)
        {
            if (i < 0 || i > 2 || GameStarted)
                return false;
            CurrentGameType = (Hanabi.GameType)i;
            return true;
        }
        public bool ChangeSeatsCount(int cnt)
        {
            if (GameStarted || cnt > MaxPlayers || cnt < MinPlayers)
                return false;
            SeatCount = cnt;
            return true;
        }
        public TableInfo GetTableInfo(int idPlayer)
        {
            var ans = new TableInfo();
            ans.Players = Players.ToArray();
            ans.Seats = new int[SeatCount];
            for (int i = 0; i < SeatCount; ++i)
                ans.Seats[i] = Seats[i];
            if (GameStarted)
            {
                int numPlayer = -1;
                for (int i = 0; i < SeatCount; ++i)
                    if (idPlayer == Seats[i])
                        numPlayer = i;
                ans.Game = TableGame.GetGameInfo(numPlayer);
            }
            else
                ans.Game = null;
            ans.GameStarted = GameStarted;
            ans.IdAdmin = IdAdmin;
            ans.CurrentGameType = CurrentGameType;
            return ans;
        }
    }

    public class TableInfo
    {
        public int[] Players { get; set; }
        public int[] Seats { get; set; }
        public Hanabi.GameType CurrentGameType { get; set; }
        public bool GameStarted { get; set; }
        public int IdAdmin { get; set; }
        public Hanabi.GameInfo Game { get; set; }
    }
}