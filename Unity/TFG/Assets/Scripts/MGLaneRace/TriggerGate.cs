using UnityEngine;
using System.Collections;


public class TriggerGate : MonoBehaviour
{
    public GameObject finishLine;


    public delegate void _OnWellSol();
    public static event _OnWellSol OnWellSol;

    public delegate void _OnWrongSol();
    public static event _OnWrongSol OnWrongSol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

