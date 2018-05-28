using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : PlayerCamera {

    [SerializeField] private Vector3 offset;
    private Vector3 defaultOffset;

    private void Start()
    {
        transform.parent = null;
    }

    private void OnEnable()
    {
        offset = transform.rotation * defaultOffset;
    }

    protected override void Initialize()
    {
        defaultOffset = offset;
        defaultCamDistance = offset.magnitude;
        camDistance = defaultCamDistance;
    }

    protected override void UpdateCamera(float mx, float my)
    {
        float h = 90f * mx * Time.deltaTime;
        float v = -90f * my * Time.deltaTime;

        float angleUp = Vector3.Angle(Vector3.up, transform.forward);
        float angleDown = Vector3.Angle(Vector3.down, transform.forward);
        v = (angleUp < 30f && v < 0f) || (angleDown < 15f && v > 0f) ? 0f : v;

        transform.position = player.transform.position + offset;
        transform.RotateAround(player.transform.position, transform.right, v);
        transform.RotateAround(player.transform.position, Vector3.up, h);
        offset = transform.position - player.transform.position;

        transform.position = player.transform.position + offset.normalized * camDistance;
    }

    
}
