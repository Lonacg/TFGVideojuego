using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasManagerLaneRace : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Game Objects:")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] gates;
    
    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject RSGView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject fadeCircle;

    [Header("Images:")]
    [SerializeField] private Image readyImage;
    [SerializeField] private Image steadyImage;
    [SerializeField] private Image goImage;
    [SerializeField] private Image operationImage;
    [SerializeField] private Image scoreImage;
    [SerializeField] private Image correctAnswerImage;
    [SerializeField] private Image failedAnswerImage;

    [Header("Text:")] 
    [SerializeField] private TextMeshProUGUI introDialoguePlace;
    [SerializeField] private TextMeshProUGUI textOperationPlace;
    [SerializeField] private List<string> linesIntroDialogue;
    private TextMeshProUGUI textChild;



    // DECLARACIÓN DE EVENTOS
    public delegate void _OnStart();
    public static event _OnStart OnStart;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()
    {
        StageManagerLaneRace.OnFadeToPlay += HandleOnFadeToPlay;
        GrannyMovement.OnReady            += HandleOnReady;
        GrannyMovement.OnSteady           += HandleOnSteady;
        GrannyMovement.OnGo               += HandleOnGo;
        TriggerGate.OnCorrectSol          += HandleOnCorrectSol;
        TriggerGate.OnWrongSol            += HandleOnWrongSol;
        StageManagerLaneRace.OnVictory    += HandleOnVictory;
    }

    void OnDisable(){
        StageManagerLaneRace.OnFadeToPlay -= HandleOnFadeToPlay;
        GrannyMovement.OnReady            -= HandleOnReady;
        GrannyMovement.OnSteady           -= HandleOnSteady;
        GrannyMovement.OnGo               -= HandleOnGo;
        TriggerGate.OnCorrectSol          -= HandleOnCorrectSol;
        TriggerGate.OnWrongSol            -= HandleOnWrongSol;
        StageManagerLaneRace.OnVictory    -= HandleOnVictory;
    }

    void Start()
    {
        ingameView.SetActive(false);
        RSGView.SetActive(true);
        tutorialView.SetActive(true);
        fadeCircle.SetActive(false);
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnFadeToPlay(){
        StartCoroutine(FadeOutFadeIn());
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

    private void HandleOnCorrectSol(){
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



    // CORRUTINAS
    IEnumerator FadeOutFadeIn(){
        // Fade Out
        fadeCircle.SetActive(true);
        yield return new WaitForSeconds(1.5f); // El fade out/in del CircleStatic dura 1,5 seg

        // Desactivamos la vista del tutorial
        tutorialView.SetActive(false);
        
        // Fade In
        fadeCircle.GetComponent<Animator>().SetTrigger("FadeInCircleLaneRace");

        yield return new WaitForSeconds(1f); // El fade out/in del CircleStatic dura 1,5 seg, quitamos 0.5 para que se visualice como una transicion y vaya fluido al aparecer

        if(OnStart != null)   
            OnStart();
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
