using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class MessageNotifier : MonoBehaviour {

    public Message message;

	public void Notify()
    {
        this.Notify(message);
    }
}
