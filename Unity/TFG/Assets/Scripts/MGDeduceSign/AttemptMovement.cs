using UnityEngine;
using TMPro;
using System.Collections;



public class AttemptMovement : MonoBehaviour
{

    [SerializeField] private GameObject stageManager;

    public int maxAttempts;
    public int attemptsNumber;

    public delegate void _OnPlaying();
    public static event _OnPlaying OnPlaying;




    void OnEnable()
    {


        ShowAttempt();


        // Eventos:
        StageManagerDeduceSign.OnNewRound += HandleOnNewRound;
        StageManagerDeduceSign.OnWrongAnswer += HandleOnWrongAnswer;
        StageManagerDeduceSign.OnHasWin += HandleOnHasWin;
        
        

        // Movimiento con corrutina en vez de animaciones
        //StartCoroutine(TransformSizeFont(startSize: 0, endSize: 80.4f, animationTime: 1));

    }

    void OnDisable(){
        StageManagerDeduceSign.OnNewRound -= HandleOnNewRound;
        StageManagerDeduceSign.OnWrongAnswer -= HandleOnWrongAnswer;
        StageManagerDeduceSign.OnHasWin -= HandleOnHasWin;

    }




    private void HandleOnWrongAnswer(){
        attemptsNumber --;

        UpgradeTextAttempt();
        

    }

    private void HandleOnNewRound(bool sameRound){
        
        StartCoroutine(RestartAttempt(sameRound));
        
    }



    private void HandleOnHasWin(){
        gameObject.GetComponent<Animator>().SetTrigger("FadeOut");
    }




    void Start(){
        maxAttempts = stageManager.GetComponent<StageManagerDeduceSign>().maxAttempts;
        attemptsNumber = stageManager.GetComponent<StageManagerDeduceSign>().attemptsNumber;

        UpgradeTextAttempt();
    }

    private void ShowAttempt(){

        StartCoroutine(WaitAndMoveAttempts());

    }

    private void UpgradeTextAttempt(){
        gameObject.GetComponent<TextMeshProUGUI>().text = "Intentos:\n" + attemptsNumber.ToString();
    }


    IEnumerator RestartAttempt(bool sameRound){
        // Hacemos el fade out del attempt
        gameObject.GetComponent<Animator>().SetTrigger("FadeOut");
        
        yield return new WaitForSeconds(0.49f); // Tiempo de transicion de la animacion FadeOut (0.29 seg), para que con el restar sea 0.30 seg

        // Actualizamos los intentos disponibles en la proxima ronda
        if(sameRound){
            attemptsNumber = maxAttempts;
        }
        else{
            maxAttempts --;
            attemptsNumber = maxAttempts;
        }
        
        UpgradeTextAttempt();


        // Lo colocamos en su posicion inicial para prepararlo para la siguiente ronda
        gameObject.GetComponent<Animator>().SetTrigger("Restart");

    }





    IEnumerator WaitAndMoveAttempts(){
        yield return new WaitForSeconds(1.5f); // Tiempo que muestra los intentos en grande

        // Cogemos el componente animator y lanzamos el trigger para que cambie de animacion
        gameObject.GetComponent<Animator>().SetTrigger("MoveAttempt");

        yield return new WaitForSeconds(0.5f); // Tiempo que duran las animaciones

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
