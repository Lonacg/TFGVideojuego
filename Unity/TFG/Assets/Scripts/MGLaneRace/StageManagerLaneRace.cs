using System;
using UnityEngine;
using TMPro;



public class StageManagerLaneRace : MonoBehaviour
{

    public int numberCorrectAnswers = 0;
    public int numberIncorrectAnswers = 0;
    public int neededScore = 3;
    public GameObject ground;
    public GameObject grannyPlayer;
    public GameObject confetyParticles;
    public GameObject[] gates;
    public GameObject[] finishGates;

    public int currentGround = 0;
    
    public TextMeshProUGUI scoreText;


    public delegate void _OnVictory();
    public static event _OnVictory OnVictory;


    void OnEnable(){
        TriggerGate.OnWellSol += HandleOnWellSol;
        TriggerGate.OnWrongSol += HandleOnWrongSol;
        GrannyMovement.OnParty += HandleOnParty;

    }

    void OnDisable(){
        TriggerGate.OnWellSol -= HandleOnWellSol;
        TriggerGate.OnWrongSol -= HandleOnWrongSol;
        GrannyMovement.OnParty += HandleOnParty;


    }



    void HandleOnWellSol(){
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


    public void IncreaseCurrentGround(){
        if(currentGround == 2){
            currentGround = 0;
        }
        else
            currentGround ++;

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "0/" + neededScore;
        numberCorrectAnswers = 0;
        numberIncorrectAnswers = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }






}