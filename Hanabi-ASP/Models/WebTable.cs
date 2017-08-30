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
        public Hanabi.IGame TableGame { get; private set; }
        private int SeatCountp;
        public int SeatCount
        {
            get
            {
                return SeatCount;
            }
            set
            {
                if (value >= MinPlayers && value <= MaxPlayers)
                    SeatCountp = value;
            }
        }
        public int IdAdmin { get; private set; }

        public WebTable(int idAdmin)
        {

        }
        public bool JoinPlayer(int id)
        {
            return true;
        }
        public bool LeavePlayer(int id)
        {
            return true;
        }
        public bool PlayerSit(int id, int placeNumber)
        {
            return true;
        }
        public bool PlayerStandUp(int id)
        {
            return true;
        }
        public bool EndGame()
        {
            return true;
        }
        public bool StartGame()
        {
            return true;
        }
    }

    public class TableInfo
    {

    }
}