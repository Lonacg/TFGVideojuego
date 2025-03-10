using UnityEngine;

public class TriggerFinalGate : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnFinalLine();
    public static event _OnFinalLine OnFinalLine;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if(OnFinalLine != null)   
                OnFinalLine();
        }
    }
    
}
