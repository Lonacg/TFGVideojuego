using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public float groundSpeed = 5 ;
    private bool wantMove = false;



    void OnEnable(){
        GrannyMovement.OnGo += HandleOnGo;
        StageManagerLaneRace.OnVictory += HandleOnVictory;
        TriggerFinalGate.OnFinalLine += HandleOnFinalLine; 
    }

    void OnDisable(){
        GrannyMovement.OnGo -= HandleOnGo;
        StageManagerLaneRace.OnVictory -= HandleOnVictory;
        TriggerFinalGate.OnFinalLine -= HandleOnFinalLine;
    }



    public void HandleOnGo(){
        StartCoroutine(WaitXSecondAndGo(seconds: 0.45f));
    }

    public void HandleOnVictory(){
        groundSpeed = 5;
    }

    public void HandleOnFinalLine(){
        StartCoroutine(StopMovement());
    }



    void Start(){
        wantMove = false;        
    }

    void Update()
    {
        if(wantMove)
            transform.Translate(0, 0, Time.deltaTime * - groundSpeed );
    }



    IEnumerator WaitXSecondAndGo(float seconds){
        yield return new WaitForSeconds(seconds);
        wantMove = true;
    }
    
    IEnumerator StopMovement(){
        // Slow Running
        groundSpeed = 3;
        yield return new WaitForSeconds(1.5f);
        
        // Walking
        groundSpeed = 2;
        yield return new WaitForSeconds(1f);
        groundSpeed = 1;
        yield return new WaitForSeconds(0.5f);
        
        // Parado
        wantMove = false;        
    }
    
}
