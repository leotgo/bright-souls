using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BrightSouls.Gameplay
{
    public sealed class LockOnCamera : PlayerCameraBase
    {
        public override CinemachineVirtualCameraBase CinemachineCamera { get => virtualCamera; }
        public ChangeTargetCommand ChangeTarget { get; private set; }
        public ICharacter Target { get => lockOnTarget; }

        public LockOnCommand lockOn;

        private CinemachineVirtualCamera virtualCamera;
        private ICharacter lockOnTarget;
        private LockOnDetector lockOnDetector;
        //private bool lockOnTargetChanged = false;
        //private float lockOnDelay = 0f;

        [SerializeField] private Player player;

        public override void SetPriority (int value)
        {
            virtualCamera.Priority = value;
        }

        private void Start ()
        {
            // Initialize commands
            ChangeTarget = new ChangeTargetCommand (player);

            // Initialize camera component refs
            lockOnDetector = player.GetComponentInChildren<LockOnDetector> ();
        }

        /// <summary>
        /// Updates the Cinemachine LookAt target for the lock on cameras to reflect
        /// the current player target.
        /// </summary>
        private void RefreshLockOnCameraTarget ()
        {
            //m_lockOnCamera.LookAt = lockOnTarget.transform;
        }

        public class LockOnCommand : PlayerCommand
        {
            public LockOnCommand (Gameplay.Player player) : base (player) { }

            public override bool CanExecute ()
            {
                return true; //player.camera.m_lockOnDetector.PossibleTargets.Count > 0 || player.camera.LockOnTarget != null;
            }

            public override void Execute ()
            {
                /*if (!player.camera.LockOnTarget)
                    player.camera.LockOnTarget = player.camera.FindLockOnTarget(0f);
                else
                    player.camera.LockOnTarget = null;*/
            }
        }

        public class ChangeTargetCommand : PlayerCommand<Vector2>
        {
            public ChangeTargetCommand (Gameplay.Player player) : base (player) { }

            public override bool CanExecute ()
            {
                return true; //player.camera.lockon.PossibleTargets.Count > 1;
            }

            public override void Execute (Vector2 input)
            {
                //player.camera.LockOnTarget = player.camera.FindLockOnTarget(dir);
            }
        }
    }
}

/*public Character LockOnTarget
{
    get => lockOnTarget;
    set
    {
        bool targetHasChanged = value != lockOnTarget;
        if(targetHasChanged)
        {
            lockOnTarget = value;
            bool hasTarget = lockOnTarget != null;
            // Change camera mode based on existence of target
            if(hasTarget)
            {
                Mode = CameraMode.LockOn;
            }
            else
            {
                Mode = CameraMode.ThirdPerson;
            }
            // Notify any observer that lock on target has changed
            this.Notify(Message.Combat_LockOnTransformChange, value != null ? value.transform : null);
        }
    }
}*/

/*              if(m_lockOnTargetChanged)
                {
                    m_lockOnDelay += Time.deltaTime;
                    if(m_lockOnDelay > 0.5f)
                    {
                        m_lockOnDelay = 0f;
                        m_lockOnTargetChanged = false;
                    }
                }
                if(Mathf.Abs(lookInput.x) > 0.5)
                {
                    if(changeLockOnTarget.IsValid() && !m_lockOnTargetChanged)
                    {
                        changeLockOnTarget.Execute(Mathf.Sign(lookInput.x));
                        m_lockOnTargetChanged = true;
                    }
                }
            }*/

/*public Character FindLockOnTarget(float inputX)
        {
            Character closestInDir = LockOnTarget;
            float diffToCenter = 1.0f;
            lockOnDetector.RefreshTargets();
            // Iterate through all the targets in lock-on range
            foreach (var target in lockOnDetector.PossibleTargets)
            {
                // Ignore current target
                if (target == LockOnTarget)
                    continue;
                // Retrieve position of the target in view space
                var viewPos = Camera.main.WorldToViewportPoint(target.transform.position);
                float diff = Mathf.Abs(viewPos.x - 0.5f);
                if (diff < diffToCenter)
                {
                    bool targetIsInInputDirection = false;
                    if ((inputX == 0f) ||
                        (inputX > 0f && viewPos.x > 0.5f) ||
                        (inputX < 0f && viewPos.x < 0.5f))
                        targetIsInInputDirection = true;
                    closestInDir = targetIsInInputDirection ? target : closestInDir;
                    diffToCenter = targetIsInInputDirection ? diff : diffToCenter;
                }

            }
            return closestInDir;
        }*/