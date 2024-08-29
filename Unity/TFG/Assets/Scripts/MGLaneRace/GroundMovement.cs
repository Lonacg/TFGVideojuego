using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    public float groundSpeed = 5 ;


    void Update()
    {

        transform.Translate(0, 0, Time.deltaTime * -groundSpeed );
        
    }



}
