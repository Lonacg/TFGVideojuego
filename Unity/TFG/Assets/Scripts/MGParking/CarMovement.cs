using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Movement:")]
    public float movSpeed = 10;
    public float rotationSpeed = 250;
    public float inertia;
    private Vector2 inputMovement = Vector2.zero;
    private Rigidbody rb;


    [Header("Accessories")]
    public Transform[] frontWheels;


    void Start()
    {
        // Asignamos el Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update(){
        // Input del teclado para el movimiento
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    }

    void FixedUpdate()
    {
        // Movemos el coche aplicando la fisica y en el FixedUpdate para que sean siempre los mismos frames
        Vector3 velocity = movSpeed * (inputMovement.y * transform. forward); 
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        if(inputMovement.y != 0){ // QUITANDO ESTE IF PIERDE REALISMO PERO GANA MANEJABILIDAD
            transform.Rotate(0, inputMovement.x * Time.deltaTime * rotationSpeed, 0);
        }


        // Giro de las ruedas delanteras
        Quaternion wheelTargetRotation = Quaternion.identity;
        if (inputMovement.x < 0) // Giro a la izquierda
            wheelTargetRotation = Quaternion.Euler(0, -30, 0);
        
        if (inputMovement.x > 0) // Giro a la derecha
            wheelTargetRotation = Quaternion.Euler(0, 30, 0);
        
        foreach (var wheel in frontWheels)
            wheel.localRotation = Quaternion.Lerp(wheel.localRotation, wheelTargetRotation, Time.deltaTime * 10);
    }

}