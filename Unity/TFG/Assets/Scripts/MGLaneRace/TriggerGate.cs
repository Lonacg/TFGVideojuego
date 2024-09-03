using UnityEngine;
using System.Collections;


public class TriggerGate : MonoBehaviour
{

    public GameObject grannyPlayer;

    public delegate void _OnWellSol();
    public static event _OnWellSol OnWellSol;

    public delegate void _OnWrongSol();
    public static event _OnWrongSol OnWrongSol;
    public int currentGateIndex;
    public int realPlayerPosition;

    void Start()
    {
        // Asignamos a Player
        grannyPlayer = GameObject.Find("GrannyPlayer");

        // Guardamos en que puerta esta este script (izq, centro o derecha)
        CheckIndexGate();
    }

    void Update()
    {
        // Guardamos a que puerta se esta dirigiendo player
        CheckPlayerDirection();
    }


    public void CheckIndexGate(){
        if(gameObject.transform.position.x == -4){
            // Es la puerta izquierda
            currentGateIndex = 0;
        }
        else{
            if(gameObject.transform.position.x == 0){
                // Es la puerta central
                currentGateIndex = 1;
            }
            else{
                // Es la puerta derecha
                currentGateIndex = 2;
            }
        }
    }

    public void CheckPlayerDirection(){
        realPlayerPosition = grannyPlayer.GetComponent<GrannyMovement>().currentIndex;
    }


    void OnTriggerEnter(Collider other){
        // Añadimos el primer && para evitar que player pase justamente entre 2 puertas y sea detectado por ambos colliders, asi, no se tiene en cuenta la posicion actual de player, si no hacia donde estaba yendo
        if (other.tag == "Player" && currentGateIndex == realPlayerPosition){
            if(gameObject.tag == "CorrectAnswer"){
                if(OnWellSol != null)   
                    OnWellSol();
            }
            if(gameObject.tag == "IncorrectAnswer"){
                if(OnWrongSol != null)   
                    OnWrongSol();
            }

            StartCoroutine(HideGates());

        }
    }





    IEnumerator HideGates(){
        yield return 0;

    }



}




/*
    void OnEnable(){
        TriggerGate.OnWellSol += HandleOnWellSol;
        TriggerGate.OnWrongSol += HandleOnWrongSol;

    }

    void OnDisable(){
        TriggerGate.OnWellSol -= HandleOnWellSol;
        TriggerGate.OnOnWrongSol -= HandleOnWrongSol;
    }
*/

