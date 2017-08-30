using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi
{
    public class Card : ICard
    {
        public int Color { get; private set; }
        public int Number { get; private set; }
        public bool KnowColor { get; private set; }
        public bool KnowNumber { get; private set; }

        public Card(int Color, int Number)
        {
            this.Color = Color;
            this.Number = Number;
            KnowColor = KnowNumber = false;
        }

        public void ReciveHintColor(int NumColor)
        {
            KnowColor = KnowColor || (NumColor == Color) || (Color == 5);
        }
        public void ReciveHintNumber(int Number)
        {
            KnowNumber = KnowNumber || (Number == this.Number);
        }
    }

    public interface ICard
    {
        int Color { get; }
        int Number { get; }
        bool KnowColor { get; }
        bool KnowNumber { get; }
        void ReciveHintColor(int NumColor);
        void ReciveHintNumber(int Number);
    }
}
