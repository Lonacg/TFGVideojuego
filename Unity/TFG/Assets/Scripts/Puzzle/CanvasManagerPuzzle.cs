using UnityEngine;
using System.Collections;

public class CanvasManagerPuzzle : MonoBehaviour
{

    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject levelsView;
    [SerializeField] private GameObject ingameView;




    void OnEnable()
    {
        StageManagerPuzzle.OnFadeToPlay += HandleOnFadeToPlay;
    }

    void OnDisable()
    {
        StageManagerPuzzle.OnFadeToPlay -= HandleOnFadeToPlay;
    }



    private void HandleOnFadeToPlay(GameObject fadecircleView){
        StartCoroutine(FadeOutFadeIn(fadecircleView));
    }


    void Awake()
    {
        // Orden final
        // tutorialView.SetActive(true);
        // levelsView.SetActive(false);
        // ingameView.SetActive(false);
        // fadecircleViewEasy.SetActive(false);
        // fadecircleViewMedium.SetActive(false);
        // fadecircleViewHard.SetActive(false);

        // Pruebas:
        tutorialView.SetActive(false);
        levelsView.SetActive(true);
        ingameView.SetActive(false);
      

    }






    IEnumerator FadeOutFadeIn(GameObject fadeCircleView){
        // Fade Out
        fadeCircleView.SetActive(true);
        yield return new WaitForSeconds(1.5f); // El fade out/in del CircleStatic dura 1,5 seg

        // Desactivamos la vista del tutorial
        levelsView.SetActive(false);
        ingameView.SetActive(true); 
         
        // Fade In
        fadeCircleView.GetComponent<Animator>().SetTrigger("FadeInCirclePuzzle");

        yield return new WaitForSeconds(1f); // El fade out/in del CircleStatic dura 1,5 seg, quitamos 0.5 para que se visualice como una transicion y vaya fluido al aparecer
           

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



    IEnumerator StartIngameView(float waitSeconds = 0){
        yield return new WaitForSeconds(waitSeconds);
        StartCoroutine(FadeCanvasGroup(ingameView, fromAlpha: 0, toAlpha: 1, animationTime: 0.5f));
    }










}
