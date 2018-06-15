using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrail : MonoBehaviour {

    public Transform refTransform;
    public Vector3 refOffset;
    public Quaternion refRotation;
    public float lerpSpeed = 10f;

	// Use this for initialization
	void Start () {
        refTransform = transform.parent;
        refOffset = Quaternion.Inverse(refTransform.rotation) * (transform.position - refTransform.position);
        refRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.parent = null;
        transform.position = Vector3.Slerp(transform.position, refTransform.position + refTransform.rotation * refOffset, lerpSpeed * Time.deltaTime);
	}
}
