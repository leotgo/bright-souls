using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;
using DuloGames.UI;

public class UIHealthBarDelayed : MonoBehaviour, IObserver {

    [SerializeField] private Character owner;

    public float updateDelay = 1f;
    private float updateDelayTime = 0f;
    private bool isWaitingUpdate = false;

    private float updateTime = 0f;
    private float updateDuration = 2f;
    private bool isUpdating;

    private Image bar;
    

	private void Start () {
        if(!owner)
            owner = GetComponentInParent<Character>();
        bar = GetComponent<Image>();
        this.Observe(Message.Combat_HealthChange);
        isUpdating = false;
	}

    private void Update()
    {
        if (isUpdating)
        {
            updateTime += Time.deltaTime;
            if (updateTime > updateDuration)
            {
                updateTime = updateDuration;
                isUpdating = false;
            }
            bar.fillAmount = Mathf.SmoothStep(bar.fillAmount, owner.Health / owner.maxHealth, updateTime / updateDuration);
        }
        else
        {
            if (isWaitingUpdate)
            {
                updateDelayTime += Time.deltaTime;
                if (updateDelayTime > updateDelay)
                {
                    isUpdating = true;
                    isWaitingUpdate = false;
                }
            }
        }
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        bool senderIsPlayer = (Object)sender == owner;
        if (!senderIsPlayer)
            return;

        switch (msg)
        {
            case Message.Combat_HealthChange:
                float diff = (float)args[0];
                if (diff < 0)
                {
                    updateTime = 0f;
                    updateDelayTime = 0f;
                    isWaitingUpdate = true;
                    isUpdating = false;
                }
                else
                {
                    bar.fillAmount = owner.Health / owner.maxHealth;
                }
                
                break;
        }
    }
}