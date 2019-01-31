using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Patterns.Observer;

public class UIControllerEnemy : MonoBehaviour, IObserver
{

    [SerializeField] private Canvas UIElementCanvas;

    private Character owner;
    private TimerAction disableUITimer;

    private IEnumerator Start()
    {
        yield return null;
        owner = GetComponentInParent<Character>();
        UIElementCanvas.enabled = false;
        disableUITimer = new TimerAction(this, 5f, () =>
        {
            UIElementCanvas.enabled = false;
        });

        this.Observe(Message.Combat_LockOnTarget);
        this.Observe(Message.Combat_HealthChange);
    }

    private void Update()
    {
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        if (owner.IsInAnyState(States.Dead))
        {
            UIElementCanvas.enabled = false;
            return;
        }
        if (msg == Message.Combat_LockOnTarget)
        {
            var target = args[0] as Character;
            if (target == owner)
            {
                UIElementCanvas.enabled = true;
                disableUITimer.Stop();
            }
            else
            {
                if (!disableUITimer.IsCounting())
                    UIElementCanvas.enabled = false;
            }
        }
        else if (msg == Message.Combat_HealthChange && (Object)sender == owner)
        {
            if (!UIElementCanvas.enabled)
            {
                UIElementCanvas.enabled = true;
                disableUITimer.Start();
            }
        }
        else if (msg == Message.Combat_Death && (Object)sender == owner)
        {
            UIElementCanvas.enabled = false;
        }
    }
}
