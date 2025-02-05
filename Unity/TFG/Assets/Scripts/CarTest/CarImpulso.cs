using Unity.VisualScripting;
using UnityEngine;

public class CarImpulso : MonoBehaviour
{
    private Vector2 inputMovement = Vector2.zero;

    [SerializeField] private float movSpeed = 10;
    [SerializeField] private float rotationSpeed = 250;
    private Rigidbody rb;

    public Transform[] frontWheels;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }


    void FixedUpdate()
    {
        // Movimiento del vehiculo
        if(inputMovement.y != 0){
            // float speed = movSpeed > maxSpeed ? maxSpeed : movSpeed;  // si condicion, entonces, ? si verdadero : si falso

            // Movimiento hacia delante o hacia atras            
            rb.AddForce(gameObject.transform.forward * inputMovement.y * movSpeed * Time.fixedDeltaTime,  ForceMode.Impulse );
            
            // Giro a izquierda y derecha
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



        Debug.Log(rb.linearVelocity);


    }


    
}
