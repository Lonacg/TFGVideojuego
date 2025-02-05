using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarTest : MonoBehaviour
{

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque;     // maximum torque the motor can apply to wheel
    public float maxSteeringAngle;   // maximum steer angle the wheel can have

    
    public float motor;
    public float steering;
    public Rigidbody rb;

    void FixedUpdate()
    {

        if(Input.GetAxis("Vertical") != 0){
            motor = maxMotorTorque * Input.GetAxis("Vertical");
            steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        }
        else{
            motor = 0;
            steering = 0;       
        }

            
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
        Debug.Log(rb.linearVelocity);

    }
}



[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;      // is this wheel attached to motor?
    public bool steering;   // does this wheel apply steer angle?
}