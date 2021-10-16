using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BrightSouls.Gameplay
{
    public sealed class PlayerCameraDirector : MonoBehaviour
    {
        public PlayerCameraBase CurrentCamera { get => currentCamera; }

        private PlayerCameraBase currentCamera;
        private Dictionary<System.Type, PlayerCameraBase> cameraMap;

        [SerializeField] private List<PlayerCameraBase> playerCameras;

        private void Awake()
        {
            cameraMap = new Dictionary<System.Type, PlayerCameraBase>();
            DeactivateCameras();
        }

        private void Start()
        {
            // * Unparent so the player movement/rotation doesn't incorrectly affect cameras
            UnparentCameras();
            ActivateCameras();
        }

        public void RegisterCamera<T>(T camera) where T : PlayerCameraBase
        {
            cameraMap.Add(typeof(T), camera);
        }

        public T GetCamera<T>() where T : PlayerCameraBase
        {
            cameraMap.TryGetValue(typeof(T), out var cam);
            return cam.GetComponent<T>();
        }

        private void UnparentCameras()
        {
            foreach (var cam in cameraMap.Values)
            {
                cam.transform.parent = transform.root;
            }
        }

        private void ActivateCameras()
        {
            foreach (var cam in cameraMap.Values)
            {
                cam.gameObject.SetActive(true);
            }
        }

        private void DeactivateCameras()
        {
            foreach (var cam in cameraMap.Values)
            {
                cam.gameObject.SetActive(false);
            }
        }

        private void BringCameraToFront<T>() where T : PlayerCameraBase
        {
            foreach (var cam in cameraMap.Values)
            {
                cam.SetPriority(0);
            }
            this.GetCamera<T>().SetPriority(1);
        }

        private void DoForAllCameras()
        {

        }
    }
}