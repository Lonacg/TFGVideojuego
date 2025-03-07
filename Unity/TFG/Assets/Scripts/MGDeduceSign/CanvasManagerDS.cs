using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class CanvasManagerDS : MonoBehaviour
{
      
    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject victoryView;
    [SerializeField] private GameObject fadeCircleView;


    [Header("Game Objects:")] 
    [SerializeField] private GameObject roundObject;
    [SerializeField] private List<string> linesIntroDialogue;


    [Header("Variables:")]
    private float textSpeed = 0.1f;
    private int indexSentence = 0;



    void OnEnable(){
        StageManagerDeduceSign.OnFadeToPlay += HandleOnFadeToPlay;
        StageManagerDeduceSign.OnGotIt += HandleOnHasWin;

    }


    void OnDisable(){
        StageManagerDeduceSign.OnFadeToPlay -= HandleOnFadeToPlay;
        StageManagerDeduceSign.OnGotIt -= HandleOnHasWin;
    }

    private void HandleOnFadeToPlay(){
        StartCoroutine(FadeOutFadeIn());
    }

    private void HandleOnHasWin(){
        victoryView.SetActive(true);
    }



    void Awake(){
        // Primer dialogo de inicio
        //string line1 = "Adivina el signo     de la operación...       ¡para poder seguir con tu misión!";
        //linesIntroDialogue.Add(line1);
    }


    void Start()
    {
        tutorialView.SetActive(true);
        ingameView.SetActive(false);
        victoryView.SetActive(false);
        fadeCircleView.SetActive(false);
        roundObject.SetActive(false);

    }



    public void StartDialogue(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        indexSentence = 0;
        dialoguePlace.text = "";
        StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines));
    }


    public void NextLine(GameObject view, TextMeshProUGUI dialoguePlace, List<string> lines){
        if(indexSentence < lines.Count){
            dialoguePlace.text = ""; 
            StartCoroutine(WriteLetterByLetter(view, dialoguePlace, lines));
        }
        else{ 
            // Despues de la ultima linea cerramos el dialogo
            StartCoroutine(FadeCanvasGroup(view, fromAlpha: 1, toAlpha: 0));
            StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1));
        }
    }

    IEnumerator FadeOutFadeIn(){
        // Fade Out
        fadeCircleView.SetActive(true);
        yield return new WaitForSeconds(1.5f); // El fade out/in del CircleStatic dura 1,5 seg

        // Desactivamos la vista del tutorial
        tutorialView.SetActive(false);
        ingameView.SetActive(true);
        
        // Fade In
        fadeCircleView.GetComponent<Animator>().SetTrigger("FadeInCircleDeduceSign");

        yield return new WaitForSeconds(1f); 

        // Empezamos el jeugo activando las rondas
        roundObject.SetActive(true);

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
}
