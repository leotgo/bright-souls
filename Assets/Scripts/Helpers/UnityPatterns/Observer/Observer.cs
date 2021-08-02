using System.Collections.Generic;
using UnityEngine;

namespace Patterns.Observer
{
    public interface IObserver {
        void OnNotification(object sender, Message subject, params object[] args);
    }

    public static class MessageSystem
    {
        private static Dictionary<Message, List<IObserver>> listeners;

        public static bool initialized = false;

        public static void Init()
        {
            initialized = true;
            listeners = new Dictionary<Message, List<IObserver>>();
        }

        public static void Notify(this object sender, Message subject, params object[] args)
        {
            if (!MessageSystem.initialized)
                Init();

            if (sender == null)
                Debug.LogError("Sender is null!");

            if (listeners.ContainsKey(subject))
                foreach (var observer in listeners[subject])
                    observer.OnNotification(sender, subject, args);
        }

        public static void Observe(this IObserver observer, Message msgType)
        {
            if (!MessageSystem.initialized)
                Init();

            if (!listeners.ContainsKey(msgType))
                listeners.Add(msgType, new List<IObserver>() { observer });
            else
            {
                if (!listeners[msgType].Contains(observer))
                    listeners[msgType].Add(observer);
            }
        }
    }
}