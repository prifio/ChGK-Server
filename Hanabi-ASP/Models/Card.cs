using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi
{
    public class Card : ICard
    {
        public GameType CurrentGameType { get; private set; }
        public int Color { get; private set; }
        public int Number { get; private set; }
        public int KnowColor { get; private set; }
        public int KnowNumber { get; private set; }


        public Card(int Color, int Number, GameType CurrentGameType)
        {
            this.Color = Color;
            this.Number = Number;
            this.CurrentGameType = CurrentGameType;
            KnowColor = -1;
            KnowNumber = -1;
        }
        public void ReceiveHintColor(int NumColor)
        {
            if ((Color == NumColor || (KnowColor == -1 && Color == 5 && CurrentGameType == GameType.RainbowIsEvery)))
                KnowColor = NumColor;
            else if(Color == 5 && CurrentGameType == GameType.RainbowIsEvery && KnowColor != Color)
                KnowColor = 5;
        }
        public void ReceiveHintNumber(int Number)
        {
            if (this.Number == Number)
                KnowNumber = Number;
        }
        private Card(int Color, int Number, int KnowColor, int KnowNumber)
        {
            this.Color = Color;
            this.Number = Number;
            this.KnowColor = KnowColor;
            this.KnowNumber = KnowNumber;
        }
        public ICard GetInfo(bool HideCard)
        {
            if (HideCard)
                return new Card(-1, -1, KnowColor, KnowNumber);
            return new Card(Color, Number, KnowColor, KnowNumber);
        }
    }

    public interface ICard
    {
        GameType CurrentGameType { get; }
        int Color { get; }
        int Number { get; }
        int KnowColor { get; }
        int KnowNumber { get; }
        void ReceiveHintColor(int NumColor);
        void ReceiveHintNumber(int Number);
        ICard GetInfo(bool HideCard);
    }
}
