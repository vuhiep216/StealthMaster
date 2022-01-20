using System;
using System.Collections;
using System.Collections.Generic;
using Funzilla;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float spd;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    private Vector3 mousePos;
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var v = Input.mousePosition - mousePos;
            if(v.magnitude<=0) return;
            v = v.normalized;
            v *= spd * Time.smoothDeltaTime;
        transform.position += new Vector3(v.x, 0, v.y);
        }
    }
}
