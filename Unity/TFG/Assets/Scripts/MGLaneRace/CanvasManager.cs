using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Game Objects:")]
    public GameObject player;
    public GameObject[] gates;

    
    [Header("Views:")]
    public GameObject introView;
    public GameObject RSGView;
    public GameObject ingameView;


    [Header("Images:")]
    public Image readyImage;
    public Image steadyImage;
    public Image goImage;
    public Image operationImage;
    public Image scoreImage;
    public Image correctAnswerImage;
    public Image failedAnswerImage;
    

    [Header("Text:")] 
    public TextMeshProUGUI introDialoguePlace;
    public TextMeshProUGUI textOperationPlace;
    public List<string> linesIntroDialogue;


    private float textSpeed = 0.1f;
    private int indexSentence = 0;
    private TextMeshProUGUI textChild;



    public delegate void _OnStart();
    public static event _OnStart OnStart;


    void Awake()
    {
        // Primer dialogo de inicio
        //string line1 = " ¡Intenta llegar a la meta!";
        //linesIntroDialogue.Add(line1);
    }




    void OnEnable(){
        GrannyMovement.OnReady += HandleOnReady;
        GrannyMovement.OnSteady += HandleOnSteady;
        GrannyMovement.OnGo += HandleOnGo;
        TriggerGate.OnWellSol += HandleOnWellSol;
        TriggerGate.OnWrongSol += HandleOnWrongSol;
        StageManagerLaneRace.OnVictory += HandleOnVictory;
    }



    void OnDisable(){
        GrannyMovement.OnReady -= HandleOnReady;
        GrannyMovement.OnSteady -= HandleOnSteady;
        GrannyMovement.OnGo -= HandleOnGo;
        TriggerGate.OnWellSol -= HandleOnWellSol;
        TriggerGate.OnWrongSol -= HandleOnWrongSol;
        StageManagerLaneRace.OnVictory -= HandleOnVictory;
    }

    void Start()
    {
        ingameView.SetActive(false);
        RSGView.SetActive(true);
        //introView.SetActive(true);
        //StartDialogue(introView, introDialoguePlace, linesIntroDialogue);

        // POR NO LANZAR LA INTRO
        if(OnStart != null)   
            OnStart();
    }



    private void HandleOnReady(){
        StartCoroutine(ShowImageForXSeconds(readyImage, seconds: 0.75f, hasText: true));
    }

    private void HandleOnSteady(){
        StartCoroutine(ShowImageForXSeconds(steadyImage, seconds: 0.75f, hasText: true));
    }

    private void HandleOnGo(){
        StartCoroutine(ShowImageForXSeconds(goImage, seconds: 0.4f, hasText: true));
        StartCoroutine(StartIngameView(waitSeconds: 1.1f));
    }

    private void HandleOnWellSol(){
        StartCoroutine(ShowImageForXSeconds(correctAnswerImage, seconds: 0.5f));
    }
    
    private void HandleOnWrongSol(){
        StartCoroutine(ShowImageForXSeconds(failedAnswerImage, seconds: 0.5f));
    }

    private void HandleOnVictory(){
        textOperationPlace.text = "";

        StartCoroutine(FadeImage(operationImage, fromAlpha: 1, toAlpha: 0));
        StartCoroutine(WaitAndFadeImage(scoreImage, 1.3f));
    }






    public void StartDialogue(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        indexSentence = 0;
        dialoguePlace.text = "";
        StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines));
    }

    public void NextLine(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        if (indexSentence < lines.Count){
            dialoguePlace.text = "";
            StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines));
        }
        else{ 
            // Despues de la ultima linea cerramos el dialogo
            StartCoroutine(FadeCanvasGroup(view, fromAlpha: 1, toAlpha: 0));

            // Lanzamos el evento de que la intro ha terminado
            if(OnStart != null)   
                OnStart();
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
        // Corrutina reutilizada de CanvasManagerParking.cs 
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


    IEnumerator ShowImageForXSeconds(Image imageToShow, float seconds, bool hasText = false){ 
        StartCoroutine(FadeImage(imageToShow, fromAlpha: 0, toAlpha: 1, hasText: hasText, animationTime: 0.3f));
        yield return new WaitForSeconds(seconds);
        StartCoroutine(FadeImage(imageToShow, fromAlpha: 1, toAlpha: 0, hasText: hasText, animationTime: 0.5f));
    }

    
    IEnumerator WaitAndFadeImage(Image imageWanted, float seconds){ 
        yield return new WaitForSeconds(seconds);
        StartCoroutine(FadeImage(imageWanted, fromAlpha: 1, toAlpha: 0, animationTime: 0.5f));
    }


    IEnumerator FadeImage(Image imageToShow, float fromAlpha, float toAlpha, bool hasText = false, float animationTime = 0.5f){   
        // Corrutina parcialmente reutilizada de CanvasManagerParking.cs 
        
        // Si la imagen tiene un hijo texto, tambien tenemos que mostrarlo u ocultarlo suavemente con el alpha
        if(hasText){
            textChild = imageToShow.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        }

        if(toAlpha > 0)
            imageToShow.gameObject.SetActive(true);

        float elapsedTime = 0;
        Color colorImage = imageToShow.color;
        while (elapsedTime < animationTime){
            colorImage.a = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / animationTime);
            imageToShow.color = colorImage;
            if(hasText){
                textChild.color = colorImage;
            }

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        
        colorImage.a = toAlpha;
        imageToShow.color = colorImage;
        if(hasText){
            textChild.color = colorImage;
        }        

        if(toAlpha == 0){
            imageToShow.gameObject.SetActive(false);
        }
    }



    IEnumerator StartIngameView(float waitSeconds = 0){
        yield return new WaitForSeconds(waitSeconds);
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1, animationTime: 0.5f));
    }
}
