using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class CarEngine : MonoBehaviour
{

    public EngineSO engineData;
    public new Rigidbody rigidbody { get; protected set; }
    
    private Vector3 desiredVelocity;
    private void OnValidate()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
    }

    public void SetDesiredVelocity(Vector3 newDesiredVelocity)
    {
        desiredVelocity = newDesiredVelocity;
    }


    private void FixedUpdate()
    {
        rigidbody.AddForce(desiredVelocity * engineData.speed * Time.deltaTime);
    }
}
