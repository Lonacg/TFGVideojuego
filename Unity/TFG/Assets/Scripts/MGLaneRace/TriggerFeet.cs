using UnityEngine;

public class TriggerFeet : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnFeetInGround();
    public static event _OnFeetInGround OnFeetInGround;
  


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Ground"))
            if(OnFeetInGround != null)   
                OnFeetInGround();
    }
}
