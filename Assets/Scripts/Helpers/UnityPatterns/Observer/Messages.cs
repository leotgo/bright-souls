using System;
using System.Linq;
using System.Collections;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;

namespace Patterns.Observer
{
    public enum Message
    {
        // COMBAT ============================ //

        Combat_Index = 1,
        Combat_Hit,
        Combat_GotHit,
        Combat_AttackEnd,
        Combat_Punch,
        Combat_Struck,
        Combat_Death,
        Combat_Dodge,
        Combat_WeaponActivateHitbox,
        Combat_WeaponDeactivateHitbox,
        Combat_Stagger,
        Combat_StaggerEnd,
        Combat_SwingSword,
        Combat_HealthChange,
        Combat_StaminaChange,
        Combat_AttackStart,
        Combat_AttackActivateHitbox,
        Combat_AttackDeactivateHitbox,
        Combat_DetectHit,
        Combat_LockOnTarget,
        Combat_BlockedHit,
        Combat_BlockBreak,
        Combat_HitEnemy,
        Combat_LockOnTransformChange,

        // MOVEMENT ========================== //

        Movement_Index = 300,
        Movement_Step,

        // AI ================================ //

        AI_ReachedWaypoint = 500,
        AI_StateSignalEnd,
        AI_StartAttack,

        // SYSTEM ============================ //

        System_Index = 700,
        System_TogglePause,
        System_Pause,
        System_Unpause,
        System_SaveData,
        System_LoadData,
        System_ReloadScene,
        System_Exit,
        System_NewGame,

        // PLAYER ====================== //

        Player_InteractionTargetChanged = 900
    }
}