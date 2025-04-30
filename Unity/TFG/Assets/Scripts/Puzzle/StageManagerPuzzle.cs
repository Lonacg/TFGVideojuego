using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;



public class StageManagerPuzzle : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject fadeCircleViewEasy;
    [SerializeField] private GameObject fadeCircleViewMedium;
    [SerializeField] private GameObject fadeCircleViewHard;

    [Header("Game Objects:")]
    [SerializeField] private GameObject backgroundPuzzle;
    [SerializeField] private GameObject samplePuzzle;
    [SerializeField] private GameObject finalPuzzle;
    [SerializeField] private GameObject easyPuzzle;
    [SerializeField] private GameObject mediumPuzzle;
    [SerializeField] private GameObject hardPuzzle;
    [SerializeField] private GameObject handStamp;
    [SerializeField] private GameObject confettiParticles;
    [SerializeField] private GameObject fireworks;

    [Header("Sprites:")]
    [SerializeField] private Sprite easyBackground;
    [SerializeField] private Sprite mediumBackground;
    [SerializeField] private Sprite hardBackground;
    [SerializeField] private Sprite sampleEasy;
    [SerializeField] private Sprite sampleMedium;
    [SerializeField] private Sprite sampleHard;
    [SerializeField] private Sprite finalPuzzleEasy;
    [SerializeField] private Sprite finalPuzzleMedium;
    [SerializeField] private Sprite finalPuzzleHard;
    
    [Header("Text:")]
    [SerializeField] private TextMeshProUGUI counterText;

    [Header("Variables:")]
    public bool movingSomePiece;        // Debe ser publica porque PieceCheck accede a ella
    public int transpositions;          // Debe ser publica porque PuzzleCheck accede a ella
    private int counter;
    private bool spacePressed = false;



    // DECLARACIÓN DE EVENTOS
    public delegate void _OnFadeToLevels();
    public static event _OnFadeToLevels OnFadeToLevels;

    public delegate void _OnFadeToPlay(GameObject fadeCircleView);
    public static event _OnFadeToPlay OnFadeToPlay;

    public delegate void _OnReturnToMenu();          
    public static event _OnReturnToMenu OnReturnToMenu;

    public delegate void _OnLastShine();          
    public static event _OnLastShine OnLastShine;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Awake()
    {
        // Accedemos al singleton para comunicar que se ha iniciado este minijuego
        GameChecker.Instance.PuzzleOnPlay();
        
        InitializeAll();
    }

    void OnEnable()
    {
        PieceCheck.OnMovingSomePiece  += HandleOnMovingSomePiece;
        PieceCheck.OnStartingMovement += HandleOnStartingMovement;
        PuzzleCheck.OnGotIt           += HandleOnGotIt;
    }

    void OnDisable()
    {
        PieceCheck.OnMovingSomePiece  -= HandleOnMovingSomePiece;
        PieceCheck.OnStartingMovement -= HandleOnStartingMovement;
        PuzzleCheck.OnGotIt           -= HandleOnGotIt;
    }

    void Update()
    {
        // Si la ventana de tutorial esta activada y pulsan espacio damos paso al inicio del juego (solo escuchamos el primer pulsado, para que no se retipa el lanzamiento del evento)
        if(tutorialView.activeSelf && Input.GetKeyDown(KeyCode.Space) && !spacePressed){
            spacePressed = true;
            if(OnFadeToLevels != null)   
                OnFadeToLevels();
        }
    }    



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnMovingSomePiece(){
        // Cambiamos el bool al que acceden el resto de piezas para impedir o permitir su movimiento
        movingSomePiece = !movingSomePiece;
    }

    private void HandleOnStartingMovement(){
        // Aumentamos en uno el contador y actualizamos el valor en la escena
        counter += 1;
        counterText.text = counter.ToString();
    }

    private void HandleOnGotIt(){
        // Bloqueamos el movimiento y lanzamos los confeti
        movingSomePiece =true;
        confettiParticles.SetActive(true); 

        StartCoroutine(WaitAndActiveHand());

        // Esperamos y salimos al menu principal
        StartCoroutine(ReturnToMenu());
    }



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    private void InitializeAll(){
        // Inicializamos los objetos de la escena como deben estar
        fadeCircleViewEasy.SetActive(false);
        fadeCircleViewMedium.SetActive(false);
        fadeCircleViewHard.SetActive(false);
        easyPuzzle.SetActive(false);
        mediumPuzzle.SetActive(false);
        hardPuzzle.SetActive(false);
        handStamp.SetActive(true);
        confettiParticles.SetActive(false);
        fireworks.SetActive(false);

        // Inicializamos las variables
        movingSomePiece = false;
        counter = 0;
        counterText.text = counter.ToString();
        spacePressed = false;
    }

    void StartLevelGame(GameObject puzzle, Sprite background, Sprite sample, Sprite finalPuzzleSprite, GameObject fadeCircleView){
        // Cambiamos el sprite del fondo de estrellas,la imagen de muestra y la imagen del final del puzzle resuelto
        backgroundPuzzle.GetComponent<SpriteRenderer>().sprite = background;
        samplePuzzle.GetComponent<Image>().sprite = sample;
        finalPuzzle.GetComponent<SpriteRenderer>().sprite = finalPuzzleSprite;
    
        // Activamos el game object del puzzle
        puzzle.SetActive(true);

        // Lanzamos el evento para que el canvas cambie de pantalla y tambien se cambie la musica de fondo
        if(OnFadeToPlay != null)                          
            OnFadeToPlay(fadeCircleView);
    }

    public void OnEasyButton(){
        // Funcion en respuesta al boton del canvas, se ejecuta con un evento interno asi que debe ser publica
        StartLevelGame(easyPuzzle, easyBackground, sampleEasy, finalPuzzleEasy,fadeCircleViewEasy);
        mediumPuzzle.SetActive(false);
        hardPuzzle.SetActive(false);
        transpositions = 8;     // Son 9 piezas
    }

    public void OnMediumButton(){
        // Funcion en respuesta al boton del canvas, se ejecuta con un evento interno asi que debe ser publica
        StartLevelGame(mediumPuzzle, mediumBackground, sampleMedium, finalPuzzleMedium, fadeCircleViewMedium);
        easyPuzzle.SetActive(false);
        hardPuzzle.SetActive(false);
        transpositions = 14;     // Son 16 piezas
    }

    public void OnHardButton(){
        // Funcion en respuesta al boton del canvas, se ejecuta con un evento interno asi que debe ser publica
        StartLevelGame(hardPuzzle, hardBackground, sampleHard, finalPuzzleHard, fadeCircleViewHard);
        easyPuzzle.SetActive(false);
        mediumPuzzle.SetActive(false);   
        transpositions = 24;     // Son 25 piezas     
    }



    // CORRUTINAS
    IEnumerator WaitAndActiveHand(){
        yield return new WaitForSeconds(1f);
        
        handStamp.GetComponent<Animator>().SetTrigger("HandIn");
    }

    IEnumerator ReturnToMenu(){
        // Espera de 6.5 seg en total antes de salir
        yield return new WaitForSeconds(4f);

        if(OnLastShine != null)                          
            OnLastShine();        
        StartCoroutine(FadeInImage(finalPuzzle));
        fireworks.SetActive(true);
        
        yield return new WaitForSeconds(3.5f);
        
        // Evento para que Load Scene vuelva a la scena del menu principal
        if(OnReturnToMenu != null)  
            OnReturnToMenu();  
    }

    IEnumerator FadeInImage(GameObject finalPuzzle){
        finalPuzzle.SetActive(true);
        Color newColor = finalPuzzle.GetComponent<SpriteRenderer>().color;

        float elapsedTime = 0;
        float animationTime = 1f;
        
        while(elapsedTime <= animationTime){
            newColor.a = Mathf.Lerp(0, 1, elapsedTime / animationTime);

            finalPuzzle.GetComponent<SpriteRenderer>().color = newColor;

            elapsedTime += Time.unscaledDeltaTime;
            yield return 0;
        }
        newColor.a = 1;
        finalPuzzle.GetComponent<SpriteRenderer>().color = newColor;
    }

}
