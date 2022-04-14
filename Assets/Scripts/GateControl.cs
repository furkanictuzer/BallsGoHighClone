using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MathType
{
    Multiply,
    Divide,
    Sum,
    Subtraction
}
public class GateControl : MonoBehaviour
{
    
    [HideInInspector] public Color color;
    
    public MathType mathType;

    public float operationNumber;

    private void Awake()
    {
        color = GetComponent<Renderer>().material.color;
    }

    
}
