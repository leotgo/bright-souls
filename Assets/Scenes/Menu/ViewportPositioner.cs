using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportPositioner : MonoBehaviour {

    public Camera referenceCamera;

    public float x = 0f;
    public float y = 0f;

	void Start ()
    {
        float z = transform.position.z - referenceCamera.transform.position.z;
        transform.position = referenceCamera.ViewportToWorldPoint(new Vector3(x, y, z));
	}
}
