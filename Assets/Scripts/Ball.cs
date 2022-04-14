using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    private Vector3 _vector;
    
    private float _angle;

    private float _speed = 6;

    private void Awake()
    {
        _angle = UnityEngine.Random.Range(45, 135);
        _vector = new Vector3(Mathf.Cos(_angle), 0, 0);
    }

    private void Update()
    {
        if (BallsMoveController.Instance.currentStage == Stage.EndGame && GetComponent<Rigidbody>() == null)
        {
            var body=gameObject.AddComponent<Rigidbody>();

            body.useGravity = false;
            body.constraints = RigidbodyConstraints.FreezeAll;

            //transform.parent = null;
        }
        else if (BallsMoveController.Instance.currentStage == Stage.EndGame)
        {
            transform.localPosition += _vector * Time.deltaTime * _speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EndEdge"))
        {
            
            
            SetEndVector(collision.contacts[0].normal.normalized);
        }
        /*else if (collision.gameObject.CompareTag("End"))
        {
            var pos = transform.position;

            pos.y = collision.contacts[0].point.y + 0.5f;

            transform.position = pos;
            transform.eulerAngles=Vector3.zero;
        }*/
    }

    private void SetEndVector(Vector3 vectorNormal)
    {
        _vector = ReflectProjectile(vectorNormal);
    }
    
    private Vector3 ReflectProjectile(Vector3 reflectVector)
    {    
        var velocity = Vector3.Reflect(_vector, reflectVector);

        return velocity;
    }
}
