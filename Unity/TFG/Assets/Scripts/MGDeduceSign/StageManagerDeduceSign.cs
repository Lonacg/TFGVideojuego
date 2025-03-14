using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class StageManagerDeduceSign : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Variables:")]
    [SerializeField, Min(2)] public int totalRounds = 3;    // Necesita ser publica para que RoundBehaviour acceda a ella
    public int maxAttempts;                                 // Necesita ser publica para que AttemptBehaviour acceda a ella
    public int attemptsNumber;                              // Necesita ser publica para que AttemptBehaviour acceda a ella
    public int currentRound;                                // Necesita ser publica para que RoundBehaviour acceda a ella
    private string answerSign;
    private bool firstOperation;
    private float animationsTime = 0.5f;

    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    private bool spacePressed = false;

    [Header("Text References:")]
    [SerializeField] private TextMeshPro firsNumberText;
    [SerializeField] private TextMeshPro secondNumberText;
    [SerializeField] private TextMeshPro resultNumberText;
    [SerializeField] private TextMeshProUGUI attemptPlace;

    [Header("GameObjects:")]     
    [SerializeField] private GameObject buttonsParent;     
    [SerializeField] private GameObject operationParent; 
    [SerializeField] private GameObject errorSheet; 
    [SerializeField] private GameObject confetyParticles;
    public List<GameObject> buttonsChosen;                  // Necesita ser publica para la correcta gestion de los botones
    


    // DECLARACIÓN DE EVENTOS
    public delegate void _OnFadeToPlay();
    public static event _OnFadeToPlay OnFadeToPlay;

    public delegate void _OnChangeBoolCanChoose();
    public static event _OnChangeBoolCanChoose OnChangeBoolCanChoose;

    public delegate void _OnCorrectAnswer();
    public static event _OnCorrectAnswer OnCorrectAnswer;
    public delegate void _OnWrongAnswer();
    public static event _OnWrongAnswer OnWrongAnswer;

    public delegate void _OnFailedRoundSFX();
    public static event _OnFailedRoundSFX OnFailedRoundSFX;

    public delegate void _OnNewRound(bool sameRound);
    public static event _OnNewRound OnNewRound;

    public delegate void _OnFadeOutAll();
    public static event _OnFadeOutAll OnFadeOutAll;

    public delegate void _OnGotIt();
    public static event _OnGotIt OnGotIt;

    public delegate void _OnReturnToMenu();          
    public static event _OnReturnToMenu OnReturnToMenu;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Awake()
    {
        currentRound = 1;
        buttonsParent.SetActive(false);
        operationParent.SetActive(false);
        attemptPlace.gameObject.SetActive(false);

        SetAttemptsNumber();
    }

    void OnEnable()
    {
        ButtonBehaviour.OnSignChosen += HandleOnSignChosen;
        RoundBehaviour.OnShowAttempt += HandleOnShowAttempt;
        AttemptBehaviour.OnPlaying += HandleOnPlaying;
    }

    void OnDisable()
    {
        ButtonBehaviour.OnSignChosen -= HandleOnSignChosen;
        RoundBehaviour.OnShowAttempt -= HandleOnShowAttempt;
        AttemptBehaviour.OnPlaying -= HandleOnPlaying;
    }

    void Start()
    {
        animationsTime = 0.5f;
        firstOperation = true;
        //totalRounds = 3;          // Elegible en el inspector de Unity al ser SerializeField
        
        // Inicializamos la operacion en "invisible" (escala 0 en y) para que al activarlo no se vea
        operationParent.transform.localScale = new Vector3(1, 0, 1);
    }

    void Update()
    {
        // Si la ventana de tutorial esta activada y pulsan espacio damos paso al inicio del juego (solo escuchamos el primer pulsado, para que no se retipa el lanzamiento del evento)
        if(tutorialView.activeSelf && Input.GetKeyDown(KeyCode.Space) && !spacePressed){
            spacePressed = true;
            if(OnFadeToPlay != null)   
                OnFadeToPlay();
        }
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnSignChosen(GameObject buttonChosenGO){

        // Metemos el boton en una lista para luego volver a activarles el script
        buttonsChosen.Add(buttonChosenGO.gameObject); // Aunque goSing sea un game object, cuando se mete a la lista tenemos que decir que queremos meterlo como .gameObject porque si no nos da null reference
        
        // Avisamos a los botones para que cambien a false
        if(OnChangeBoolCanChoose != null){
            OnChangeBoolCanChoose();
        }

        // Comprobamos si el simbolo escogido es el correcto
        CheckAnswer(buttonChosenGO);
    }
    
    private void HandleOnShowAttempt(){
        attemptPlace.gameObject.SetActive(false);
        attemptPlace.gameObject.SetActive(true);
    }

    private void HandleOnPlaying(){
        buttonsParent.SetActive(false);
        buttonsParent.SetActive(true);

        operationParent.SetActive(false);
        operationParent.SetActive(true);
        StartCoroutine(FadeInOperation(animationsTime));
    }



    // MÉTODOS DE ESTA CLASE
    public void UpdateNumbers(){  

        // Buscamos los nuevos numeros generados en el script SetOperation
        SetOperationDeduceSign scriptSetOperation = operationParent.GetComponent<SetOperationDeduceSign>();
        int firstNumber = scriptSetOperation.firstNumber;
        int secondNumber = scriptSetOperation.secondNumber;
        int resultNumber = scriptSetOperation.resultNumber;
        answerSign = scriptSetOperation.answerSign;

        // Actualizamos los textos
        firsNumberText.text = firstNumber.ToString();
        secondNumberText.text = secondNumber.ToString();
        resultNumberText.text = resultNumber.ToString();
    }

    private void SetAttemptsNumber(){
        if(totalRounds - currentRound >= 4){
            maxAttempts = 4;
        }
        else{
            maxAttempts = totalRounds + 1 - currentRound; // + 1 porque current round ya se ha sumando
        } 

        attemptsNumber = maxAttempts;
    }

    public void CheckAnswer(GameObject goSign){
        
        // Solucion correcta
        if(goSign.CompareTag(answerSign)){        // Terrible para leer: if((goSign.tag == "Addition" && answerSign == 0) || (goSign.tag == "Subtraction" && answerSign == 1) || (goSign.tag == "Multiplication" && answerSign == 2) || (goSign.tag == "Division" && answerSign == 3));

            ManageCorrectAnswer(goSign);
            
            if(OnCorrectAnswer != null){
                OnCorrectAnswer();
            }
        }
        else{ // Solucion incorrecta
            attemptsNumber --;
            ManageWrongAnswer(goSign);

            if(OnWrongAnswer != null){
                OnWrongAnswer();
            }
        }
    }

    public void ManageCorrectAnswer(GameObject goSign){

        MakeButtonGreen(goSign);

        if(currentRound == totalRounds){

            // Lanzamos el evento de victoria: canvas muestra el conseguido
            if(OnGotIt != null) 
                OnGotIt();

            // Vaciamos la pizarra
            if(OnFadeOutAll != null) 
                OnFadeOutAll();
            FadeOutButtonsAndOperation();

            // Lanzamos los confeti
            StartCoroutine(LaunchConfetti());

            // Esperamos y volvemos al menu principal de seleccion
            StartCoroutine(ReturnToMenu());
            
        }
        else{
            // Ronda e intentos de la siguiente fase
            currentRound ++;
            StartCoroutine(WaitAndChangeAttempts());
            StartCoroutine(WaitAndNewRound());
        }
    }

    public void ManageWrongAnswer(GameObject goSign){
        
        MakeButtonRed(goSign);

        if(attemptsNumber == 0){

            // Ocultamos todo lo de la pizarra
            if(OnFadeOutAll != null)
                OnFadeOutAll();
            FadeOutButtonsAndOperation();

            // Mostramos el dialogo de error
            StartCoroutine(ShowError());

            // Actualizamos el int de intentos para la siguiente ronda
            StartCoroutine(WaitAndChangeAttempts());
        }
        else{
            // Avisamos a los botones para que cambien a true
            if(OnChangeBoolCanChoose != null){
                OnChangeBoolCanChoose();
            }
        }
    } 

    public void MakeButtonGreen(GameObject goSign){
        StartCoroutine(ShowFlashButton(goSign, childNumber: 1));       // El verde es el segundo hijo (1) y el rojo el tercero
        goSign.GetComponent<ButtonBehaviour>().ChangeButtonColor(Color.green);

    }


    public void MakeButtonRed(GameObject goSign){
        // Cambiamos el boton a rojo y deshabilitamos el script
        ButtonBehaviour scriptButton = goSign.GetComponent<ButtonBehaviour>();

        Vector4 darkRed = new(0.8f, 0, 0, 1);
        StartCoroutine(ShowFlashButton(goSign, childNumber: 2));       // El verde es el segundo hijo y el rojo el tercero (2)
        scriptButton.ChangeButtonColor(darkRed);
        scriptButton.enabled = false; 
    }

    public void RestartButtons(){
        if(buttonsChosen[0] != null){
            foreach(GameObject goSign in buttonsChosen){
                // ButtonBehaviour scriptButton = goSign.GetComponent<ButtonBehaviour>(); // NO SE PUEDE PONER ESTO porque en este momento el script esta desactivado asi que lanza Null reference
                goSign.GetComponent<ButtonBehaviour>().enabled = true; 
                goSign.GetComponent<ButtonBehaviour>().ChangeButtonColor(Color.white); 
            }
            buttonsChosen = new List<GameObject>();
        }
    }

    private void FadeOutButtonsAndOperation(){
        buttonsParent.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(FadeOutOperation(animationsTime));
    }



    // CORRUTINAS
     IEnumerator ShowFlashButton(GameObject goSign, int childNumber){
        GameObject flashParticles = goSign.transform.GetChild(childNumber).gameObject;
        flashParticles.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        flashParticles.SetActive(false);
    }

    IEnumerator TransformSizeOperation(float startSize, float endSize, float animationTime){
        // Funcion reutilizada de MGLaneRace
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newScale = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            
            operationParent.transform.localScale = new Vector3(1, newScale, 1);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        operationParent.transform.localScale = new Vector3(1, endSize, 1);
    }

    IEnumerator FadeInOperation(float animationTime){

        if(firstOperation){
            firstOperation = false;
        }
        else{
            RestartButtons();
        }

        UpdateNumbers();

        StartCoroutine(TransformSizeOperation(startSize: 0, endSize: 1, animationTime: animationTime));
        yield return new WaitForSeconds(animationTime); 

        // Avisamos para que cambien a true
        if(OnChangeBoolCanChoose != null){
            OnChangeBoolCanChoose();
        }
    }

    IEnumerator FadeOutOperation(float animationTime){

        StartCoroutine(TransformSizeOperation(startSize: 1, endSize: 0, animationTime: animationTime));
        yield return 0;
    }

    IEnumerator WaitAndNewRound(){
        yield return new WaitForSeconds(animationsTime);

        if(OnNewRound != null){
            OnNewRound(sameRound: false);
        }

        FadeOutButtonsAndOperation();
    }

    IEnumerator WaitAndChangeAttempts(){
        yield return new WaitForSeconds(animationsTime);

        SetAttemptsNumber();
    }
    
    IEnumerator ShowError(){
        errorSheet.SetActive(true);

        // Las esperas en total son 3.5 seg: 1 FadeIn + 2 Stay + 1 FadeOut - 0,5 de solape de animaciones ()
        yield return new WaitForSeconds(0.5f);
        if(OnFailedRoundSFX != null){
            OnFailedRoundSFX();
        }

        yield return new WaitForSeconds(3f);

        if(OnNewRound != null){
            OnNewRound(sameRound: true);
        }
        errorSheet.SetActive(false);
    }


    IEnumerator LaunchConfetti(){
        yield return new WaitForSeconds(animationsTime * 2);    // 1 segundo
        confetyParticles.SetActive(true); 
    }

    IEnumerator ReturnToMenu(){
        yield return new WaitForSeconds(animationsTime * 7);    // 3.5 segundos
        
        // Evento para que Load Scene vuelva a la scena del menu principal
        if(OnReturnToMenu != null)  
            OnReturnToMenu();  
    }

}
