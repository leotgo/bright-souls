using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

[RequireComponent(typeof(Text))]
public class UIHealthText : MonoBehaviour, IObserver {

    public Character owner;
    private Text uiText;

	void Start () {
        uiText = GetComponent<Text>();
        this.Observe(Message.Combat_HealthChange);
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        bool senderIsOwner = (Object)sender == owner;
        if (!senderIsOwner)
            return;

        switch (msg)
        {
            case Message.Combat_HealthChange:
                float diff = (float)args[0];
                uiText.text = Mathf.CeilToInt(owner.Health / owner.maxHealth * 100f).ToString() + "%";
                break;
        }
    }
}
