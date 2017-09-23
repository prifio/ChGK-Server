using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanabi
{
    public class GameStory
    {
        private List<Event> EventsList = new List<Event>();
        public void HintColor(int from, int to, int numColor)
        {
            var ans = new Event();
            ans.Type = EventType.HintColor;
            ans.Color = numColor;
            ans.Number = -1;
            ans.PlayerFrom = from;
            ans.PlayerTo = to;
            EventsList.Add(ans);
        }
        public void Hintnumber(int from, int to, int Number)
        {
            var ans = new Event();
            ans.Type = EventType.HintNumber;
            ans.Number  = Number;
            ans.Color = -1;
            ans.PlayerFrom = from;
            ans.PlayerTo = to;
            EventsList.Add(ans);
        }
        public void PlaceCard(int from, int numColor, int Number)
        {
            var ans = new Event();
            ans.Type = EventType.LayOK;
            ans.Color = numColor;
            ans.Number = Number;
            ans.PlayerFrom = from;
            ans.PlayerTo = -1;
            EventsList.Add(ans);
        }
        public void MakeFall(int from, int numColor, int Number)
        {
            var ans = new Event();
            ans.Type = EventType.LayFall;
            ans.Color = numColor;
            ans.Number = Number;
            ans.PlayerFrom = from;
            ans.PlayerTo = -1;
            EventsList.Add(ans);
        }
        public void DropCard(int from, int numColor, int Number)
        {
            var ans = new Event();
            ans.Type = EventType.Drop;
            ans.Color = numColor;
            ans.Number = Number;
            ans.PlayerFrom = from;
            ans.PlayerTo = -1;
            EventsList.Add(ans);
        }
        public Event[] GetInfo()
        {
            return EventsList.ToArray();
        }
    }

    public class Event
    {
        public EventType Type { get; set; }
        public int Color { get; set; }
        public int Number { get; set; }
        public int PlayerFrom { get; set; }
        public int PlayerTo { get; set; }
    }

    public enum EventType
    {
        HintColor,
        HintNumber,
        LayOK,
        LayFall,
        Drop
    }
}