using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    public GameObject player;
    public GameObject introView;
    public GameObject ingameView;

    public GameObject[] gates;
    


    public Image correctAnswerImage;
    public Image failedAnswerImage;
    public Image scorePanel;
    public Image operationPanel;
    public TextMeshProUGUI textOperation;


    public float textSpeed = 0.1f;
    private int indexSentence = 0;
    private int currentGround = 0;



    void OnEnable(){
        TriggerGate.OnWellSol += HandleOnWellSol;
        TriggerGate.OnWrongSol += HandleOnWrongSol;
        GroundMovement.OnGo += HandleOnGo;
        StageManagerLaneRace.OnVictory += HandleOnVictory;

    }



    void OnDisable(){
        TriggerGate.OnWellSol -= HandleOnWellSol;
        TriggerGate.OnWrongSol -= HandleOnWrongSol;
        GroundMovement.OnGo -= HandleOnGo;
        StageManagerLaneRace.OnVictory -= HandleOnVictory;

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


    private void HandleOnGo(){
        StartCoroutine(StartIngameView(0.2f));

    }

    private void HandleOnVictory(){
        textOperation.text = "";

        StartCoroutine(FadeImage(operationPanel, fromAlpha: 1, toAlpha: 0));
        StartCoroutine(WaitAndFadeImage(scorePanel, 1.3f));

    }




    public void IncreaseCurrentGround(){
        if(currentGround == 2){
            currentGround = 0;
        }
        else
            currentGround ++;

    }


    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            textOperation.fontSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

    }


    public void ChangeOperation(){
        GameObject currentGate = gates[currentGround];
        SetOperationLaneRace scriptSetOperation = currentGate.GetComponent<SetOperationLaneRace>();

        int firstNumber = scriptSetOperation.firstNumber;
        int secondNumber = scriptSetOperation.secondNumber;

        string symbol = scriptSetOperation.symbol;

        textOperation.text = "";
        textOperation.text = firstNumber + symbol + secondNumber;
    }


    IEnumerator ShowNewOperation(){
        StartCoroutine(TransformSizeFont(startSize: 54.4f, endSize: 0, animationTime: 1));
        yield return new WaitForSeconds(1);
        ChangeOperation();
        StartCoroutine(TransformSizeFont(startSize: 0, endSize: 54.4f, animationTime: 1));
    }


    IEnumerator StartIngameView(float seconds){
        yield return new WaitForSeconds(seconds);
        ChangeOperation();
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1, animationTime: 0.5f));
    }


}
