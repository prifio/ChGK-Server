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
        private List<ICard> Cards;

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
            return Cards[i];
        }
        public void ReceiveHintColor(int numColor)
        {
            for (int i = 0; i < Cards.Count; ++i)
                Cards[i].ReceiveHintColor(numColor);
        }
        public void ReceiveHintNumber(int numColor)
        {
            for (int i = 0; i < Cards.Count; ++i)
                Cards[i].ReceiveHintNumber(numColor);
        }
        public PlayerInfo GetInfo(bool HideCard)
        {
            var ans = new PlayerInfo();
            ans.Cards = new ICard[Cards.Count];
            for (int i = 0; i < Cards.Count; ++i)
                ans.Cards[i] = Cards[i].GetInfo(HideCard);
            return ans;
        }
    }

    public interface IPlayer
    {
        int CountCard { get; }
        ICard DropCard(int num);
        void TakeCard(ICard NewCard);
        ICard CardByIndex(int i);
        void ReceiveHintColor(int numColor);
        void ReceiveHintNumber(int numColor);
        PlayerInfo GetInfo(bool HideCard);
    }
    
    public class PlayerInfo
    {
        public ICard[] Cards { get; set; }
    }
}
