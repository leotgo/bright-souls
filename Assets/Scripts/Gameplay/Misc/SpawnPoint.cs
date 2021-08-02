using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject prefab;

    private void Start()
    {
        var go = Instantiate(prefab);
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
    }
}
