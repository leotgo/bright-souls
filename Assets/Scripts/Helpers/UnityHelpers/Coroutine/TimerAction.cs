using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TimerAction
{
    private float delay;
    private float currentTime;
    private bool isStopped;
    private bool kill;
    private System.Action action;

    public TimerAction(MonoBehaviour source, float delay, System.Action action)
    {
        this.delay = delay;
        this.isStopped = true;
        this.action = action;
        this.kill = false;
        source.StartCoroutine(this.Update());
    }

    public void Start()
    {
        currentTime = 0f;
        isStopped = false;
    }

    public void Pause()
    {
        isStopped = true;
    }

    public void Stop()
    {
        currentTime = 0f;
        isStopped = true;
    }

    public void Kill()
    {
        kill = true;
    }

    public bool IsCounting()
    {
        return !isStopped;
    }

    private IEnumerator Update()
    {
        while (!kill)
        {
            if (isStopped)
                yield return null;
            else
            {
                currentTime += Time.deltaTime;
                if (currentTime > delay)
                {
                    action.Invoke();
                    this.Stop();
                }
                yield return null;
            }
        }
    }
}
