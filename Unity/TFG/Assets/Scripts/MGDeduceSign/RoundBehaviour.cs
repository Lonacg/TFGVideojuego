using System.Collections;
using TMPro;
using UnityEngine;

public class RoundBehaviour : MonoBehaviour
{


    [SerializeField] private AnimationCurve curve;
    [SerializeField] private TextMeshProUGUI attempText;
    [SerializeField] private GameObject stageManager;

    

    private int correctAnswers = 0;
    private int totalRounds;




    public delegate void _OnShowAttempt();
    public static event _OnShowAttempt OnShowAttempt;


    void OnEnable()
    {
        StageManagerDeduceSign.OnCorrectAnswer += HandleOnCorrectAnswer;

    }

    void OnDisable(){
        StageManagerDeduceSign.OnCorrectAnswer -= HandleOnCorrectAnswer;
    }

    void Start(){

        gameObject.GetComponent<TextMeshProUGUI>().text = "Ronda 1";

        // Comprobamos cual es el numero de rondas que debe superar Player
        totalRounds = stageManager.GetComponent<StageManagerDeduceSign>().totalRounds;

        // Activamos el movimiento del texto Ronda X
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition;

        startPosition.x = -650;
        endPosition.x =  0;         // No haria falta, ya que es cero, pero por facilitar la comprension
        
        StartCoroutine(MoveRound(startPosition, endPosition, mustNotifyAttempt: true));
    }


    private void HandleOnCorrectAnswer(){
        correctAnswers ++;

        if(correctAnswers < totalRounds){
            StartCoroutine(GoOutGoIn());            
        }
        else{

        }

    }


    private void NewRoundText(){

        int numberRound = correctAnswers ++;
        if(numberRound == totalRounds){
            gameObject.GetComponent<TextMeshProUGUI>().text = "¡Ronda final!";
        }
        else{
            gameObject.GetComponent<TextMeshProUGUI>().text = "Ronda " + numberRound.ToString();
        }


    }


    IEnumerator MoveRound(Vector3 startPosition, Vector3 endPosition, bool mustNotifyAttempt = false, float animationTime = 1){

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
        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition;

        // Se va el texto de la ronda hecha
        startPosition.x = 0;        // No haria falta, ya que es cero, pero por facilitar la comprension
        endPosition.x = 650;

        StartCoroutine(MoveRound(startPosition, endPosition));

        yield return new WaitForSeconds(1);

        // Actualizamos el numero de ronda
        NewRoundText();


        // Viene el texto de la ronda nueva
        startPosition.x = -650;        
        endPosition.x = 0;

        StartCoroutine(MoveRound(startPosition, endPosition));

    }



}


