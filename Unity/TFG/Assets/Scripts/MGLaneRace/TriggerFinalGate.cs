using UnityEngine;

public class TriggerFinalGate : MonoBehaviour
{

    public delegate void _OnFinalLine();
    public static event _OnFinalLine OnFinalLine;


    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            if(OnFinalLine != null)   
                OnFinalLine();
        }
    }
}
