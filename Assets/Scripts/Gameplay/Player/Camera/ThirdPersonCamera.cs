using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using BrightSouls.Player;

namespace BrightSouls
{
    public sealed class ThirdPersonCamera : PlayerCameraBase
    {
        /* ------------------------------- Definitions ------------------------------ */

        public sealed class RotateCameraCommand : PlayerCommand<Vector2>
        {
            private ThirdPersonCamera thirdPersonCamera = null;

            public RotateCameraCommand(PlayerComponentIndex player) : base(player)
            {
                thirdPersonCamera = player.CameraDirector.GetCamera<ThirdPersonCamera>();
            }

            public override bool CanExecute()
            {
                return true;
            }

            public override void Execute(Vector2 input)
            {
                thirdPersonCamera.SetInputAxisValue(input);
            }
        }

        /* ------------------------------- Properties ------------------------------- */

        public override CinemachineVirtualCameraBase CinemachineCamera
        {
            get => freeLookCamera;
        }

        public RotateCameraCommand Look
        {
            get;
            private set;
        }

        /* ------------------------ Inspector-assigned Fields ----------------------- */

        [SerializeField] private PlayerComponentIndex player;
        [SerializeField] private CinemachineFreeLook freeLookCamera;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Start ()
        {
            InitializeCommands ();
            InitializeInput ();
        }

        /* ----------------------------- Initialization ----------------------------- */

        private void InitializeCommands ()
        {
            Look = new RotateCameraCommand (player);
        }

        private void InitializeInput ()
        {
            var look = player.Input.currentActionMap.FindAction ("Look");
            look.performed += ctx => Look.Execute (look.ReadValue<Vector2> ());
        }

        /* --------------------------- Core Functionality --------------------------- */

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

        /* -------------------------------------------------------------------------- */
    }
}