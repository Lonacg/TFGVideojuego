using UnityEngine;
using System.Collections;


public class CameraManager : MonoBehaviour  
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 0.5f; 
    private Vector3 target;
    private Vector3 originalOffset;
    private Vector3 offset;
    private Vector3 finalOffset;
    private Quaternion orignalRotation;
    private Vector3 originalPosition;
    private Vector3 RigthPosition;
    private Vector3 LeftPosition;
    private bool firstMovement;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()
    {
        CanvasManagerLaneRace.OnStart += HandleOnStart;
        TriggerFinalGate.OnFinalLine  += HandleOnFinalLine;
    }

    void OnDisable()
    {
        CanvasManagerLaneRace.OnStart -= HandleOnStart;
        TriggerFinalGate.OnFinalLine  -= HandleOnFinalLine;
    }

    private void Start()
    {
        // Rotacion inicial
        orignalRotation = Quaternion.Euler(39.74f, 0, 0);
        originalPosition = new Vector3(0, 7.71f, -7.73f);

        // Posicion inicial
        transform.SetPositionAndRotation(originalPosition, orignalRotation);

        // Lugar al que apunta la camara
        originalOffset = new Vector3(0, 1.5f, 2.4f);
        finalOffset = new Vector3(0, 1.5f, 0);
        offset = originalOffset;
        target = player.transform.position + originalOffset;

        // Posiciones intermedias de la cámara
        RigthPosition = new Vector3(2.33f, 1.31f, 1.13f);
        LeftPosition = new Vector3(-2.33f, 1.31f, 1.13f);
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnStart(){
        firstMovement = true;
        StartCoroutine(MoveCamera());
    }

    private void HandleOnFinalLine(){
        StartCoroutine(MoveCamera());
    }



    // CORRUTINAS
    IEnumerator MoveCamera(){

        float animationTime = 3;

        // Movimiento de la posicion inicial a posicion derecha
        float elapsedTime = 0;
        
        while(elapsedTime <= animationTime){
            offset = Vector3.Lerp(offset, finalOffset, speed * Time.deltaTime);
            target = player.transform.position + offset;

            transform.LookAt(target);

            transform.position = Vector3.Lerp(transform.position, RigthPosition, speed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }

        // Movimiento de la posicion derecha a posicion izquierda
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
