using System;
using UnityEngine;
using TMPro;
using System.Collections;




public class StageManagerLaneRace : MonoBehaviour
{
    [Header("Variables")]
    public int numberCorrectAnswers = 0;
    public int numberIncorrectAnswers = 0;
    private int neededScore = 3;
    private int currentGround = 0;
    private int extraTerrainsPlaced = 0;

    [Header("Game Objects:")]
    public GameObject ground;
    public GameObject terrain;
    public GameObject extraTerrain;
    public GameObject grannyPlayer;
    public GameObject confetyParticles;
    public GameObject[] gates;
    public GameObject[] finishGates;

    
    [Header("Text:")]
    public TextMeshProUGUI textOperationPlace;
    public TextMeshProUGUI scoreText;



    public delegate void _OnVictory();          // El evento de victoria se lanza en el momento en el que se consiguen 3 aciertos (OnFinalLine es cuando cruza la meta)
    public static event _OnVictory OnVictory;



    void OnEnable(){
        GrannyMovement.OnGo += HandleOnGo;
        TriggerGate.OnWellSol += HandleOnWellSol;
        TriggerGate.OnWrongSol += HandleOnWrongSol;
        NextOperationTrigger.OnNextOperation += HandleOnNextOperation;
        TriggerExtraTerrain.OnNewGround += HandleOnNewGround;
        GrannyMovement.OnParty += HandleOnParty;
    }

    void OnDisable(){
        GrannyMovement.OnGo -= HandleOnGo;
        TriggerGate.OnWellSol -= HandleOnWellSol;
        TriggerGate.OnWrongSol -= HandleOnWrongSol;
        NextOperationTrigger.OnNextOperation -= HandleOnNextOperation;
        TriggerExtraTerrain.OnNewGround -= HandleOnNewGround;
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
        // Cuando falla 4 veces, reducimos en 0.5 mas la velocidad del movimiento y cambiamos la animacion de correr
        if(numberIncorrectAnswers  == 4){
            ground.GetComponent<GroundMovement>().groundSpeed -= 0.5f;
            grannyPlayer.GetComponent<GrannyMovement>().ChangeAnimation("SlowRunning");
        }
    }

    private void HandleOnNextOperation(){
        StartCoroutine(ShowNewOperation());
    }

    private void HandleOnNewGround(){
        // Indice para contar cuantos terrenos extra se han colocado para calcular la posicion del nuevo a instanciar
        extraTerrainsPlaced ++;

        Vector3 newPosition = new Vector3(0, 0, 1000 * extraTerrainsPlaced);    // 1000 es el z de los terrenos por defeco en Unity

        // Instanciamos un terreno nuevo como hijo de Ground
        GameObject newTerrain = Instantiate(extraTerrain, terrain.transform);
        
        // Lo colocamos en la posicion local correcta (para que no sea respecto al padre)
        newTerrain.transform.localPosition = newPosition;
    }

    private void HandleOnParty(){
        confetyParticles.SetActive(true); 
    }



    void Start(){
        // positionCurrentTerrain = terrain.transform.localPosition;
        
        scoreText.text = "0/" + neededScore;
        numberCorrectAnswers = 0;
        numberIncorrectAnswers = 0;
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