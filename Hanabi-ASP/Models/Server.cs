using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hanabi;

namespace Hanabi_ASP.Models
{
    public class Server
    {
        private Dictionary<int, WebPlayer> Accounts;
        private Dictionary<int, WebPlayer> Tables;
        private Dictionary<string, int> GetIdByNick;
        private bool NickIsGood(string nick)
        {
            if (nick.Length == 0 || nick.Length > 15)
                return false;
            bool ans = true;
            for (int i = 0; i < nick.Length; ++i)
                ans = ans && ((nick[i] >= 'a' && nick[i] <= 'z') || (nick[i] >= 'A' && nick[i] <= 'Z') || (nick[i] >= '0' && nick[i] <= '9') || (nick[i] == '_'));
            return ans;
        }
        public int LogIn(string nick, string pass)
        {
            if (GetIdByNick.ContainsKey(nick))
            {
                if (Accounts[GetIdByNick[nick]].Password == pass)
                    return GetIdByNick[nick];
                else
                    return -1;
            }    
            if (!NickIsGood(nick))
                return -1;
            int id = Utily.GetTag();
            GetIdByNick[nick] = id;
            Accounts.Add(id, new WebPlayer(nick, pass, id));
            return id;
        }
        public bool CreateTable(int idPlayer, string name, string pass)
        {
            return true;
        }
        public bool CheckPassPlayer(int id, string pass)
        {
            return Accounts.ContainsKey(id) && Accounts[id].Password == pass;
        }
        public bool CheckPassTable(int id, string pass)
        {
            return true;
        }
        public bool JoinTable(int idPlayer, string name, string pass)
        {
            return true;
        }
        public bool LeaveTable(int idPlayer)
        {
            return true;
        }
        public bool UseHintColor(int idPlayer, int numPlayer, int numColor)
        {
            return true;
        }
        public bool UseHintNumber(int idPlayer, int numPlayer, int Number)
        {
            return true;
        }
        public bool PlaceCard(int idPlayer, int numPlayer, int NumCard)
        {
            return true;
        }
        public bool DropCard(int idPlayer, int numPlayer, int NumCard)
        {
            return true;
        }
        public TableInfo Get(int idPlayer)
        {
            return null;
        }
    }
}