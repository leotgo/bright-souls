using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls
{
    public class CameraAlignedParticle : MonoBehaviour
    {
        void Update ()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}