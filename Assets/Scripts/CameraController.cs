using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.transform.position;
    }

    private void Update()
    {
        var position = target.transform.position;
        transform.position = new Vector3(
            Mathf.Clamp(position.x,-5f,5f),
            position.y,
            position.z) + offset;
    }
}
