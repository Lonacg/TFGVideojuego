using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public GameObject introView;
    public GameObject ingameView;
    public GameObject errorView;

    public TextMeshProUGUI introDialoguePlace;
    public TextMeshProUGUI errorDialoguePlace;



    public List<string> linesIntroDialogue;
    public List<string> linesErrorDialogue;
    // Alternativa escribiendo las frases en el inspector
    //public string[] linesIntroDialogue;
    //public string[] linesErrorDialogue;

    public float textSpeed = 0.1f;
    private int index = 0;


    void OnEnable(){
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
    }

    void OnDisable(){
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
    }

    void Awake(){
        // Primer dialogo de inicio
        string line1 = "¿Podrás aparcar en el lugar correcto?";
        string line2 = "3... 2... 1.... ¡YA!";
        linesIntroDialogue.Add(line1);
        linesIntroDialogue.Add(line2);

        // Dialogo tras errorlinesErrorDialogue
        line1 = "¡¡ERROR!!";
        line2 = "Repasa la operación";
        linesErrorDialogue.Add(line1);
        linesErrorDialogue.Add(line2);
    }

    void Start()
    {   
        errorView.SetActive(false);
        introView.SetActive(true);        
        StartDialogue(introView, introDialoguePlace, linesIntroDialogue);
    }

    void Update()
    {

    }

    // Lee la siguiente frase
    public void NextLine(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        if (index < lines.Count){
            dialoguePlace.text = "";
            StartCoroutine(WriteLine(view, dialoguePlace, lines));
        }
        else{ // Despues de la ultima linea cerramos el dialogo y empieza el juego
            StartCoroutine(FadeCanvasGroup(view, from: 1, to: 0));
            StartCoroutine(FadeCanvasGroup(ingameView, from: 0, to: 1));
        }
    }

    public void StartDialogue(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        index = 0;
        StartCoroutine(WriteLine(view, dialoguePlace, lines));
    
    }

    void HandleOnWrongParked (GameObject go){
        StartCoroutine(FadeCanvasGroup(errorView, from: 0, to: 1));
        StartDialogue(errorView, errorDialoguePlace, linesErrorDialogue);
        //RechargeView(ingameView);
    }


    // Escribe letra a letra
    IEnumerator WriteLine(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){ 
        foreach (char letter in lines[index].ToCharArray()){
            dialoguePlace.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        index ++;
        yield return new WaitForSeconds(1);
        NextLine(view, dialoguePlace, lines);
    }

    IEnumerator RechargeView(GameObject view){
        StartCoroutine(FadeCanvasGroup(view, from: 1, to: 0));
        yield return new WaitForSeconds(1.1f);
        StartCoroutine(FadeCanvasGroup(view, from: 0, to: 1));
    }


    // Corrutina para esconder y mostrar vistas suavemente
    IEnumerator FadeCanvasGroup(GameObject view, float from, float to){ 
        CanvasGroup canvasGroup = view.GetComponent<CanvasGroup>();
        if(to > 0)
            view.SetActive(true);

        float animationTime = 0.3f;
        float elapsedTime = 0;

        while(elapsedTime <= animationTime){
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsedTime / animationTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return 0;
        }

        canvasGroup.alpha = to;

        if (to == 0)
            view.SetActive(false);

    }




}
