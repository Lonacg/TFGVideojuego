using UnityEngine;
using System.Collections;


public class TriggerGate : MonoBehaviour
{


    public delegate void _OnWellSol();
    public static event _OnWellSol OnWellSol;

    public delegate void _OnWrongSol();
    public static event _OnWrongSol OnWrongSol;
  


    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            if(gameObject.tag == "CorrectAnswer"){
                if(OnWellSol != null)   
                    OnWellSol();
            }
            if(gameObject.tag == "IncorrectAnswer"){
                if(OnWrongSol != null)   
                    OnWrongSol();
            }
        }
    }



}


