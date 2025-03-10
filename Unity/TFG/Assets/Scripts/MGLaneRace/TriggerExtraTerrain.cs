using UnityEngine;

public class TriggerExtraTerrain : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnNewGround();
    public static event _OnNewGround OnNewGround;
  


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if(OnNewGround != null)   
                OnNewGround();
        }
    }
}
