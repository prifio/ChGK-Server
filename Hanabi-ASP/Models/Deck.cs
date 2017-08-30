using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanabi
{
    public class Deck : IDeck
    {
        public int CountCards
        {
            get
            {
                return CardQueue.Count;
            }
        }

        private Queue<ICard> CardQueue;
        
        public Deck(GameType gt)
        {
            int len;
            CardQueue = new Queue<ICard>();
            if (gt == GameType.FiveColor)
                len = 50;
            else
                len = 55;
            int[] seq = new int[len];
            for (int i = 0; i < len; i++)
                seq[i] = i;
            for (int i = 1; i < len; ++i)
                Utily.Swap<int>(ref seq[i], ref seq[Utily.Next() % (i + 1)]);
            for (int i = 0; i < len; i++)
            {
                if (seq[i] >= 50)
                    CardQueue.Enqueue(new Card(5, seq[i] - 49));
                else
                {
                    int color = seq[i] / 10;
                    int num = seq[i] % 10;
                    if (num <= 2)
                        num = 1;
                    else if (num == 9)
                        num = 5;
                    else
                        num = (num - 1) / 2;
                    CardQueue.Enqueue(new Card(color, num));
                }
            }
        }

        public ICard GetNext()
        {
            return CardQueue.Dequeue();
        }
    }

    public interface IDeck
    {
        int CountCards { get; }
        ICard GetNext();
    }
}
