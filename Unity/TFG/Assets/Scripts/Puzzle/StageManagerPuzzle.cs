using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StageManagerPuzzle : MonoBehaviour
{
    [Header("Views:")]
    [SerializeField] private GameObject fadeCircleViewEasy;
    [SerializeField] private GameObject fadeCircleViewMedium;
    [SerializeField] private GameObject fadeCircleViewHard;

    [Header("Game Objects:")]
    [SerializeField] private GameObject backgroundPuzzle;
    [SerializeField] private GameObject sampleImage;
    [SerializeField] private GameObject easyPuzzle;
    [SerializeField] private GameObject mediumPuzzle;
    [SerializeField] private GameObject hardPuzzle;
    [SerializeField] private GameObject confettiParticles;


    [Header("Sprites:")]
    [SerializeField] private Sprite easyBackground;
    [SerializeField] private Sprite mediumBackground;
    [SerializeField] private Sprite hardBackground;
    [SerializeField] private Sprite sampleEasy;
    [SerializeField] private Sprite sampleMedium;
    [SerializeField] private Sprite sampleHard;
    
    [Header("Text:")]
    [SerializeField] private TextMeshProUGUI counterText;
    [Header("Variables:")]
    public bool movingSomePiece;        // Debe ser publica porque PieceCheck accede a ella
    private int counter;
    public int transpositions;          // Debe ser publica porque PuzzleCheck accede a ella
    





    public delegate void _OnFadeToPlay(GameObject fadeCircleView);
    public static event _OnFadeToPlay OnFadeToPlay;

    public delegate void _OnReturnToMenu();          
    public static event _OnReturnToMenu OnReturnToMenu;


    void OnEnable()
    {
        PieceCheck.OnMovingSomePiece += HandleOnMovingSomePiece;
        PieceCheck.OnStartingMovement += HandleOnStartingMovement;
        PuzzleCheck.OnGotIt += HandleOnGotIt;
        
    }


    void OnDisable()
    {
        PieceCheck.OnMovingSomePiece -= HandleOnMovingSomePiece;
        PieceCheck.OnStartingMovement -= HandleOnStartingMovement;
        PuzzleCheck.OnGotIt -= HandleOnGotIt;
        
    }


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

        // Esperamos y salimos al menu principal
        StartCoroutine(ReturnToMenu());


    }




    void Awake()
    {
        // Inicializamos los objetos de la escena como deben estar
        fadeCircleViewEasy.SetActive(false);
        fadeCircleViewMedium.SetActive(false);
        fadeCircleViewHard.SetActive(false);
        easyPuzzle.SetActive(false);
        mediumPuzzle.SetActive(false);
        hardPuzzle.SetActive(false);

        // Inicializamos las variables
        movingSomePiece = false;
        counter = 0;
        counterText.text = counter.ToString();


    }

    void Update()
    {
        
    }

    // Las funciones de respuesta a los botones deben ser publicas para que aparezcan en el inspector
    public void OnEasyButton(){
        StartLevelGame(easyPuzzle, easyBackground, sampleEasy, fadeCircleViewEasy);
        mediumPuzzle.SetActive(false);
        hardPuzzle.SetActive(false);
        transpositions = 8;     // Son 9 piezas

    }

    public void OnMediumButton(){
        StartLevelGame(mediumPuzzle, mediumBackground, sampleMedium, fadeCircleViewMedium);
        easyPuzzle.SetActive(false);
        hardPuzzle.SetActive(false);
        transpositions = 14;     // Son 16 piezas
    }


    public void OnHardButton(){
        StartLevelGame(hardPuzzle, hardBackground, sampleHard, fadeCircleViewHard);
        easyPuzzle.SetActive(false);
        mediumPuzzle.SetActive(false);   
        transpositions = 24;     // Son 25 piezas     
    }



    void StartLevelGame(GameObject puzzle, Sprite background, Sprite sample, GameObject fadeCircleView){


        // Cambiamos el sprite del fondo de estrellas y la imagen de muestra
        backgroundPuzzle.GetComponent<SpriteRenderer>().sprite = background;
        sampleImage.GetComponent<Image>().sprite = sample;
    
        // Activamos el game object del puzzle
        puzzle.SetActive(true);

        // Lanzamos el evento para que el canvas cambie de pantalla y tambien se cambie la musica de fondo
        if(OnFadeToPlay != null)                          
            OnFadeToPlay(fadeCircleView);
    }





    IEnumerator ReturnToMenu(){
        yield return new WaitForSeconds(6.5f);
        
        // Evento para que Load Scene vuelva a la scena del menu principal
        if(OnReturnToMenu != null)  
            OnReturnToMenu();  
    }

}
