using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasManagerDS : MonoBehaviour
{
    

    
    [Header("Views:")]
    public GameObject introView;
    public GameObject ingameView;
    public GameObject errorView;
    public GameObject victoryView;




    [Header("Text:")] 
    public TextMeshProUGUI introDialoguePlace;
    public TextMeshProUGUI errorDialoguePlace;
    public List<string> linesIntroDialogue;
    public List<string> linesErrorDialogue;


    [Header("Variables:")]
    private float textSpeed = 0.1f;
    private int indexSentence = 0;



    public delegate void _OnSameRound();
    public static event _OnSameRound OnSameRound;



    void OnEnable(){
        StageManagerDeduceSign.OnHasWin += HandleOnHasWin;
        StageManagerDeduceSign.OnErrorView += HandleOnErrorView;
        

    }



    void OnDisable(){
        StageManagerDeduceSign.OnHasWin -= HandleOnHasWin;
        StageManagerDeduceSign.OnErrorView -= HandleOnErrorView;
    }


    private void HandleOnHasWin(){
        victoryView.SetActive(true);
    }

    private void HandleOnErrorView(){
        StartCoroutine(StartErrorView());
        

    }


    void Awake(){
        // Primer dialogo de inicio
        //string line1 = "Adivina el signo     de la operación...       ¡para poder seguir con tu misión!";
        string line1 = "go";
        linesIntroDialogue.Add(line1);

        line1 = "¡Inténtalo de nuevo!";
        linesErrorDialogue.Add(line1);
    }

    void Start()
    {
        ingameView.SetActive(false);
        errorView.SetActive(false);
        victoryView.SetActive(false);
        introView.SetActive(true);
        StartDialogue(introView, introDialoguePlace, linesIntroDialogue);
    }



    public void StartDialogue(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines, bool errorActive = false){
        indexSentence = 0;
        dialoguePlace.text = "";
        StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines, errorActive));
    }


    public void NextLine(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines, bool errorActive = false){
        if(indexSentence < lines.Count){
            dialoguePlace.text = ""; 
            StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines, errorActive));
        }
        else{ 
            // Despues de la ultima linea cerramos el dialogo
            StartCoroutine(FadeCanvasGroup(view, fromAlpha: 1, toAlpha: 0));

            if(errorActive){
                if(OnSameRound != null)   
                    OnSameRound();

            }
            else{
                StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1));
            }
            
        }
    }



    IEnumerator WriteLetterByLetter(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines, bool errorActive = false){ 
        foreach (char letter in lines[indexSentence].ToCharArray()){
            dialoguePlace.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        indexSentence ++;
        yield return new WaitForSeconds(1);
        NextLine(view, dialoguePlace, lines, errorActive);
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



    IEnumerator StartErrorView(){
        float animationTime = 1;
        StartCoroutine(FadeCanvasGroup(errorView, fromAlpha: 0, toAlpha: 1, animationTime));
        yield return new WaitForSeconds(animationTime + 0.5f);
        StartDialogue(errorView, errorDialoguePlace, linesErrorDialogue, errorActive: true);
    }
}
