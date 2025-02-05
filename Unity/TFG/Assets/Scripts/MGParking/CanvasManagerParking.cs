using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class CanvasManagerParking : MonoBehaviour
{
    [Header("Game Objects:")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject introView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject victoryView;
    [SerializeField] private GameObject operationImage;
    [SerializeField] private GameObject errorImage;

    [Header("Texts:")]
    [SerializeField] private TextMeshProUGUI introDialoguePlace;
    [SerializeField] private TextMeshProUGUI errorDialoguePlace;
    [SerializeField] private TextMeshProUGUI victoryDialoguePlace;
    [SerializeField] private TextMeshProUGUI operationFirstTryText;
    [SerializeField] private TextMeshProUGUI operationSecondTryText;
    [SerializeField] private List<string> linesIntroDialogue;
    //public List<string> linesVictoryDialogue;

    // Alternativa escribiendo las frases en el inspector
    //public string[] linesIntroDialogue;
    //public string[] linesErrorDialogue;

    //[Header("Variables:")]
    private float textSpeed = 0.1f;
    private int indexSentence = 0;
    private bool firstTry= true;



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
        StartCoroutine(FadeCanvasGroup(victoryView, fromAlpha: 0, toAlpha: 1));
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 1, toAlpha: 0));
        player.GetComponent<CarMovement>().enabled = false;
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
        introView.SetActive(true);        
        StartDialogue(introView, introDialoguePlace, linesIntroDialogue);
    }



    public void StartDialogue(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        indexSentence = 0;
        dialoguePlace.text = "";

        // Desactivamos el Script de Player para que no se pueda mover mientras dure la corrutina 
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
            
            // Activamos el Script de Player para que vuelva a poder moverse
            player.GetComponent<CarMovement>().enabled = true;
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
    }

}
