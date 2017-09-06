using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using Hanabi;

namespace Hanabi_ASP.Controllers
{
    public class apiController : ApiController
    {
        static Models.Server MainServer = new Models.Server();

        [HttpPost]
        public int LogIn([FromBody] LogInInfo s)
        {
            return MainServer.LogIn(s.Nick, s.Password);
        }

        [HttpPost]
        public int CreateTable([FromBody] TableInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return -1;
            return MainServer.CreateTable(s.idPlayer, s.TableName, s.TablePassword);
        }

        [HttpPost]
        public int JoinTable([FromBody] TableInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return -1;
            return MainServer.JoinTable(s.idPlayer, s.TableName, s.TablePassword);
        }

        [HttpPost]
        public bool LeaveTable([FromBody] PlayerInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.LeaveTable(s.idPlayer);
        }

        [HttpPost]
        public bool SitDown([FromBody] SitDownInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.SitDown(s.idPlayer, s.numSeat);
        }

        [HttpPost]
        public bool StandUp([FromBody] PlayerInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.StandUp(s.idPlayer);
        }

        [HttpPost]
        public bool ForceStandUp([FromBody] ForceStandUpInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.ForceStandUp(s.idPlayer, s.idStandingPlayer);
        }

        [HttpPost]
        public bool KickPlayer([FromBody] KickInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.KickPlayer(s.idPlayer, s.idKicksPlayer);
        }

        [HttpPost]
        public bool ChangeGameType([FromBody] GameTypeInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.ChangeGameType(s.idPlayer, s.newGameType);
        }

        [HttpPost]
        public bool ChangeSeatsCount([FromBody] ChangeSeatsInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.ChangeGameType(s.idPlayer, s.SeatsInfo);
        }

        [HttpPost]
        public bool StartGame([FromBody] PlayerInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.StartGame(s.idPlayer);
        }

        [HttpPost]
        public bool EndGame([FromBody] PlayerInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.EndGame(s.idPlayer);
        }

        [HttpPost]
        public bool HintColor([FromBody] HintColorInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.UseHintColor(s.idPlayer, s.numPlayer, s.numColor);
        }

        [HttpPost]
        public bool HintNumber([FromBody] HintNumberInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.UseHintNumber(s.idPlayer, s.numPlayer, s.Number);
        }

        [HttpPost]
        public bool PlaceCard([FromBody] TurnCardInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.PlaceCard(s.idPlayer, s.numCard);
        }

        [HttpPost]
        public bool DropCard([FromBody] TurnCardInfo s)
        {
            if (!MainServer.CheckPassPlayer(s.idPlayer, s.PlayerPassword))
                return false;
            return MainServer.DropCard(s.idPlayer, s.numCard);
        }

        [HttpGet]
        public Hanabi_ASP.Models.ServerInfo GetInfo(int idPlayer, string PlayerPassword)
        {
            if (!MainServer.CheckPassPlayer(idPlayer, PlayerPassword))
                return null;
            return MainServer.Get(idPlayer);
        }
    }

    public class TurnCardInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int numCard { get; set; }
    }

    public class HintNumberInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int Number { get; set; }
        public int numPlayer { get; set; }
    }
    
    public class HintColorInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int numColor { get; set; }
        public int numPlayer { get; set; }
    }

    public class ChangeSeatsInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int SeatsInfo { get; set; }
    }
    
    public class GameTypeInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int newGameType { get; set; }
    }
    
    public class KickInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int idKicksPlayer { get; set; }
    }

    public class ForceStandUpInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int idStandingPlayer { get; set; }
    }

    public class SitDownInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        public int numSeat { get; set; }
    }

    public class PlayerInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
    }

    public class TableInfo
    {
        public int idPlayer { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlayerPassword { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TableName { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TablePassword { get; set; }
    }

    
    public class LogInInfo
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Nick { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }
    }
}
