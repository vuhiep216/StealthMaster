using System;
using System.Collections;
using System.Collections.Generic;
using Funzilla;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private NavMeshAgent agent;
    private void Start()
    {

    }

    private void Update()
    {
        var move = dynamicJoystick.Direction * speed;
        agent.velocity = new Vector3(move.x, 0, move.y);
    }
}
