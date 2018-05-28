using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class LockOnDetector : MonoBehaviour, IObserver {

    private Player owner;
    public List<Character> possibleTargets;

    private void Start()
    {
        owner = GetComponentInParent<Player>();
        possibleTargets = new List<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (!character)
            return;
        else if (character == owner)
            return;

        if (!possibleTargets.Contains(character) && !character.IsInAnyState(States.Dead))
            possibleTargets.Add(character);
    }

    private void OnTriggerExit(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (!character)
            return;
        else if (character == owner)
            return;

        if (possibleTargets.Contains(character))
            possibleTargets.Remove(character);
    }

    public void OnNotification(object sender, Message msg, params object[] args)
    {
        if(msg == Message.Combat_Death)
        {
            Character c = (Character)sender;
            if (possibleTargets.Contains(c))
                possibleTargets.Remove(c);
        }
    }

}
