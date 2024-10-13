using UnityEngine;
using TMPro;
using System.Collections;



public class AttemptMovement : MonoBehaviour
{

    private TextMeshProUGUI attemptPlace;
    [SerializeField] private GameObject stageManager;

    private int maxAttempts;
    private int attemptsNumber;

    public delegate void _OnPlaying();
    public static event _OnPlaying OnPlaying;




    void OnEnable()
    {

        attemptPlace = gameObject.GetComponent<TextMeshProUGUI>();

        ShowAttempt();


        // Eventos:
        StageManagerDeduceSign.OnCorrectAnswer += HandleOnCorrectAnswer;
        StageManagerDeduceSign.OnWrongAnswer += HandleOnWrongAnswer;
        

        // Movimiento con corrutina en vez de animaciones
        //StartCoroutine(TransformSizeFont(startSize: 0, endSize: 80.4f, animationTime: 1));

    }

    void OnDisable(){
        StageManagerDeduceSign.OnCorrectAnswer -= HandleOnCorrectAnswer;
        StageManagerDeduceSign.OnWrongAnswer -= HandleOnWrongAnswer;

    }






    private void ShowAttempt(){

        StartCoroutine(WaitAndMoveAttempts());

    }


    private void HandleOnWrongAnswer(){
        attemptsNumber --;

        UpgradeTextAttempt();
        

    }

    private void HandleOnCorrectAnswer(){
        
        StartCoroutine(RestartAttempt());
        attemptPlace.GetComponent<Animator>().SetTrigger("FadeOut");

    }


    void Awake(){
        int totalRounds = stageManager.GetComponent<StageManagerDeduceSign>().totalRounds;

        // El numero de intentos varia en funcion de las rondas totales que haya. Si hay mas de 4 rondas, los intentos pueden ser su maximo que es 4 (por los 4 simbolos de operacion). Si hay menos, los intentos van con la ronda
        if(totalRounds > 4)
            maxAttempts = 4;
        else
            maxAttempts = totalRounds;
            
        attemptsNumber = maxAttempts;

        UpgradeTextAttempt();
    }


    private void UpgradeTextAttempt(){
        gameObject.GetComponent<TextMeshProUGUI>().text = "Intentos:\n" + attemptsNumber.ToString();
    }


    IEnumerator RestartAttempt(){
        // Hacemos el fade out del attept y lo colocamos en su posicion inicial con Restart, para prepararlo para la siguiente ronda
        attemptPlace.GetComponent<Animator>().SetTrigger("FadeOut");
        
        yield return new WaitForSeconds(0.49f); // Tiempo de transicion de la animacion FadeOut (0.29 seg), para que con el restar sea 0.30 seg

        // Actualizamos los intentos disponibles en esta ronda

        maxAttempts --;
        attemptsNumber = maxAttempts;

        attemptPlace.GetComponent<Animator>().SetTrigger("Restart");

    }





    IEnumerator WaitAndMoveAttempts(){
        yield return new WaitForSeconds(1.5f);

        // Cogemos el componente animator y lanzamos el trigger para que cambie de animacion
        attemptPlace.GetComponent<Animator>().SetTrigger("MoveAttempt");

        yield return new WaitForSeconds(0.5f);

        // Lanzamos el evento para que se active la operacion y los botones
        if(OnPlaying != null)   
            OnPlaying();
    }








    // Movimiento del texto
    // IEnumerator MovementText(TextMeshPro text, float animationTime){

    //     float elapsedTime = 0;

    //     Vector3 startPosition = transform.localPosition;
    //     Vector3 endPosition = transform.localPosition;

    //     startPosition.x = -650;
    //     endPosition.x =  0;

    //     while(elapsedTime < animationTime){

    //         transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

    //         elapsedTime += Time.deltaTime;
    //         yield return 0;
    //     }
    //     transform.localPosition = endPosition;

    // }

    // Agrandar el texto
    // IEnumerator TransformSizeFont(float startSize, float endSize, float animationTime){
    //     // Corrutina reutilizada de MGLaneRace

    //     float elapsedTime = 0;

    //     while(elapsedTime < animationTime){
    //         float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
    //         attempPlace.fontSize = newSize;
    //         elapsedTime += Time.deltaTime;
    //         yield return 0;
    //     }
    // }

    
}
