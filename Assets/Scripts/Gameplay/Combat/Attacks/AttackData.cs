using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BrightSouls
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "BrightSouls/Data/Combat/AttackData", order = 0)]
    public class AttackData : ScriptableObject
    {
        public List<CombatEffect> SourceEffects { get => sourceEffects; }
        public List<CombatEffect> TargetEffects { get => targetEffects; }

        [SerializeField] public List<CombatEffect> sourceEffects;
        [SerializeField] public List<CombatEffect> targetEffects;
    }
}