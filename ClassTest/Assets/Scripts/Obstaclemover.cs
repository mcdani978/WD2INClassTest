using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyRotation : MonoBehaviour
{
    //This makes the obstacles rotate in a circle 

    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float circleRadius = 5f;  
    [SerializeField] private float circleSpeed = 2f;   
    [SerializeField] private float initialAngleOffset = 0f; 

    private float angle;
    private Vector3 initialPosition;  

    void Start()
    {

        initialPosition = transform.position;


        angle = initialAngleOffset;
    }

    void Update()
    {

        angle += circleSpeed * Time.deltaTime;


        float x = Mathf.Cos(angle) * circleRadius;
        float z = Mathf.Sin(angle) * circleRadius;


        transform.position = new Vector3(x + initialPosition.x, initialPosition.y, z + initialPosition.z);

    
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}