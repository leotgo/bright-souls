using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BrightSouls
{
    public sealed class ThirdPersonCamera : PlayerCameraBase
    {
        public override CinemachineVirtualCameraBase CinemachineCamera { get => freeLookCamera; }
        public RotateCameraCommand Look { get; private set; }

        [SerializeField] private Player player;
        [SerializeField] private CinemachineFreeLook freeLookCamera;

        private void Start ()
        {
            InitializeCommands ();
            InitializeInput ();
        }

        private void InitializeCommands ()
        {
            Look = new RotateCameraCommand (player);
        }

        private void InitializeInput ()
        {
            var look = player.Input.currentActionMap.FindAction ("Look");
            look.performed += ctx => Look.Execute (look.ReadValue<Vector2> ());
        }

        public override void SetPriority (int value)
        {
            freeLookCamera.Priority = value;
        }

        public void SetInputAxisValue (Vector2 input)
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = input.x;
            freeLookCamera.m_YAxis.m_InputAxisValue = input.y;
        }

        public void SetMaxSpeed (float x, float y)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = x;
            freeLookCamera.m_YAxis.m_MaxSpeed = y;
        }

        public sealed class RotateCameraCommand : PlayerCommand<Vector2>
        {
            private ThirdPersonCamera m_thirdPersonCamera = null;

            public RotateCameraCommand (Player player) : base (player)
            {
                m_thirdPersonCamera = player.CameraDirector.GetCamera<ThirdPersonCamera> ();
            }

            public override bool IsValid ()
            {
                return true;
            }

            public override void Execute (Vector2 input)
            {
                m_thirdPersonCamera.SetInputAxisValue (input);
            }
        }
    }
}