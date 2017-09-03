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
        public Server()
        {
            Accounts = new Dictionary<int, WebPlayer>();
            Tables = new Dictionary<int, WebTable>();
            GetIdByName = new Dictionary<string, int>();
            GetIdByNick = new Dictionary<string, int>();
        }
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
                if (Tables[id].GameStarted)
                    return false;
                if (Tables[id].IdAdmin == idPlayer)
                {
                    int[] pls = Tables[id].ListPlayers();
                    for (int i = 0; i < pls.Length; ++i)
                    {
                        Accounts[pls[i]].LeaveTable();
                    }
                    Tables.Remove(id);
                    return true;
                }
                else
                {
                    if (!Tables[Accounts[idPlayer].TableId].LeavePlayer(idPlayer))
                        return false;
                    Accounts[idPlayer].LeaveTable();
                    return true;
                }
            }
            return false;
        }
        public bool UseHintColor(int idPlayer, int numPlayer, int numColor)
        {
            if (Accounts[idPlayer].NowPlay)
                return Tables[Accounts[idPlayer].TableId].UseHintColor(idPlayer, numPlayer, numColor);
            return false;
        }
        public bool UseHintNumber(int idPlayer, int numPlayer, int Number)
        {
            if (Accounts[idPlayer].NowPlay)
                return Tables[Accounts[idPlayer].TableId].UseHintNumber(idPlayer, numPlayer, Number);
            return false;
        }
        public bool PlaceCard(int idPlayer, int numPlayer, int NumCard)
        {
            if (Accounts[idPlayer].NowPlay)
                return Tables[Accounts[idPlayer].TableId].PlaceCard(idPlayer, NumCard);
            return false;
        }
        public bool DropCard(int idPlayer, int numPlayer, int NumCard)
        {
            if (Accounts[idPlayer].NowPlay)
                return Tables[Accounts[idPlayer].TableId].DropCard(idPlayer, NumCard);
            return false;
        }
        public bool StartGame(int idPlayer)
        {
            if (!Accounts[idPlayer].NowPlay)
                return false;
            int id = Accounts[idPlayer].TableId;
            if (Tables[id].IdAdmin != idPlayer)
                return false;
            return Tables[id].StartGame();
        }
        public bool EndGame(int idPlayer)
        {
            if (!Accounts[idPlayer].NowPlay)
                return false;
            int id = Accounts[idPlayer].TableId;
            if (Tables[id].IdAdmin != idPlayer)
                return false;
            return Tables[id].EndGame();
        }
        public bool KickPlayer(int idHow, int idKicks)
        {
            if (!Accounts[idHow].NowPlay)
                return false;
            int id = Accounts[idHow].TableId;
            if (Tables[id].IdAdmin != idHow)
                return false;
            return Tables[id].LeavePlayer(idKicks);
        }
        public bool ForceStandUp(int idHow, int idKicks)
        {
            if (!Accounts[idHow].NowPlay)
                return false;
            int id = Accounts[idHow].TableId;
            if (Tables[id].IdAdmin != idHow)
                return false;
            return Tables[id].PlayerStandUp(idKicks);
        }
        public bool ChangeGameType(int idPlayer, int NewGameType)
        {
            if (!Accounts[idPlayer].NowPlay || Tables[Accounts[idPlayer].TableId].IdAdmin != idPlayer)
                return false;
            return Tables[Accounts[idPlayer].TableId].ChangeGameType(NewGameType);
        }
        public bool ChangeSeatsCount(int idPlayer, int NewCount)
        {
            if (!Accounts[idPlayer].NowPlay || Tables[Accounts[idPlayer].TableId].IdAdmin != idPlayer)
                return false;
            return Tables[Accounts[idPlayer].TableId].ChangeSeatsCount(NewCount);
        }
        public ServerInfo Get(int idPlayer)
        {
            var ans = new ServerInfo();
            ans.IdPlayer = idPlayer;
            ans.NickById = new Dictionary<int, string>();
            if (Accounts[idPlayer].NowPlay)
            {
                ans.IdTabel = Accounts[idPlayer].TableId;
                ans.Table = Tables[Accounts[idPlayer].TableId].GetTableInfo(idPlayer);
                for (int i = 0; i < ans.Table.Players.Length; ++i)
                    ans.NickById.Add(ans.Table.Players[i], Accounts[ans.Table.Players[i]].Nick);
            }
            else
            {
                ans.IdTabel = -1;
                ans.Table = null;
                ans.NickById.Add(idPlayer, Accounts[idPlayer].Nick);
            }
            return null;
        }
    }

    public class ServerInfo
    {
        public int IdPlayer { get; set; }
        public int IdTabel { get; set; }
        public Dictionary<int, string> NickById { get; set; }
        public TableInfo Table { get; set; }
    }
}