using UnityEngine;
using System.Collections;


public class TriggerGate : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnCorrectSol();
    public static event _OnCorrectSol OnCorrectSol;

    public delegate void _OnWrongSol();
    public static event _OnWrongSol OnWrongSol;
  


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if(gameObject.CompareTag("CorrectAnswer")){
                if(OnCorrectSol != null)   
                    OnCorrectSol();
            }
            if(gameObject.CompareTag("IncorrectAnswer")){
                if(OnWrongSol != null)   
                    OnWrongSol();
            }
        }
    }

}
