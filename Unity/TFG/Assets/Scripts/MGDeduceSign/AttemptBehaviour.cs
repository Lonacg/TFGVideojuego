using UnityEngine;
using TMPro;
using System.Collections;



public class AttemptBehaviour : MonoBehaviour
{
    [Header("Game Objects:")]
    [SerializeField] private GameObject stageManager;
    [SerializeField] private GameObject flashAtemptParticles;

    private int currentAttemp;



    public delegate void _OnPlaying();
    public static event _OnPlaying OnPlaying;



    void OnEnable()
    {
        ShowAttempt();

        // Eventos:
        StageManagerDeduceSign.OnNewRound += HandleOnNewRound;
        StageManagerDeduceSign.OnWrongAnswer += HandleOnWrongAnswer;
        StageManagerDeduceSign.OnFadeOutAll += HandleOnFadeOutAll;
    }

    void OnDisable(){
        StageManagerDeduceSign.OnNewRound -= HandleOnNewRound;
        StageManagerDeduceSign.OnWrongAnswer -= HandleOnWrongAnswer;
        StageManagerDeduceSign.OnFadeOutAll -= HandleOnFadeOutAll;
    }



    private void HandleOnWrongAnswer(){
        UpgradeTextAttempt(wantFlash: true);
    }


    private void HandleOnNewRound(bool sameRound){

        StartCoroutine(RestartAttempt(sameRound));        
    }


    private void HandleOnFadeOutAll(){
        gameObject.GetComponent<Animator>().SetTrigger("FadeOut");
    }



    void Start(){
        UpgradeTextAttempt();
    }



    private void ShowAttempt(){

        StartCoroutine(WaitAndMoveAttempts());
    }


    private void UpgradeTextAttempt(bool wantFlash = false){
        currentAttemp = stageManager.GetComponent<StageManagerDeduceSign>().attemptsNumber;
        if(wantFlash){
            StartCoroutine(ShowFlashAttempt());       // El verde es el primer hijo y el rojo el segundo
        }
        gameObject.GetComponent<TextMeshProUGUI>().text = "Intentos:\n" + currentAttemp.ToString();
    }

    IEnumerator ShowFlashAttempt(){
        flashAtemptParticles.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        flashAtemptParticles.SetActive(false);

    }


    IEnumerator RestartAttempt(bool sameRound){

        if(!sameRound){
            // Hacemos el fade out del attempt
            gameObject.GetComponent<Animator>().SetTrigger("FadeOut");
            
            yield return new WaitForSeconds(1f); // Tiempo de transicion para que no se vea por un momento los intentos de la siguiente ronda hasta que no se oculten del todo
        }
        
        // Actualizamos los intentos disponibles en la proxima ronda
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



    // ALTERNATIVA A LA ANIMACION (solo fadeIn, fadeOut y movimiento, sin cambio de color)

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
