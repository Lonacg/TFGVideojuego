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
    private int currentGround = 0;



    public delegate void _OnStart();
    public static event _OnStart OnStart;





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

    private void HandleOnReady(){
        StartCoroutine(ShowImageForXSeconds(readyImage, 0.75f));
    }
    private void HandleOnSteady(){
        StartCoroutine(ShowImageForXSeconds(steadyImage, 0.75f));
    }

    private void HandleOnGo(){
        StartCoroutine(ShowImageForXSeconds(goImage, 0.4f));
        StartCoroutine(StartIngameView(waitSeconds: 1.1f));
    }



    private void HandleOnWellSol(){
        IncreaseCurrentGround();

        StartCoroutine(ShowImageForXSeconds(correctAnswerImage, 1.3f));
        StartCoroutine(ShowNewOperation());
    }
    
    private void HandleOnWrongSol(){
        IncreaseCurrentGround();

        StartCoroutine(ShowImageForXSeconds(failedAnswerImage, 1.3f));
        StartCoroutine(ShowNewOperation());
    }

    


    private void HandleOnVictory(){
        textOperationPlace.text = "";

        StartCoroutine(FadeImage(operationImage, fromAlpha: 1, toAlpha: 0));
        StartCoroutine(WaitAndFadeImage(scoreImage, 1.3f));

    }

    void Awake(){
        // Primer dialogo de inicio
        string line1 = " ¡Intenta llegar a la meta!";
        linesIntroDialogue.Add(line1);

    }


    void Start()
    {
        ingameView.SetActive(false);
        RSGView.SetActive(true);
        introView.SetActive(true);
        StartDialogue(introView, introDialoguePlace, linesIntroDialogue);

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void IncreaseCurrentGround(){
        if(currentGround == 2){
            currentGround = 0;
        }
        else
            currentGround ++;

    }

    public void ChangeOperation(){
        GameObject currentGate = gates[currentGround];
        SetOperationLaneRace scriptSetOperation = currentGate.GetComponent<SetOperationLaneRace>();

        int firstNumber = scriptSetOperation.firstNumber;
        int secondNumber = scriptSetOperation.secondNumber;

        string symbol = scriptSetOperation.symbol;

        textOperationPlace.text = "";
        textOperationPlace.text = firstNumber + symbol + secondNumber;
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

    IEnumerator ShowImageForXSeconds(Image imageToShow, float seconds){ 

        StartCoroutine(FadeImage(imageToShow, fromAlpha: 0, toAlpha: 1, animationTime: 0.3f));
        yield return new WaitForSeconds(seconds);
        StartCoroutine(FadeImage(imageToShow, fromAlpha: 1, toAlpha: 0, animationTime: 0.5f));
    }

    
    IEnumerator WaitAndFadeImage(Image imageWanted, float seconds){ 

        yield return new WaitForSeconds(seconds);
        StartCoroutine(FadeImage(imageWanted, fromAlpha: 1, toAlpha: 0, animationTime: 0.5f));

    }


    IEnumerator FadeImage(Image imageToShow, float fromAlpha, float toAlpha, float animationTime = 0.5f){
        float elapsedTime = 0;
        Color colorImage = imageToShow.color;

        if(toAlpha > 0)
            imageToShow.gameObject.SetActive(true);

        while (elapsedTime < animationTime){
            colorImage.a = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / animationTime);
            imageToShow.color = colorImage;

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        
        colorImage.a = toAlpha;
        imageToShow.color = colorImage;

        if(toAlpha == 0){
            imageToShow.gameObject.SetActive(false);
        }
    }



    IEnumerator TransformSizeFont(float startSize, float endSize, float animationTime){
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            textOperationPlace.fontSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

    }





    IEnumerator ShowNewOperation(){
        StartCoroutine(TransformSizeFont(startSize: 54.4f, endSize: 0, animationTime: 1));
        yield return new WaitForSeconds(1);
        ChangeOperation();
        StartCoroutine(TransformSizeFont(startSize: 0, endSize: 54.4f, animationTime: 1));
    }


    IEnumerator StartIngameView(float waitSeconds = 0){
        yield return new WaitForSeconds(waitSeconds);
        ChangeOperation();
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1, animationTime: 0.5f));
    }


}
