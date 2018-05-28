using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCamera : MonoBehaviour {

    public Player player = null;
    public LayerMask wallCollisionMask;

    protected float defaultCamDistance;
    protected float camDistance;

    private bool isInitialized = false;

    protected void Awake()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            if (!player)
                player = GetComponentInParent<Player>();
            Initialize();
        }
    }

    protected void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        UpdateCamera(mx, my);
        RaycastWalls();
    }

    protected abstract void Initialize();
    protected abstract void UpdateCamera(float mx, float my);

    private void RaycastWalls()
    {
        Vector3 dirVec = (transform.position - player.transform.position).normalized * defaultCamDistance * 1.1f;
        Ray ray = new Ray(player.transform.position, dirVec.normalized);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, dirVec.magnitude, wallCollisionMask.value))
        {
            float dist = ((hitInfo.point) - player.transform.position).magnitude - 0.4f;
            camDistance = Mathf.Lerp(camDistance, Mathf.Clamp(dist, 0.6f, defaultCamDistance), 20f * Time.deltaTime);
        }
        else
            camDistance = Mathf.Lerp(camDistance, defaultCamDistance, 15f * Time.deltaTime);
    }
}
