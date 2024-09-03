using System;
using UnityEngine;
using TMPro;
using System.Collections;




public class StageManagerLaneRace : MonoBehaviour
{
    [Header("Variables")]
    public int numberCorrectAnswers = 0;
    public int numberIncorrectAnswers = 0;
    public int neededScore = 3;
    public int currentGround = 0;

    [Header("Game Objects:")]
    public GameObject ground;
    public GameObject grannyPlayer;
    public GameObject confetyParticles;
    public GameObject[] gates;
    public GameObject[] finishGates;

    
    [Header("Text:")]
    public TextMeshProUGUI textOperationPlace;
    public TextMeshProUGUI scoreText;



    public delegate void _OnVictory();
    public static event _OnVictory OnVictory;


    void OnEnable(){
        GrannyMovement.OnGo += HandleOnGo;
        TriggerGate.OnWellSol += HandleOnWellSol;
        TriggerGate.OnWrongSol += HandleOnWrongSol;
        NextOperationTrigger.OnNextOperation += HandleOnNextOperation;
        GrannyMovement.OnParty += HandleOnParty;

    }

    void OnDisable(){
        GrannyMovement.OnGo -= HandleOnGo;
        TriggerGate.OnWellSol -= HandleOnWellSol;
        TriggerGate.OnWrongSol -= HandleOnWrongSol;
        NextOperationTrigger.OnNextOperation -= HandleOnNextOperation;
        GrannyMovement.OnParty -= HandleOnParty;


    }

    private void HandleOnGo(){
        StartCoroutine(WaitXSecondAndChangeOperation(seconds: 1.1f));
    }

    private void HandleOnWellSol(){
        numberCorrectAnswers ++;
        scoreText.text = numberCorrectAnswers + "/" + neededScore;

        IncreaseCurrentGround();

        if(numberCorrectAnswers == neededScore){
                    
            // Activamos la meta en la siguiente puerta, desactivamos los numeros que tenia y borramos la siguiente linea de puertas
            finishGates[currentGround].SetActive(true);
            gates[currentGround].SetActive(false);
            if(currentGround == 2)
                Destroy(gates[0]);
            else
                Destroy(gates[currentGround + 1]);


            // Lanzamos el evento de que se ha llegado al maximo de aciertos necesario
            if(OnVictory != null)   
                OnVictory();

        }

    }

    private void HandleOnWrongSol(){
        numberIncorrectAnswers ++;
        IncreaseCurrentGround();

        // Cuando falla 2 veces, reducimos en 1 la velocidad del movimiento para facilitarselo y cambiamos la animacion de correr
        if(numberIncorrectAnswers  == 2){
            ground.GetComponent<GroundMovement>().groundSpeed -= 1;
            grannyPlayer.GetComponent<GrannyMovement>().ChangeAnimation("Running");
        }
        // Cuando falla 4 veces, reducimos en 1 la velocidad del movimiento otra vez y cambiamos la animacion de correr
        if(numberIncorrectAnswers  == 4){
            ground.GetComponent<GroundMovement>().groundSpeed -= 0.5f;
            grannyPlayer.GetComponent<GrannyMovement>().ChangeAnimation("SlowRunning");
        }
    }


    private void HandleOnParty(){
        confetyParticles.SetActive(true);
    }


    private void HandleOnNextOperation(){
        ChangeOperation();

    }


    public void ChangeOperation(){
        GameObject currentGate = gates[currentGround];
        SetOperationLaneRace scriptSetOperation = currentGate.GetComponent<SetOperationLaneRace>();

        int firstNumber = scriptSetOperation.firstNumber;
        int secondNumber = scriptSetOperation.secondNumber;

        string symbol = scriptSetOperation.symbol;

        textOperationPlace.text = "";
        textOperationPlace.text = firstNumber + symbol + secondNumber;
    }

    public void IncreaseCurrentGround(){
        if(currentGround == 2){
            currentGround = 0;
        }
        else
            currentGround ++;

    }


    void Start()
    {
        scoreText.text = "0/" + neededScore;
        numberCorrectAnswers = 0;
        numberIncorrectAnswers = 0;
    }


    IEnumerator WaitXSecondAndChangeOperation(float seconds){
        yield return new WaitForSeconds(seconds);
        ChangeOperation();

    }

    IEnumerator ShowNewOperation(){
        StartCoroutine(TransformSizeFont(startSize: 54.4f, endSize: 0, animationTime: 1));
        yield return new WaitForSeconds(1);
        ChangeOperation();
        StartCoroutine(TransformSizeFont(startSize: 0, endSize: 54.4f, animationTime: 1));
    }


    IEnumerator TransformSizeFont(float startSize, float endSize, float animationTime){
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            textOperationPlace.fontSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

    }




}