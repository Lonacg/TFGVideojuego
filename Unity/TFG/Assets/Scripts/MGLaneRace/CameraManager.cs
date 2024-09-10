using UnityEngine;
using System.Collections;



public class CameraManager : MonoBehaviour  
{
    public GameObject player;
    public float speed = 0.5f; 
    
    private Vector3 target;
    private Vector3 originalOffset;
    private Vector3 offset;
    private Vector3 finalOffset;
    private Quaternion orignalRotation;
    private Vector3 originalPosition;
    private Vector3 RigthPosition;
    private Vector3 LeftPosition;
    
    private bool firstMovement;



    void OnEnable(){
        CanvasManager.OnStart += HandleOnStart;
        TriggerFinalGate.OnFinalLine += HandleOnFinalLine;
    }

    void OnDisable(){
        CanvasManager.OnStart -= HandleOnStart;
        TriggerFinalGate.OnFinalLine -= HandleOnFinalLine;
    }

    private void HandleOnStart()
    {
        firstMovement = true;
        StartCoroutine(MoveCamera());
    }

    private void HandleOnFinalLine()
    {
        StartCoroutine(MoveCamera());
    }

    private void Start()
    {
        // Rotacion inicial
        orignalRotation = Quaternion.Euler(39.74f, 0, 0);
        originalPosition = new Vector3(0, 7.71f, -7.73f);

        // Posicion inicial
        transform.position = originalPosition;
        transform.rotation = orignalRotation;

        // Lugar al que apunta la camara
        originalOffset = new Vector3(0, 1.5f, 2.4f);
        finalOffset = new Vector3(0, 1.5f, 0);
        offset = originalOffset;
        target = player.transform.position + originalOffset;

        // Posiciones intermedias de la cámara
        RigthPosition = new Vector3(2.33f, 1.31f, 1.13f);
        LeftPosition = new Vector3(-2.33f, 1.31f, 1.13f);
    }


    IEnumerator MoveCamera(){

        float animationTime = 3;

        // Posicion Inicial a posicion derecha
        float elapsedTime = 0;
        
        while(elapsedTime <= animationTime){
            offset = Vector3.Lerp(offset, finalOffset, speed * Time.deltaTime);

            target = player.transform.position + offset;

            transform.LookAt(target);

            transform.position = Vector3.Lerp(transform.position, RigthPosition, speed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }

        // Posicion derecha a posicion izquierda
        animationTime = 2;
        elapsedTime = 0;
        
        while(elapsedTime <= animationTime){

            transform.LookAt(target);

            transform.position = Vector3.Lerp(transform.position, LeftPosition, speed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }


        if(firstMovement){
            // Vuelta a la posicion original
            elapsedTime = 0;
            animationTime = 5;
            
            while(elapsedTime <= animationTime){
                offset = Vector3.Lerp(offset, originalOffset, speed * Time.deltaTime);

                target = player.transform.position + offset;

                transform.LookAt(target);

                transform.position = Vector3.Lerp(transform.position, originalPosition, speed * Time.deltaTime);

                elapsedTime += Time.deltaTime;
                yield return 0;
            }
            firstMovement = false;
        }
    }

}