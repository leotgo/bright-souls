using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patterns.Observer;
using Helpers.Timing;

public class MessageEvent : MonoBehaviour, IObserver {

    public Component sender;
    public Message message;
    public UnityEvent evt;
    public float delay = 0f;

	void Start () {
        this.Observe(message);
	}

	public void OnNotification(object sender, Message msg, params object[] args)
    {
        if((Object)sender == this.sender)
        {
            if(msg == message)
            {
                if (delay > 0f)
                {
                    ActionHelper.DelayScaled(delay, () => evt.Invoke());
                }
                else
                    evt.Invoke();
            }
        }
    }
}
