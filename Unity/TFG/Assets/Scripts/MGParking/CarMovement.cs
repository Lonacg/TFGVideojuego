using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float movSpeed = 150;
    [SerializeField] private float rotationSpeed = 150;
    [SerializeField] private Transform[] frontWheels;

    private Vector2 inputMovement = Vector2.zero;
    private Rigidbody rb;
    

    void Start(){
        // Asignamos el Rigidbody
        rb = GetComponent<Rigidbody>();
    }


    void Update(){
        // Input del teclado para el movimiento
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }


    void FixedUpdate(){
        // Movimiento del vehiculo aplicando la fisica y en el FixedUpdate para que sean siempre los mismos frames
        if(inputMovement.y != 0){
            // float speed = movSpeed > maxSpeed ? maxSpeed : movSpeed;  // si condicion, entonces, ? si verdadero : si falso

            // Movimiento hacia delante o hacia atras            
            rb.AddForce(gameObject.transform.forward * inputMovement.y * movSpeed * Time.fixedDeltaTime,  ForceMode.Impulse );
            
            // Giro a izquierda y derecha
            transform.Rotate(xAngle: 0, yAngle: inputMovement.x * Time.deltaTime * rotationSpeed, zAngle: 0);
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