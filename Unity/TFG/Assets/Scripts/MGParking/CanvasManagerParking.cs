using UnityEngine;
using System.Collections;
using TMPro;


public class CanvasManagerParking : MonoBehaviour
{
    [Header("Game Objects:")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject operationImage;
    [SerializeField] private GameObject errorImage;
    [SerializeField] private GameObject poolingRoad;

    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject victoryView;
    [SerializeField] private GameObject fadeCircleView;

    [Header("Texts:")]
    [SerializeField] private TextMeshProUGUI operationFirstTryText;
    [SerializeField] private TextMeshProUGUI operationSecondTryText;

    //[Header("Variables:")]
    private int errorCount= 0;
    private bool spacePressed;



    public delegate void _OnPlay();
    public static event _OnPlay OnPlay;

    public delegate void _OnGotIt();
    public static event _OnGotIt OnGotIt;

    public delegate void _OnReturnToMenu();
    public static event _OnReturnToMenu OnReturnToMenu;



    void Awake()
    {   
        // Desactivamos el Script de Player para que no se pueda mover mientras esta el tutorial
        player.GetComponent<CarMovement>().enabled = false;
        player.GetComponent<AudioSource>().enabled = false;

        // Vistas activadas
        tutorialView.SetActive(true);
        operationFirstTryText.gameObject.SetActive(true);


        // Vistas desactivadas
        ingameView.SetActive(false);
        victoryView.SetActive(false);
        fadeCircleView.SetActive(false);
        operationSecondTryText.gameObject.SetActive(false);

        // Inicializacion de variables
        spacePressed = false;
    }

    void OnEnable()
    {
        ParkingTrigger.OnWellParked  += HandleOnWellParked;
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
    }

    void OnDisable()
    {
        ParkingTrigger.OnWellParked  -= HandleOnWellParked;
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
    }

    void Update()
    {
        // Si la ventana de tutorial esta activada y pulsan espacio damos paso al inicio del juego (solo escuchamos el primer pulsado, para que no se retipa el lanzamiento del evento)
        if(tutorialView.activeSelf && Input.GetKeyDown(KeyCode.Space) && !spacePressed){
            spacePressed = true;
            StartCoroutine(StartGame());
        }
    }



    void HandleOnWrongParked(GameObject go){
        errorCount += 1;
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



    IEnumerator StartGame(){
        // Fade Out
        fadeCircleView.SetActive(true);
        yield return new WaitForSeconds(1.5f); // El fade out/in del CircleStatic dura 1,5 seg

        // Desactivamos la vista del tutorial y activamos la de juego
        tutorialView.SetActive(false);
        ingameView.SetActive(true);
        
        // Fade In
        fadeCircleView.GetComponent<Animator>().SetTrigger("FadeInCircleParking");

        //Las lineas precedentes se han escrito en lugar de las siguientes, para hacer una transicion mas bonita con el circulo
        //StartCoroutine(FadeCanvasGroup(tutorialView, fromAlpha: 1, toAlpha: 0));
        //StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1));

        // Avisamos de que empieza el juego
        if(OnPlay != null)                          
            OnPlay();

        // Activamos el Script de Player para que vuelva a poder moverse, el sonido del motor en marcha y activamos el Pool de objetos para que aparezcan los vehículos cruzando la calle
        player.GetComponent<CarMovement>().enabled = true;
        player.GetComponent<AudioSource>().enabled = true;

        //Activamos el pool de objetos para que pasen los vehiculos por la carretera
        poolingRoad.SetActive(true);
    }

    IEnumerator FadeCanvasGroup(GameObject view, float fromAlpha, float toAlpha, float animationTime = 0.3f){ 
        CanvasGroup canvasGroup = view.GetComponent<CanvasGroup>();
        // En funcion del alpha de la vista deducimos si quiere activarla o desactivarla
        if(toAlpha > 0)
            view.SetActive(true);

        // Cuerpo de la corrutina: modificamos el alpha con Math.Lerp interpolando el alpha
        float elapsedTime = 0;
        while(elapsedTime <= animationTime){
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / animationTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return 0;
        }
        // Establecemos el valor final deseado para que no haya decimales infinitos
        canvasGroup.alpha = toAlpha;

        // Desactivamos la vista si lo que queriamos era desactivar, es decir, hemos dejado el alpha en 0 con la corrutina
        if(toAlpha == 0)
            view.SetActive(false);
    }

    IEnumerator ShowErrorImage(float seconds = 2){ 
        // Desactivamos el Script de Player para que no se pueda mover mientras dure la corrutina 
        player.GetComponent<AudioSource>().enabled = false;
        player.GetComponent<CarMovement>().enabled = false;

        // Mostramos el panel de error  ("Repasa la operacion")
        float animationsTime = seconds / 4f;
        StartCoroutine(FadeCanvasGroup(errorImage, fromAlpha: 0, toAlpha: 1, animationTime: animationsTime));

        // Actualizacion de la operacion si es el segundo fallo
        if(errorCount == 2){
            // Esperamos la mitad del tiempo y hacemos desaparecer la operacion
            yield return new WaitForSeconds(seconds / 2);
            StartCoroutine(FadeCanvasGroup(operationImage, fromAlpha: 1, toAlpha: 0, animationTime: animationsTime));

            // Esperamos el tiempo que tarda la animacion en desaparecer y un pelin extra, para que no haya un epsilon de diferencia y eviar el error de que no aparezca despues
            yield return new WaitForSeconds(animationsTime + 0.01f);

            // Actualizamos la operacion
            operationFirstTryText.gameObject.SetActive(false);
            operationSecondTryText.gameObject.SetActive(true);

            // Hacemos aparecer la imagen de la operacion
            StartCoroutine(FadeCanvasGroup(operationImage, fromAlpha: 0, toAlpha: 1, animationTime: animationsTime));

            // Tiempo transcurrido = seconds/2 + seconds/4. Luego falta por transcurrir seconds/4 = animationsTime
            yield return new WaitForSeconds(animationsTime);
        }
        else{
            yield return new WaitForSeconds(seconds);
        }

        StartCoroutine(FadeCanvasGroup(errorImage, fromAlpha: 1, toAlpha: 0, animationTime: animationsTime));

        // Activamos el Script de Player para que vuelva a poder moverse
        player.GetComponent<CarMovement>().enabled = true;
        player.GetComponent<AudioSource>().enabled = true;
    }

    IEnumerator ReturnToMenu(){
        yield return new WaitForSeconds(4.5f);
        
        // Evento para que Load Scene vuelva a la scena del menu principal
        if(OnReturnToMenu != null)  
            OnReturnToMenu();  
    }

}