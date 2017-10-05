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
        
        private bool CardBad(int ch)
        {
            return ch >= 50 && ch <= 53;
        }

        public Deck(GameType gt)
        {
            CardQueue = new Queue<ICard>();
            int len;
            if (gt == GameType.FiveColor)
                len = 50;
            else
                len = 55;
            int[] seq = new int[len];
            for (int i = 0; i < len; i++)
                seq[i] = i;
            for (int i = 1; i < len; ++i)
                Utily.Swap<int>(ref seq[i], ref seq[Utily.Next() % (i + 1)]);
            int mv = 0;
            while (CardBad(seq[mv]))
                ++mv;
            for (int j = 0; j < len; j++)
            {
                int i = (j + mv) % len;
                if (seq[i] >= 50)
                    CardQueue.Enqueue(new Card(5, seq[i] - 49, gt));
                else
                {
                    int color = seq[i] / 10;
                    int num = seq[i] % 10;
                    if (num <= 2)
                        num = 1;
                    else if (num == 9)
                        num = 5;
                    else
                        num = (num + 1) / 2;
                    CardQueue.Enqueue(new Card(color, num, gt));
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
