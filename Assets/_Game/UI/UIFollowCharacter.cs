using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCharacter : MonoBehaviour {

    private float yOffset = 1.25f;
    private Character owner;
    private RectTransform rt;

    private void Start()
    {
        owner = GetComponentInParent<Character>();
        rt = GetComponent<RectTransform>();
    }

    private void Update () {
        var worldPoint = Camera.main.WorldToScreenPoint(owner.transform.position + Vector3.up * yOffset);
        rt.position = worldPoint;
	}
}
