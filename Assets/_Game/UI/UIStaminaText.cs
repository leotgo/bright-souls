using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

[RequireComponent(typeof(Text))]
public class UIStaminaText : MonoBehaviour, IObserver {

    public Character owner;
    private Text uiText;

    void Start()
    {
        uiText = GetComponent<Text>();
        this.Observe(Message.Combat_StaminaChange);
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        bool senderIsOwner = (Object)sender == owner;
        if (!senderIsOwner)
            return;

        switch (msg)
        {
            case Message.Combat_StaminaChange:
                float diff = (float)args[0];
                var stamina = owner.GetComponent<StaminaBehaviour>();
                uiText.text = Mathf.CeilToInt(stamina.Value / stamina.maxValue * 100f).ToString() + "%";
                break;
        }
    }
}
