using UnityEngine;

public class TriggerFinalGate : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            // PARAR LA VELOCIDAD DE GROUND Y PONER LA ANIMACION DE GANANDO

        }
    }
}
