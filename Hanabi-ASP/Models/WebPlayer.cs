using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanabi_ASP.Models
{
    public class WebPlayer
    {
        public int Id { get; private set; }
        public string Nick { get; private set; }
        public string Password { get; private set; }
        public bool NowPlay { get; private set; }
        public int TableId { get; private set; }

        public void JoinTable(int id)
        {
            NowPlay = true;
            TableId = id;
        }
        public void LeaveTable()
        {
            NowPlay = false;
            TableId = -1;
        }
        public WebPlayer(string nick, string pass, int id)
        {
            this.Id = id;
            this.Nick = nick;
            this.Password = pass;
            NowPlay = false;
            TableId = -1;
        }
    }
}