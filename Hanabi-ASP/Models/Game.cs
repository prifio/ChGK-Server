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
        public int DropCount {
            get {
                return DropsCards.Count;
            }
        }
        private GameStory Story;
        private IDeck GameDeck;
        private List<ICard> DropsCards;
        private IPlayer[] Players;
        private int CardsOnHand;
        private bool LastCicle;
        private int LostTurn;

        private ICard TryTakeCard()
        {
            if (GameDeck.CountCards > 0)
            {
                if (GameDeck.CountCards == 1)
                {
                    LastCicle = true;
                    LostTurn = CountPlayers + 1;//+1 bcz this turn add to calc
                }
                return GameDeck.GetNext();
            }
            return null;
        }
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
            DropsCards = new List<ICard>();
            Story = new GameStory();
        }
        public ICard DropIndexCard(int NumCard)
        {
            return DropsCards[NumCard];
        }
        public bool UseHintColor(int NumPlayer, int NumColor)
        {
            if (CountHints == 0 || NumColor > 5 || (NumColor == 5 && CurrentGameType != GameType.RainbowIsNewColor) || NumPlayer == CurrentPlayer || NumColor < 0 || NumPlayer < 0 || NumPlayer >= CountPlayers || GameIsEnd)
                return false;
            Players[NumPlayer].ReceiveHintColor(NumColor);
            CountHints--;
            Story.HintColor(CurrentPlayer, NumPlayer, NumColor);
            UpdateAfter();
            return true;
        }
        public bool UseHintNumber(int NumPlayer, int Number)
        {
            if (CountHints == 0 || NumPlayer == CurrentPlayer || Number > 5 || Number <= 0 || NumPlayer < 0 || NumPlayer >= CountPlayers || GameIsEnd)
                return false;
            Players[NumPlayer].ReceiveHintNumber(Number);
            CountHints--;
            UpdateAfter();
            Story.Hintnumber(CurrentPlayer, NumPlayer, Number);
            return true;
        }
        public bool PlaceCard(int NumCard)
        {
            if (NumCard <= 0 || NumCard > 5 || GameIsEnd)
                return false;
            ICard cards = Players[CountPlayers].CardByIndex(NumCard);
            if (Tabel[cards.Color] + 1 == cards.Number)
            {
                Story.PlaceCard(CurrentPlayer, cards.Color, cards.Number);
                Tabel[cards.Color]++;
                Result++;
            }
            else
            {
                Story.MakeFall(CurrentPlayer, cards.Color, cards.Number);
                DropsCards.Add(cards);
                CountFall++;
                if (CountFall == 3)
                    GameIsEnd = true;
            }
            ICard next = TryTakeCard();
            if (next != null)
                Players[CurrentPlayer].TakeCard(next);
            UpdateAfter();
            return true;
        }
        public bool DropCard(int NumCard)
        {
            if (NumCard >= CardsOnHand || NumCard < 0 || GameIsEnd || CountHints == 8)
                return false;
            var cards = Players[CurrentPlayer].DropCard(NumCard);
            DropsCards.Add(cards);
            CountHints++;
            ICard next = TryTakeCard();
            if (next != null)
                Players[CurrentPlayer].TakeCard(next);
            Story.DropCard(CurrentPlayer, cards.Color, cards.Number);
            UpdateAfter();
            return true;
        }
        public GameInfo GetGameInfo(int numPlayer)
        {
            var ans = new GameInfo();
            ans.CurrentGameType = CurrentGameType;
            ans.CountFall = CountFall;
            ans.CountHints = CountHints;
            ans.Result = Result;
            ans.GameIsEnd = GameIsEnd;
            ans.Table = new int[Tabel.Length];
            for (int i = 0; i < Tabel.Length; i++)
                ans.Table[i] = Tabel[i];
            ans.DropsCards = new ICard[DropsCards.Count];
            for (int i = 0; i < DropsCards.Count; i++)
                ans.DropsCards[i] = new Card(DropsCards[i].Color, DropsCards[i].Number, CurrentGameType);
            ans.Players = new PlayerInfo[CountPlayers];
            for (int i = 0; i < CountPlayers; ++i)
            {
                ans.Players[i] = Players[i].GetInfo(i == numPlayer);
            }
            ans.Story = Story.GetInfo();
            return ans;
        }
        private void UpdateAfter()
        {
            if (LastCicle)
                --LostTurn;
            GameIsEnd = GameIsEnd || (LastCicle && LostTurn == 0);
            CurrentPlayer = (CurrentPlayer + 1) % CountPlayers;
        }
    }

    public interface IGame
    {
        int DropCount { get; }
        int CurrentPlayer { get; }
        int CountPlayers { get; }
        int CountHints { get; }
        int CountFall { get; }
        int Result { get; }
        bool GameIsEnd { get; }
        int[] Tabel { get; }
        GameType CurrentGameType { get; }
        ICard DropIndexCard(int NumCard);
        bool UseHintColor(int NumPlayer, int NumColor);
        bool UseHintNumber(int NumPlayer, int Number);
        bool PlaceCard(int NumCard);
        bool DropCard(int NumCard);
        GameInfo GetGameInfo(int numPlayer);
        //return true if input-data is OK
    }

    public enum GameType
    {
        FiveColor,
        RainbowIsNewColor,
        RainbowIsEvery
    }
    
    public class GameInfo
    {
        public int CurrentPlayer { get; set; }
        public int CountHints { get; set; }
        public int CountFall { get; set; }
        public int Result { get; set; }
        public bool GameIsEnd { get; set; }
        public int[] Table { get; set; }
        public GameType CurrentGameType { get; set; }
        public ICard[] DropsCards { get; set; }
        public PlayerInfo[] Players { get; set; }
        public Event[] Story { get; set; }
    }
}
