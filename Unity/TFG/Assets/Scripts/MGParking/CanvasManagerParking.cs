using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CanvasManagerParking : MonoBehaviour
{
    [Header("Game Objects:")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject introView;
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject victoryView;
    [SerializeField] private GameObject fadeCircleStatic;
    [SerializeField] private GameObject backgroundBlack;
    [SerializeField] private GameObject operationImage;
    [SerializeField] private GameObject errorImage;
    [SerializeField] private GameObject poolingRoad;


    [Header("Texts:")]
    [SerializeField] private TextMeshProUGUI introDialoguePlace;
    [SerializeField] private TextMeshProUGUI errorDialoguePlace;
    [SerializeField] private TextMeshProUGUI victoryDialoguePlace;
    [SerializeField] private TextMeshProUGUI operationFirstTryText;
    [SerializeField] private TextMeshProUGUI operationSecondTryText;
    [SerializeField] private List<string> linesIntroDialogue;

    //[Header("Variables:")]
    private float textSpeed = 0.1f;
    private int indexSentence = 0;
    private bool firstTry= true;



    public delegate void _OnPlay();
    public static event _OnPlay OnPlay;

    public delegate void _OnGotIt();
    public static event _OnGotIt OnGotIt;

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
        if(OnGotIt != null)  // Lanzamos un evento de Conseguido para que se reproduzca el sonido
            OnGotIt();
        player.GetComponent<AudioSource>().enabled = false;
        player.GetComponent<CarMovement>().enabled = false;

        // Esperamos dos segundos para que se vean los confeti y volvemos al menu principal
        StartCoroutine(ReturnToMenu());
    }



    void Awake(){
        // Primer dialogo de inicio
        //string line1 = " ¿Podrás aparcar en el lugar correcto?";
        string line1 = " ";
        //string line2 = "3... 2... 1.... ¡YA!";
        linesIntroDialogue.Add(line1);
        //linesIntroDialogue.Add(line2);

        // Dialogo tras victoria (esta escrito directamente en la etiqueta)
        //line1 = "¡¡Conseguido!!";
        //linesVictoryDialogue.Add(line1);
    }

    void Start(){   
        ingameView.SetActive(false);
        victoryView.SetActive(false);
        //introView.SetActive(true);        
        //StartDialogue(introView, introDialoguePlace, linesIntroDialogue);
        tutorialView.SetActive(true);

    }


    void Update(){
        // Si la ventana de tutorial esta activado y pulsan espacio damos paso al inicio del juego
        if(tutorialView.activeSelf && Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(StartGame());
        }
    }


    IEnumerator StartGame(){
            // StartCoroutine(FadeCanvasGroup(tutorialView, fromAlpha: 1, toAlpha: 0));
            //StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1));

            fadeCircleStatic.GetComponent<Animator>().SetTrigger("FadeOutCircleStatic");
            yield return new WaitForSeconds(2f); // El fade out/in del CircleStatic dura 1,5 seg

            // Lo ponemos negro completo mientras cambiamos la vista
            backgroundBlack.SetActive(true);
            Debug.Log("fONDO NEGRO ACTIVADO");
            tutorialView.SetActive(false);
            ingameView.SetActive(true);


            // Destapamos la pantalla
            backgroundBlack.SetActive(false);
            Debug.Log("fONDO NEGRO DESACTIVADO");
            fadeCircleStatic.GetComponent<Animator>().SetTrigger("FadeInCircleStatic");



            // Avisamos de que empieza el juego
            if(OnPlay != null)                          
                OnPlay();

            // Activamos el Script de Player para que vuelva a poder moverse, el sonido del motor en marcha y activamos el Pool de objetos para que aparezcan los vehículos cruzando la calle
            player.GetComponent<CarMovement>().enabled = true;
            player.GetComponent<AudioSource>().enabled = true;

            poolingRoad.SetActive(true);
    }

    public void StartDialogue(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        indexSentence = 0;
        dialoguePlace.text = "";

        // Desactivamos el Script de Player para que no se pueda mover mientras dure la corrutina y el AudioSouerce para que no se ejecute el sonido del motor
        player.GetComponent<AudioSource>().enabled = false;
        player.GetComponent<CarMovement>().enabled = false;

        StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines));
    }

    public void NextLine(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        if (indexSentence < lines.Count){
            dialoguePlace.text = "";
            StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines));
        }
        else{ // Despues de la ultima linea cerramos el dialogo y empieza el juego
            StartCoroutine(FadeCanvasGroup(view, fromAlpha: 1, toAlpha: 0));
            StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1));
            

            // Empieza el juego: activamos el Script de Player para que vuelva a poder moverse y activamos el Pool de objetos para que aparezcan los vehículos cruzando la calle
            if(OnPlay != null)                          // Si hay alguien suscrito al evento, le envia la info
                OnPlay();
            player.GetComponent<CarMovement>().enabled = true;
            player.GetComponent<AudioSource>().enabled = true;

            poolingRoad.SetActive(true);

        }
    }



    IEnumerator WriteLetterByLetter(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){ 
        foreach (char letter in lines[indexSentence].ToCharArray()){
            dialoguePlace.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        indexSentence ++;
        yield return new WaitForSeconds(1);
        NextLine(view, dialoguePlace, lines);
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
        
        // AQUI OSCURECER PANTALLA
        SceneManager.LoadScene("MainMenu");
    }

}
