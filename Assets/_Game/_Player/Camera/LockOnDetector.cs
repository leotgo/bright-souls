using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Observer;

public class LockOnDetector : MonoBehaviour {

    private Player owner;
    private List<Character> possibleTargets;
    public List<Character> PossibleTargets {
        get {
            List<Character> targets = new List<Character>(possibleTargets);
            targets.RemoveAll((Character c) => { return c.IsInAnyState(States.Dead) || !c.GetComponentInChildren<Renderer>().isVisible; });
            return targets;
        }
    }

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

    public void RefreshTargets()
    {
        possibleTargets.RemoveAll((Character c) => { return c.IsInAnyState(States.Dead); });
    }

}
