using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.AI
{

    public abstract class AIBehaviour
    {
        public abstract void OnBehaviourStart(AICharacter agent);
        public abstract void OnBehaviourUpdate(AICharacter agent);
        public abstract void OnBehaviourEnd(AICharacter agent);
    }
}
