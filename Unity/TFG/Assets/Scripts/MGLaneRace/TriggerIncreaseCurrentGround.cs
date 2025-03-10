using UnityEngine;

public class TriggerIncreaseCurrentGround : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnIncreaseCurrentGround();
    public static event _OnIncreaseCurrentGround OnIncreaseCurrentGround;
  


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
            if(OnIncreaseCurrentGround != null)   
                OnIncreaseCurrentGround();
    }
    
}
