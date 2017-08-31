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
        private Dictionary<int, WebTable> Tables;
        private Dictionary<string, int> GetIdByNick;//player
        private Dictionary<string, int> GetIdByName;//table
        private bool NickIsGood(string nick)
        {
            if (nick.Length == 0 || nick.Length > 15)
                return false;
            bool ans = true;
            for (int i = 0; i < nick.Length; ++i)
                ans = ans && ((nick[i] >= 'a' && nick[i] <= 'z') || (nick[i] >= 'A' && nick[i] <= 'Z') || (nick[i] >= '0' && nick[i] <= '9') || (nick[i] == '_'));
            return ans;
        }
        private bool PassIsGood(string nick)
        {
            if (nick.Length < 6)
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
            if (!NickIsGood(nick) || !PassIsGood(pass))
                return -1;
            int id = Utily.GetTag();
            GetIdByNick.Add(nick, id);
            Accounts.Add(id, new WebPlayer(nick, pass, id));
            return id;
        }
        public int CreateTable(int idPlayer, string name, string pass)
        {
            if (GetIdByName.ContainsKey(name))
                return -1;
            if (!NickIsGood(name) || !PassIsGood(pass))
                return -1;
            int id = Utily.Next();
            Tables.Add(id, new WebTable(idPlayer));
            GetIdByName.Add(name, id);
            Accounts[idPlayer].JoinTable(id);
            return id;
        }
        public bool CheckPassPlayer(int id, string pass)
        {
            return Accounts.ContainsKey(id) && Accounts[id].Password == pass;
        }
        public bool CheckPassTable(int id, string pass)
        {
            return Tables.ContainsKey(id) && Tables[id].Password == pass;
        }
        public int JoinTable(int idPlayer, string name, string pass)
        {
            if (!GetIdByName.ContainsKey(name))
                return -1;
            int id = GetIdByName[name];
            if (Tables[id].Password != pass)
                return -1;
            if (Tables[id].JoinPlayer(idPlayer))
            {
                Accounts[idPlayer].JoinTable(id);
                return id;
            }
            return -1;
        }
        public bool LeaveTable(int idPlayer)
        {
            if (Accounts[idPlayer].NowPlay)
            {
                int id = Accounts[idPlayer].TableId;
                if (Tables[id].IdAdmin == idPlayer)
                {
                    
                }
                Tables[Accounts[idPlayer].TableId].LeavePlayer(idPlayer);
                Accounts[idPlayer].LeaveTable();
            }
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