using UnityEngine;

public class TriggerFeet : MonoBehaviour
{
    // Declaracion de eventos:
    public delegate void _OnFeetInGround();
    public static event _OnFeetInGround OnFeetInGround;
  


    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ground"))
            if(OnFeetInGround != null)   
                OnFeetInGround();
    }
}
