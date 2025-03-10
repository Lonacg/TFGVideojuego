using UnityEngine;

public class TriggerNextOperation : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnNextOperation();
    public static event _OnNextOperation OnNextOperation;
  


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
            if(OnNextOperation != null)   
                OnNextOperation();
    }

}
