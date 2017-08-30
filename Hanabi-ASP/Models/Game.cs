using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi
{
    public class Game :IGame
    {
        public int CurrentPlayer { get; private set; }
        public int CountPlayers { get; private set; }
        public int CountHints { get; private set; }
        public int CountFall { get; private set; }
        public int Result { get; private set; }
        public bool GameIsEnd { get; private set; }
        public int[] Tabel { get; private set; }
        public GameType CurrentGameType { get; private set; }
        private IDeck GameDeck;
        private IPlayer[] Players;
        private int CardsOnHand;

        public Game(GameType gt, int CountPlayers)
        {
            this.CountPlayers = CountPlayers;
            this.CurrentGameType = gt;
            this.GameDeck = new Deck(gt);
            CurrentPlayer = Utily.Next() % CountPlayers;
            Players = new IPlayer[CountPlayers];
            if (CountPlayers == 3)
                CardsOnHand = 5;
            else
                CardsOnHand = 4;
            for (int i = 0; i < CountPlayers; ++i)
            {
                Players[i] = new Player();
                for (int j = 0; j < CardsOnHand; ++j)
                    Players[i].TakeCard(GameDeck.GetNext());
            }
            CountHints = 8;
            CountFall = 0;
            Result = 0;
            GameIsEnd = false;
            int countColors = 6;
            if (gt == GameType.FiveColor)
                countColors = 5;
            Tabel = new int[countColors];
        }

        public bool UseHintColor(int NumPlayer, int NumColor)
        {
            return true;
        }
        public bool UseHintNumber(int NumPlayer, int Number)
        {
            return true;
        }
        public bool PlaceCard(int NumCard)
        {
            return true;
        }
        public bool DropCard(int NumCard)
        {
            return true;
        }
    }

    public interface IGame
    {
        int CurrentPlayer { get; }
        int CountPlayers { get; }
        int CountHints { get; }
        int CountFall { get; }
        int Result { get; }
        bool GameIsEnd { get; }
        int[] Tabel { get; }
        GameType CurrentGameType { get; }
        bool UseHintColor(int NumPlayer, int NumColor);
        bool UseHintNumber(int NumPlayer, int Number);
        bool PlaceCard(int NumCard);
        bool DropCard(int NumCard);
        //return true if input-data is OK
    }
    public enum GameType
    {
        FiveColor,
        RainbowIsNewColor,
        RainbowIsEvery
    }
}
