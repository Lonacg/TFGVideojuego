using System;
using UnityEngine;
using TMPro;
using System.Collections;




public class StageManagerLaneRace : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private int numberCorrectAnswers = 0;
    [SerializeField] private int numberIncorrectAnswers = 0;
    private readonly int neededScore = 5;
    private int currentGround = 0;
    private int extraTerrainsPlaced = 0;

    [Header("Game Objects:")]
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject terrain;
    [SerializeField] private GameObject extraTerrain;
    [SerializeField] private GameObject grannyPlayer;
    [SerializeField] private GameObject confettiParticles;
    [SerializeField] private GameObject particlesGreen;
    [SerializeField] private GameObject particlesRed;
    [SerializeField] private GameObject[] gates;
    [SerializeField] private GameObject[] finishGates;
    [SerializeField] private GameObject[] nextOpTriggers;



    
    [Header("Text:")]
    public TextMeshProUGUI textOperationPlace;
    public TextMeshProUGUI scoreText;



    public delegate void _OnVictory();          // El evento de victoria se lanza en el momento en el que se consiguen los aciertos objetivos (3 por defecto) (OnFinalLine es cuando cruza la meta)
    public static event _OnVictory OnVictory;


    public delegate void _OnMiddleVelocity();          
    public static event _OnMiddleVelocity OnMiddleVelocity;

    public delegate void _LowVelocity();          
    public static event _LowVelocity OnLowVelocity;


    void OnEnable(){
        GrannyMovement.OnGo += HandleOnGo;
        TriggerGate.OnCorrectSol += HandleOnCorrectSol;
        TriggerGate.OnWrongSol += HandleOnWrongSol;
        TriggerIncreaseCurrentGround.OnIncreaseCurrentGround += HandleOnIncreaseCurrentGround;
        TriggerNextOperation.OnNextOperation += HandleOnNextOperation;
        TriggerExtraTerrain.OnNewGround += HandleOnNewGround;
        GrannyMovement.OnParty += HandleOnParty;
    }

    void OnDisable(){
        GrannyMovement.OnGo -= HandleOnGo;
        TriggerGate.OnCorrectSol -= HandleOnCorrectSol;
        TriggerGate.OnWrongSol -= HandleOnWrongSol;
        TriggerIncreaseCurrentGround.OnIncreaseCurrentGround -= HandleOnIncreaseCurrentGround;
        TriggerNextOperation.OnNextOperation -= HandleOnNextOperation;
        TriggerExtraTerrain.OnNewGround -= HandleOnNewGround;
        GrannyMovement.OnParty -= HandleOnParty;
    }

    void Start()
    {        
        scoreText.text = "0/" + neededScore;
        numberCorrectAnswers = 0;
        numberIncorrectAnswers = 0;
    }

    

    private void HandleOnGo(){
        StartCoroutine(WaitXSecondAndChangeOperation(seconds: 1.1f));
    }

    private void HandleOnCorrectSol(){
        StartCoroutine(LaunchParticles(particlesGreen));
        
        numberCorrectAnswers ++;
        scoreText.text = numberCorrectAnswers + "/" + neededScore;

        if(numberCorrectAnswers == neededScore){
                    
            // Activamos la meta en la siguiente puerta, desactivamos los numeros que tenia y borramos la siguiente linea de puertas
            finishGates[currentGround].SetActive(true);     // currentGround, porque segun se cruza una puerta se aumenta en uno y estamos en la siguiente
            gates[currentGround].SetActive(false);

            //Destruimos el trigger de la meta que llama a cambiar la operacion, ya que esa funcion accede a la operacion que genera gates[_], y al borrarla en la siguiente linea, cuando cruza el trigger nos salta un null refence con razon
            Destroy(nextOpTriggers[currentGround]);

            // Destruimos el siguiente bloque de puertas a la meta para que no se vea al cruzarla 
            if(currentGround == 2){
                Destroy(gates[0]);
            }
            else{
                Destroy(gates[currentGround + 1]);
            }

            // Lanzamos el evento de que se ha llegado al maximo de aciertos necesario
            if(OnVictory != null)   
                OnVictory();
        }
    }

    private void HandleOnWrongSol(){
        StartCoroutine(LaunchParticles(particlesRed));

        numberIncorrectAnswers ++;

        // Cuando falla 2 veces, reducimos en 1 la velocidad del movimiento para facilitarselo y cambiamos la animacion de correr
        if(numberIncorrectAnswers  == 2){
            // Lanzamos el evento de reducir la velocidad a la intermedia
            if(OnMiddleVelocity != null)   
                OnMiddleVelocity();

            // ground.GetComponent<GroundMovement>().groundSpeed -= 1;
            // grannyPlayer.GetComponent<GrannyMovement>().ChangeAnimation("Running");
        }
        // Cuando falla 4 veces, reducimos en 0.5 mas la velocidad del movimiento y cambiamos la animacion de correr
        if(numberIncorrectAnswers  == 4){
            // Lanzamos el evento de reducir la velocidad a la mas baja
            if(OnLowVelocity != null)   
                OnLowVelocity();

            //ground.GetComponent<GroundMovement>().groundSpeed -= 0.5f;
            //grannyPlayer.GetComponent<GrannyMovement>().ChangeAnimation("SlowRunning");
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

    private void HandleOnIncreaseCurrentGround(){
        IncreaseCurrentGround();
    }


    private void HandleOnParty(){
        confettiParticles.SetActive(true); 
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

    IEnumerator LaunchParticles(GameObject particles){
        particles.SetActive(true); 
        yield return new WaitForSeconds(1);
        particles.SetActive(false); 
    }


}