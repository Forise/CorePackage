//Developed by Pavel Kravtsov.
using System;
using System.Collections.Generic;

namespace Core.EventSystem
{
    public static class EventManager
    {
        static EventManager() { }

        static Dictionary<string, HashSet<EventHandler<GameEventArgs>>> subscribes = new Dictionary<string, HashSet<EventHandler<GameEventArgs>>>();

        public static void Subscribe(string eventType, EventHandler<GameEventArgs> callback)
        {
            if (!subscribes.ContainsKey(eventType))
                subscribes.Add(eventType, new HashSet<EventHandler<GameEventArgs>>());
            if (!subscribes[eventType].Contains(callback))
                subscribes[eventType].Add(callback);
        }

        public static void Unsubscribe(string eventType, EventHandler<GameEventArgs> callback)
        {
            if (eventType != null && subscribes.ContainsKey(eventType) && subscribes[eventType].Contains(callback))
            {
                subscribes[eventType].Remove(callback);
                if (subscribes[eventType].Count == 0)
                    subscribes.Remove(eventType);
            }
        }

        public static void UnsubscribeAll()
        {
#if DEBUG
            //Debug.Log(("[EventManager] " +  System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif
            subscribes.Clear();
        }

        public static void Notify(object sender, GameEventArgs args)
        {
            if (subscribes.ContainsKey(args.type))
            {
                HashSet<EventHandler<GameEventArgs>> handlers = new HashSet<EventHandler<GameEventArgs>>(subscribes[args.type]);
                foreach (var handler in handlers)
                    handler(sender, args);
            }
        }
    }

}
