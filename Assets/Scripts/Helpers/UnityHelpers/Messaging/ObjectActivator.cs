using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using Helpers.Timing;

public class ObjectActivator : MonoBehaviour, IObserver {

    public enum ActivationMethod
    {
        All,
        RoundRobin,
        Random
    }

    [SerializeField] private GameObject[] targets;
    [SerializeField] private GameObject sender;
    [SerializeField] private Message message;
    public float deactivationTime = 1f;

    private int index = 0;

    private void Start()
    {
        this.Observe(message);
        foreach (var t in targets)
            t.SetActive(false);
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        if ((sender as MonoBehaviour).gameObject != this.sender)
            return;

        if(msg == message)
        {
            if(targets.Length > 0)
            {
                targets[index].SetActive(true);
                index = (index + 1 >= 0 && index + 1 < targets.Length) ? index + 1 : 0;
                ActionHelper.DelayScaled(1f, () => { targets[index].SetActive(false); });
            }
        }
    }

}
