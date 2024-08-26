using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Movement:")]
    public float runSpeed = 10;
    public float rotationSpeed = 250;

    public float x, y;


    [Header("Accessories")]
    public Transform[] frontWheels;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Entrada de movimiento
        x= Input.GetAxis("Horizontal");
        y= Input.GetAxis("Vertical");

        // Actualizacion del movimiento
        transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
        transform.Translate(0,0, y * Time.deltaTime * runSpeed); 


        // Giro de las ruedas delanteras
        Quaternion wheelTargetRotation = Quaternion.identity;
        if (x < 0) // Giro a la izquierda
            wheelTargetRotation = Quaternion.Euler(0, -30, 0);
        
        if (x > 0) // Giro a la derecha
            wheelTargetRotation = Quaternion.Euler(0, 30, 0);
        
        foreach (var wheel in frontWheels)
            wheel.localRotation = Quaternion.Lerp(wheel.localRotation, wheelTargetRotation, Time.deltaTime * 10);
        


    }
}
