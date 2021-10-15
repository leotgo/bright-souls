using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BrightSouls
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "BrightSouls/Data/Combat/AttackData", order = 0)]
    public class AttackData : ScriptableObject
    {
        public List<ICombatEffect> SourceEffects { get => sourceEffects; }
        public List<ICombatEffect> TargetEffects { get => targetEffects; }

        [SerializeReference] public List<ICombatEffect> sourceEffects;
        [SerializeReference] public List<ICombatEffect> targetEffects;
    }
}