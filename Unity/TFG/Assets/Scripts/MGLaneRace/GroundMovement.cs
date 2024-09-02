using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    public delegate void _OnGo();
    public static event _OnGo OnGo;

    public float groundSpeed = 5 ;
    private float elapsedTime;
    private bool wantMove = false;


    void OnEnable(){
        TriggerFinalGate.OnFinalLine += HandleOnFinalLine;
    }

    void OnDisable(){
        TriggerFinalGate.OnFinalLine -= HandleOnFinalLine;

    }


    public void HandleOnFinalLine(){
        StartCoroutine(StopMovement());

    }




    void Start(){
        wantMove = false;
        StartCoroutine(WaitForXSeconds(3f));
    }

    void Update()
    {
        if(wantMove)
            transform.Translate(0, 0, Time.deltaTime * - groundSpeed );
        
    }

    IEnumerator WaitForXSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        if(OnGo != null)   
            OnGo();
        wantMove = true;

    }
    IEnumerator StopMovement(){
        yield return new WaitForSeconds(1f);
        groundSpeed = 2;

        yield return new WaitForSeconds(1f);
        groundSpeed = 1;
        yield return new WaitForSeconds(0.5f);
        
        wantMove = false;        

    }

}
