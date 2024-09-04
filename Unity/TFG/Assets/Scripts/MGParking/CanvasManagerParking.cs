using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class CanvasManagerParking : MonoBehaviour
{
    [Header("Game Objects:")]
    public GameObject player;
    public GameObject introView;
    public GameObject ingameView;
    public GameObject errorView;
    public GameObject victoryView;

    [Header("Text:")]
    public TextMeshProUGUI introDialoguePlace;
    public TextMeshProUGUI errorDialoguePlace;
    public TextMeshProUGUI victoryDialoguePlace;
    public List<string> linesIntroDialogue;
    public List<string> linesErrorDialogue;
    //public List<string> linesVictoryDialogue;

    // Alternativa escribiendo las frases en el inspector
    //public string[] linesIntroDialogue;
    //public string[] linesErrorDialogue;

    [Header("Variables:")]
    public float textSpeed = 0.1f;
    private int indexSentence = 0;



    void OnEnable(){
        ParkingTrigger.OnWellParked += HandleOnWellParked;
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
    }

    void OnDisable(){
        ParkingTrigger.OnWellParked -= HandleOnWellParked;
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
    }



    void HandleOnWrongParked(GameObject go){
        StartCoroutine(FadeCanvasGroup(errorView, fromAlpha: 0, toAlpha: 1));
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 1, toAlpha: 0));
        StartDialogue(errorView, errorDialoguePlace, linesErrorDialogue);
    }

    void HandleOnWellParked(GameObject go){
        StartCoroutine(FadeCanvasGroup(victoryView, fromAlpha: 0, toAlpha: 1));
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 1, toAlpha: 0));
        player.GetComponent<CarMovement>().enabled = false;
    }



    void Awake(){
        // Primer dialogo de inicio
        string line1 = " ¿Podrás aparcar en el lugar correcto?";
        string line2 = "3... 2... 1.... ¡YA!";
        linesIntroDialogue.Add(line1);
        linesIntroDialogue.Add(line2);

        // Dialogo tras error
        line1 = "¡¡ERROR!!";
        line2 = "Repasa la operación";
        linesErrorDialogue.Add(line1);
        linesErrorDialogue.Add(line2);

        // Dialogo tras victoria (esta escrito directamente en la etiqueta)
        //line1 = "¡¡Conseguido!!";
        //linesVictoryDialogue.Add(line1);
    }

    void Start(){   
        errorView.SetActive(false);
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
            
            // Activamos el Script de Player para que no se pueda mover mientras dure la corrutina 
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

    IEnumerator FadeCanvasGroup(GameObject view, float fromAlpha, float toAlpha){ 
        CanvasGroup canvasGroup = view.GetComponent<CanvasGroup>();
        if(toAlpha > 0)
            view.SetActive(true);

        float animationTime = 0.3f;
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

}
