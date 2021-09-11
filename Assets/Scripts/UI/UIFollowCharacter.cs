using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightSouls.UI
{
    public class UIFollowCharacter : MonoBehaviour
    {
        /* ------------------------ Inspector-Assigned Fields ----------------------- */

        [SerializeField] private ICharacter owner;
        [SerializeField] private float yOffset = 1.25f;
        [SerializeField] private RectTransform rt;

        /* ------------------------------ Unity Events ------------------------------ */

        private void Update ()
        {
            var worldPoint = Camera.main.WorldToScreenPoint(owner.transform.position + Vector3.up * yOffset);
            rt.position = worldPoint;
        }

        /* -------------------------------------------------------------------------- */
    }
}