using UnityEngine;
using System.Collections;



public class CanvasManagerPuzzle : MonoBehaviour
{    
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject levelsView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject victoryView;
    [SerializeField] private GameObject fadeScene;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()
    {
        StageManagerPuzzle.OnFadeToLevels += HandleOnFadeToLevels;
        StageManagerPuzzle.OnFadeToPlay   += HandleOnFadeToPlay;
        PuzzleCheck.OnGotIt               += HandleOnGotIt;
    }

    void OnDisable()
    {
        StageManagerPuzzle.OnFadeToLevels -= HandleOnFadeToLevels;
        StageManagerPuzzle.OnFadeToPlay   -= HandleOnFadeToPlay;
        PuzzleCheck.OnGotIt               -= HandleOnGotIt;
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnFadeToLevels(){
        StartCoroutine(FadeToLevels());
    }

    private void HandleOnFadeToPlay(GameObject fadecircleView){
        StartCoroutine(FadeOutFadeIn(fadecircleView));
    }

    private void HandleOnGotIt(){
        StartCoroutine(StartVictoryView());
    }

    void Awake()
    {
        // Inicializamos las varibles
        tutorialView.SetActive(true);
        levelsView.SetActive(false);
        ingameView.SetActive(false);
        victoryView.SetActive(true); // Debe ser true siempre
    }



    // CORRUTINAS
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

    IEnumerator StartVictoryView(){
        yield return new WaitForSeconds(1);
        victoryView.SetActive(true);
    }

    IEnumerator FadeToLevels(){

        // Situamos el objeto Fade en el orden uno para que se muestre delante del canvas y se vea el oscurecimiento de la pantalla
        fadeScene.GetComponent<Canvas>().sortingOrder = 1;

        // Activamos la animacion de oscurecer la pantalla
        fadeScene.GetComponent<Animator>().SetTrigger("FadeOutScene");

        yield return new WaitForSeconds(1.5f); // Tiempo que dura la animacion de FadeOutScene

        tutorialView.SetActive(false);
        levelsView.SetActive(true);
        fadeScene.GetComponent<Animator>().SetTrigger("FadeInScene");
        yield return new WaitForSeconds(1.5f); // Tiempo que dura la animacion de FadeOutScene        

        fadeScene.GetComponent<Canvas>().sortingOrder = -3;
    }

}
