using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCamera : PlayerCamera
{


    public Vector3 lockOnOffset;

    private float accumX = 0f;
    private float maxSideLookDist = 3f;
    
    
    public Transform LockOnTarget {
        get {
            return player.Combat.LockOnTarget.transform;
        }
        set {
            player.Combat.LockOnTarget = value.GetComponent<Character>();
        }
    }

    private void Start()
    {
        transform.parent = null;
    }

    protected override void Initialize()
    {
        defaultCamDistance = lockOnOffset.magnitude;
        camDistance = defaultCamDistance;
    }

    protected override void UpdateCamera(float mx, float my)
    {
        if (LockOnTarget == null)
            return;

        accumX += mx * Time.deltaTime;
        accumX = Mathf.Clamp(accumX, -1f, 1f);
        Quaternion modifRot = Quaternion.Euler(0f, accumX * 30f, 0f);
        if(Mathf.Abs(mx) < 0.1f)
            accumX = Mathf.Lerp(accumX, 0f, 5f * Time.deltaTime);

        var lookRot = modifRot * Quaternion.LookRotation((LockOnTarget.position - transform.position).normalized, Vector3.up);
        var targetPos = player.transform.position 
            + (Quaternion.LookRotation((LockOnTarget.position - player.transform.position).normalized, Vector3.up) * lockOnOffset).normalized 
            * camDistance;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, 50f * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, 15f * Time.deltaTime);
    }
}
