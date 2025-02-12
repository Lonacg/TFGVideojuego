using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CanvasManagerParking : MonoBehaviour
{
    [Header("Game Objects:")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject victoryView;
    [SerializeField] private GameObject fadeCircle;
    [SerializeField] private GameObject operationImage;
    [SerializeField] private GameObject errorImage;
    [SerializeField] private GameObject poolingRoad;


    [Header("Texts:")]
    [SerializeField] private TextMeshProUGUI operationFirstTryText;
    [SerializeField] private TextMeshProUGUI operationSecondTryText;

    //[Header("Variables:")]
    private bool firstTry= true;
    private bool spacePressed;



    public delegate void _OnPlay();
    public static event _OnPlay OnPlay;

    public delegate void _OnGotIt();
    public static event _OnGotIt OnGotIt;


    public delegate void _OnReturnToMenu();
    public static event _OnReturnToMenu OnReturnToMenu;


    void OnEnable(){
        ParkingTrigger.OnWellParked += HandleOnWellParked;
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
    }

    void OnDisable(){
        ParkingTrigger.OnWellParked -= HandleOnWellParked;
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
    }



    void HandleOnWrongParked(GameObject go){
        StartCoroutine(ShowErrorImage());
    }

    void HandleOnWellParked(GameObject go){
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 1, toAlpha: 0));
        StartCoroutine(FadeCanvasGroup(victoryView, fromAlpha: 0, toAlpha: 1));
        if(OnGotIt != null)  // Lanzamos el evento de Conseguido para que se reproduzca el sonido
            OnGotIt();
        player.GetComponent<AudioSource>().enabled = false;
        player.GetComponent<CarMovement>().enabled = false;

        // Esperamos dos segundos para que se vean los confeti y volvemos al menu principal
        StartCoroutine(ReturnToMenu());
    }




    void Awake()
    {   
        // Vistas activadas
        tutorialView.SetActive(true);

        // Vistas desactivadas
        ingameView.SetActive(false);
        victoryView.SetActive(false);
        fadeCircle.SetActive(false);


        // Inicializacion de variables
        spacePressed = false;
    }


    void Update(){
        // Si la ventana de tutorial esta activada y pulsan espacio damos paso al inicio del juego (solo escuchamos el primer pulsado, para que no se retipa el lanzamiento del evento)
        if(tutorialView.activeSelf && Input.GetKeyDown(KeyCode.Space) && !spacePressed){
            spacePressed = true;
            StartCoroutine(StartGame());
        }
    }



    IEnumerator StartGame(){
        // Fade Out
        fadeCircle.SetActive(true);
        yield return new WaitForSeconds(1.5f); // El fade out/in del CircleStatic dura 1,5 seg

        // Desactivamos la vista del tutorial y activamos la de juego
        tutorialView.SetActive(false);
        ingameView.SetActive(true);
        
        // Fade In
        fadeCircle.GetComponent<Animator>().SetTrigger("FadeInCircleParking");

        //Las lineas precedentes se han escrito en lugar de las siguientes, para hacer una transicion mas bonita con el circulo
        //StartCoroutine(FadeCanvasGroup(tutorialView, fromAlpha: 1, toAlpha: 0));
        //StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1));


        // // Avisamos de que empieza el juego
        if(OnPlay != null)                          
            OnPlay();

        // Activamos el Script de Player para que vuelva a poder moverse, el sonido del motor en marcha y activamos el Pool de objetos para que aparezcan los vehículos cruzando la calle
        player.GetComponent<CarMovement>().enabled = true;
        player.GetComponent<AudioSource>().enabled = true;

        poolingRoad.SetActive(true);
    }



    IEnumerator FadeCanvasGroup(GameObject view, float fromAlpha, float toAlpha, float animationTime = 0.3f){ 
        CanvasGroup canvasGroup = view.GetComponent<CanvasGroup>();
        if(toAlpha > 0)
            view.SetActive(true);

        float elapsedTime = 0;

        while(elapsedTime <= animationTime){
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / animationTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return 0;
        }

        canvasGroup.alpha = toAlpha;

        if(toAlpha == 0)
            view.SetActive(false);
    }

    IEnumerator ShowErrorImage(float seconds = 2){ 
        // Desactivamos el Script de Player para que no se pueda mover mientras dure la corrutina 
        player.GetComponent<AudioSource>().enabled = false;
        player.GetComponent<CarMovement>().enabled = false;

        // Mostramos el panel de error  ("Repasa la operacion")
        float animationsTime = 0.6f;
        StartCoroutine(FadeCanvasGroup(errorImage, fromAlpha: 0, toAlpha: 1, animationTime: animationsTime));

        // Actualizacion de la operacion si es el primer fallo
        if(firstTry){
            yield return new WaitForSeconds(1);
            StartCoroutine(FadeCanvasGroup(operationImage, fromAlpha: 1, toAlpha: 0, animationTime: animationsTime));
            yield return new WaitForSeconds(animationsTime);
            operationFirstTryText.gameObject.SetActive(false);
            operationSecondTryText.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(operationImage, fromAlpha: 0, toAlpha: 1, animationTime: animationsTime));
            firstTry = false;
        }

        yield return new WaitForSeconds(seconds);
        StartCoroutine(FadeCanvasGroup(errorImage, fromAlpha: 1, toAlpha: 0, animationTime: animationsTime));

        // Activamos el Script de Player para que vuelva a poder moverse
        player.GetComponent<CarMovement>().enabled = true;
        player.GetComponent<AudioSource>().enabled = true;
    }


    IEnumerator ReturnToMenu(){
        yield return new WaitForSeconds(6);
        
        // Evento para que Load Scene vuelva a la scena del menu principal
        if(OnReturnToMenu != null)  
            OnReturnToMenu();
        
        
    }

}
