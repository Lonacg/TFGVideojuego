using System.Collections;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    private float groundSpeed;  
    private float maxGroundSpeed = 5 ;  
    private bool wantMove = false;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable(){
        GrannyMovement.OnGo                   += HandleOnGo;
        StageManagerLaneRace.OnMiddleVelocity += HandleOnMiddleVelocity;
        StageManagerLaneRace.OnLowVelocity    += HandleOnLowVelocity;
        StageManagerLaneRace.OnVictory        += HandleOnVictory;
        TriggerFinalGate.OnFinalLine          += HandleOnFinalLine; 
    }

    void OnDisable(){
        GrannyMovement.OnGo                   -= HandleOnGo;
        StageManagerLaneRace.OnMiddleVelocity -= HandleOnMiddleVelocity;
        StageManagerLaneRace.OnLowVelocity    -= HandleOnLowVelocity;
        StageManagerLaneRace.OnVictory        -= HandleOnVictory;
        TriggerFinalGate.OnFinalLine          -= HandleOnFinalLine;
    }

    void Start()
    {
        wantMove = false;   
        maxGroundSpeed = 5;     
        groundSpeed = maxGroundSpeed;     
    }

    void Update()
    {
        if(wantMove)
            transform.Translate(0, 0, Time.deltaTime * - groundSpeed );
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    public void HandleOnGo(){
        StartCoroutine(WaitXSecondAndGo(seconds: 0.45f));
    }

    public void HandleOnMiddleVelocity(){
        StartCoroutine(ChangeGroundVelocity(desiredVel: groundSpeed - 1, animationTime: 1f));
    }

    public void HandleOnLowVelocity(){
        StartCoroutine(ChangeGroundVelocity(desiredVel: groundSpeed - 0.5f));
    }

    public void HandleOnVictory(){
        StartCoroutine(ChangeGroundVelocity(desiredVel: maxGroundSpeed, animationTime: 2f));
    }

    public void HandleOnFinalLine(){
        StartCoroutine(StopMovement());
    }



    // CORRUTINAS
    IEnumerator WaitXSecondAndGo(float seconds){
        yield return new WaitForSeconds(seconds);
        wantMove = true;
    }

    IEnumerator ChangeGroundVelocity(float desiredVel, float animationTime = 0.5f){
        // Corrutina reutilizada parcialmente del parking: ErrorXBehaviour.cs
        float elapsedTime = 0;
        float startVel = groundSpeed;
        while(elapsedTime < animationTime){
            groundSpeed = Mathf.Lerp(startVel, desiredVel, elapsedTime / animationTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        groundSpeed = desiredVel;
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
