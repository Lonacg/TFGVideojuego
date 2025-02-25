using UnityEngine;

public class TriggerFinalGate : MonoBehaviour
{
    // Declaracion de eventos:
    public delegate void _OnFinalLine();
    public static event _OnFinalLine OnFinalLine;



    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if(OnFinalLine != null)   
                OnFinalLine();
        }
    }
    
}