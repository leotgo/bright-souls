using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;
using Sirenix.OdinInspector;

public class AnimStateMessage : StateMachineBehaviour {

    public List<AnimMessageEvent> events = new List<AnimMessageEvent>();
    private int currentEventId = 0;

	 //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (events.Count == 0)
            return;

        currentEventId = 0;

        foreach (var e in events)
            if (e.trigger == AnimMessageEvent.EventTrigger.StateEnter)
                animator.Notify(e.message, e.args);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (events.Count == 0)
            return;

        float time = stateInfo.normalizedTime % 1f;

        if (currentEventId >= events.Count)
        {
            if (time < events[0].time)
            {
                currentEventId = 0;
            }
        }
        else if(time > events[currentEventId].time)
        {
            if (events[currentEventId].trigger == AnimMessageEvent.EventTrigger.StateTime)
            {
                var msg = events[currentEventId].message;
                animator.Notify(events[currentEventId].message, events[currentEventId].args);
            }
            currentEventId++;
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (events.Count == 0)
            return;
        currentEventId = 0;
        foreach (var e in events)
            if (e.trigger == AnimMessageEvent.EventTrigger.StateExit)
                animator.Notify(e.message, e.args);
    }
}

[System.Serializable]
public class AnimMessageEvent
{
    public enum EventTrigger
    {
        StateEnter,
        StateTime,
        StateExit
    }
    public bool IsStateTimeTrigger {
        get {
            return trigger == EventTrigger.StateTime;
        }
    }

    public EventTrigger trigger = EventTrigger.StateTime;
    [ShowIf("IsStateTimeTrigger")]
    public float time = 0f;
    public Message message;
    public string args;
}