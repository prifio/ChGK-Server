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
        public bool GameIsStarter { get; private set; }
        private Hanabi.IGame TableGame;
        private int SeatCountp;
        public int SeatCount
        {
            get
            {
                return SeatCount;
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
        public Hanabi.GameType CurrentGameType { get; set; }
        private HashSet<int> Players;
        private List<int> Seats;

        public WebTable(int idAdmin)
        {
            this.IdAdmin = idAdmin;
            Players = new HashSet<int>();
            Seats = new List<int>();
            SeatCount = 3;
            Players.Add(idAdmin);
            Seats[0] = idAdmin;
            GameIsStarter = false;
            TableGame = null;
            CurrentGameType = Hanabi.GameType.FiveColor;
        }
        public bool JoinPlayer(int id)
        {
            if (Players.Count >= MaxPlayers)
                return false;
            if (Players.Contains(id) || GameIsStarter)
                return false;
            Players.Add(id);
            return true;
        }
        public bool LeavePlayer(int id)
        {
            if (Players.Contains(id) && GameIsStarter)
            {
                Players.Remove(id);
                return true;
            }
            return false;
        }
        public bool PlayerSitDown(int id, int placeNumber)
        {
            if (placeNumber < 0 || placeNumber >= SeatCount || !Players.Contains(id) || GameIsStarter)
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
            if (GameIsStarter)
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
            if (!GameIsStarter)
                return false;
            TableGame = null;
            GameIsStarter = false;
            return true;
        }
        public bool StartGame()
        {
            if (GameIsStarter)
                return false;
            var isGood = true;
            for (int i = 0; i < SeatCount; i++)
                isGood = isGood && Seats[i] != -1;
            isGood = isGood && Players.Count == SeatCount;
            if (!isGood)
                return false;
            GameIsStarter = true;
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
            if (!GameIsStarter || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.UseHintColor(numPlayer, numColor);
        }
        public bool UseHintNumber(int id, int numPlayer, int Number)
        {
            if (!GameIsStarter || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.UseHintNumber(numPlayer, Number);
        }
        public bool PlaceCard(int id,  int numCard)
        {
            if (!GameIsStarter || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.PlaceCard(numCard);
        }
        public bool DropCard(int id, int numCard)
        {
            if (!GameIsStarter || Seats[TableGame.CurrentPlayer] != id)
                return false;
            return TableGame.DropCard(numCard);
        }
    }

    public class TableInfo
    {

    }
}