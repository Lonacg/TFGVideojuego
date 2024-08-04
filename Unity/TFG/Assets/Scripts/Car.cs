using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{

    [Header("Car model:")]
    public Transform carModel;
    public Vector3 modelOffset;


    public Transform[] frontWheels;


    [Header("Car components:")]
    public CarEngine carEngine;

    public CarDriver carDriver;





    void Start()
    {
        
    }

    void Update()
    {
        carEngine.SetDesiredVelocity(carDriver.desiredDirection.y * carModel.forward);
    }

    void FixedUpdate()
    {

        carModel.position = carEngine.rigidbody.position + modelOffset;
        
        // Giro de las ruedas del modelo
        Quaternion wheelTargetRotation = Quaternion.identity;
        if (carDriver.desiredDirection.x < 0)
        {
            carModel.Rotate(carModel.up, -carEngine.engineData.turnSpeed * Time.deltaTime);
            wheelTargetRotation = Quaternion.Euler(0, -20, 0);
        }
        if (carDriver.desiredDirection.x > 0)
        {
            carModel.Rotate(carModel.up, carEngine.engineData.turnSpeed * Time.deltaTime);
            wheelTargetRotation = Quaternion.Euler(0, 20, 0);
        }

        foreach (var wheel in frontWheels)
        {
            wheel.localRotation = Quaternion.Lerp(wheel.localRotation, wheelTargetRotation, Time.deltaTime * 10);
        }

        

    }

}
