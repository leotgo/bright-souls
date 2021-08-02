using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BrightSouls
{
    public abstract class PlayerCameraBase : MonoBehaviour
    {
        // This is a test comment
        public bool IsLockOnCamera { get => this is LockOnCamera; }
        public bool IsThirdPersonCamera { get => this is ThirdPersonCamera; }

        public abstract CinemachineVirtualCameraBase CinemachineCamera { get; }
        public abstract void SetPriority(int value);
    }
}