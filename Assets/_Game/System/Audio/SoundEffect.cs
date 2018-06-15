using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXName
{
    SFX_None = 0,
    SFX_Player_StepBasic1 = 100,
    SFX_Player_StepBasic2,
    SFX_Player_Hurt1,
    SFX_Player_Hurt2,
    SFX_Player_DodgeWhoosh,
    SFX_Player_DodgeStep,
    SFX_Player_Death,
    SFX_Player_SwingSword = 150,
    SFX_Player_SwingSword2,
    SFX_Player_SwingSword3,
    SFX_Player_GotCut1,
    SFX_Player_GotCut2,
    SFX_Player_GotCutImpact1,
    SFX_Player_Bleed1,
    SFX_Player_CutEnemy1 = 175,
    SFX_Player_CutEnemy2,
    SFX_Player_BlockedHit,
    SFX_Player_BlockBreak,
    SFX_Enemy_StepBase = 200,
    SFX_Enemy_Hurt1,
    SFX_Enemy_Hurt2,
    SFX_Enemy_Cut1,
    SFX_Enemy_Cut2,
    SFX_Enemy_HitShield,
}

[System.Serializable]
public class SoundEffect
{
    public SFXName effect = SFXName.SFX_None;
    public AudioClip clip = null;
    public ClipPlayOptions options;

    [System.Serializable]
    public class ClipPlayOptions
    {
        [Range(0, 256)]
        public int priority = 128;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(-3f, 3f)]
        public float pitch = 1f;

        [Range(0f, 1f)]
        public float pitchVariance = 0f;

        [Range(-1f, 1f)]
        public float stereoPan = 0f;

        public float delay = 0f;
    }
}

