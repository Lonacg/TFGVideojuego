using System.Collections;
using TMPro;
using UnityEngine;

public class RoundBehaviour : MonoBehaviour
{

    [Header("References:")]
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private TextMeshProUGUI attempText;
    [SerializeField] private GameObject stageManager;


    [Header("Variables:")]
    private int totalRounds;
    private Vector3[] goingOutPositions;
    private Vector3[] goingInPositions;



    public delegate void _OnShowAttempt();
    public static event _OnShowAttempt OnShowAttempt;

    public delegate void _OnRoundMovementSFX();
    public static event _OnRoundMovementSFX OnRoundMovementSFX;

    void OnEnable()
    {
        StageManagerDeduceSign.OnNewRound += HandleOnNewRound;
        StageManagerDeduceSign.OnFadeOutAll += HandleOnFadeOutAll;
    }


    void OnDisable(){
        StageManagerDeduceSign.OnNewRound -= HandleOnNewRound;
        StageManagerDeduceSign.OnFadeOutAll -= HandleOnFadeOutAll;
    }



    private void HandleOnNewRound(bool sameRound){

        if(sameRound){
            StartCoroutine(GoIn());  

        }
        else{
            StartCoroutine(GoOutGoIn());              
        }
    }


    private void HandleOnFadeOutAll(){
        StartCoroutine(GoOut()); 
    }



    void Start(){

        // Inicializamos variables
        goingInPositions = new Vector3[2];
        goingOutPositions = new Vector3[2];
        SetPositionsRound();
        

        // Comprobamos cual es el numero de rondas que debe superar Player
        totalRounds = stageManager.GetComponent<StageManagerDeduceSign>().totalRounds;


        // Activamos el movimiento del texto Ronda 1
        gameObject.GetComponent<TextMeshProUGUI>().text = "RONDA 1";
        StartCoroutine(MoveRound(goingInPositions[0], goingInPositions[1], curve, mustNotifyAttempt: true));
        
        // Avisamos para que se reproduzca el sonido de movimiento
        if(OnRoundMovementSFX != null){
            OnRoundMovementSFX();
        }
    }



    private void SetPositionsRound(){
        
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition;

        // Posiciones cuando va a entrar en pantalla 
        startPosition.x = -650;
        endPosition.x =  0;         // No haria falta, ya que ya es cero, pero por facilitar la comprension
        
        goingInPositions[0] = startPosition;
        goingInPositions[1] = endPosition;

        // Posiciones cuando va a salir de la pantalla 
        startPosition.x = 0;        // No haria falta, ya que ya es cero, pero por facilitar la comprension
        endPosition.x =  650;         
        
        goingOutPositions[0] = startPosition;
        goingOutPositions[1] = endPosition;    
    }


    private void NewRoundText(){

        int currentRound = stageManager.GetComponent<StageManagerDeduceSign>().currentRound;

        if(currentRound == totalRounds){
            gameObject.GetComponent<TextMeshProUGUI>().text = "RONDA FINAL";
        }
        else{
            gameObject.GetComponent<TextMeshProUGUI>().text = "RONDA " + currentRound.ToString();
        }
    }



    IEnumerator MoveRound(Vector3 startPosition, Vector3 endPosition, AnimationCurve curve, bool mustNotifyAttempt = false, float animationTime = 1){

        // // Avisamos para que se reproduzca el sonido de movimiento
        // if(OnRoundMovementSFX != null){
        //     OnRoundMovementSFX();
        // }

        // necesitamos mustNotifyAttempt porque esta funcion vale tanto para cuando se va (que no muestra los intentos a continuacion suya), como para cuando llega (que si los muestra) 
        float elapsedTime = 0;
        
        while(elapsedTime < animationTime){
            
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(elapsedTime / animationTime));

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = endPosition;

        // Avisamos para que se muestren los intentos diponibles del nivel
        if(OnShowAttempt != null && mustNotifyAttempt)   
            OnShowAttempt();
    }


    IEnumerator GoOutGoIn(){
        // Sale la ronda actual
        StartCoroutine(MoveRound(goingOutPositions[0], goingOutPositions[1], curve));

        yield return new WaitForSeconds(1);

        // Actualizamos el numero de ronda
        NewRoundText();

        // Viene la ronda nueva
        StartCoroutine(MoveRound(goingInPositions[0], goingInPositions[1], curve, mustNotifyAttempt: true));
    }


    IEnumerator GoOut(){
        // Sale la ronda actual
        StartCoroutine(MoveRound(goingOutPositions[0], goingOutPositions[1], curve));

        yield return 0;
    }


    IEnumerator GoIn(){
        // Actualizamos el numero de ronda
        NewRoundText();

        // Viene la ronda nueva
        StartCoroutine(MoveRound(goingInPositions[0], goingInPositions[1], curve, mustNotifyAttempt: true));
        yield return 0; 
    } 
}


