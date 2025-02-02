using UnityEngine;

public class CarTest : MonoBehaviour
{

    private float movLat;
    private float movDelante;

    public float movSpeed = 10;
    public float rotationSpeed = 250;
    
    public Rigidbody rb;




    void Update()
    {
        movLat = Input.GetAxisRaw("Horizontal");
        movDelante = Input.GetAxisRaw("Vertical");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * movDelante * movSpeed, ForceMode.Acceleration);

    }
}
