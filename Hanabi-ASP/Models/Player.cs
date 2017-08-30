using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi
{
    public class Player : IPlayer
    {
        public int CountCard
        {
            get
            {
                return Cards.Count;
            }
        }
        public List<ICard> Cards;

        public Player()
        {
            Cards = new List<ICard>();
        }
        public ICard DropCard(int index)
        {
            var ans = Cards[index];
            Cards.RemoveAt(index);
            return ans;
        }
        public void TakeCard(ICard NewCard)
        {
            Cards.Add(NewCard);
        }
        public ICard CardByIndex(int i)
        {
            return Cards[0];
        }
        public void ReciveHintColor(int numColor)
        {
            for (int i = 0; i < Cards.Count; ++i)
                Cards[i].ReciveHintColor(numColor);
        }
        public void ReciveHintNumber(int numColor)
        {
            for (int i = 0; i < Cards.Count; ++i)
                Cards[i].ReciveHintNumber(numColor);
        }
    }

    public interface IPlayer
    {
        int CountCard { get; }
        ICard DropCard(int num);
        void TakeCard(ICard NewCard);
        ICard CardByIndex(int i);
        void ReciveHintColor(int numColor);
        void ReciveHintNumber(int numColor);
    }
}
