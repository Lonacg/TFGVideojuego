using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class CanvasManagerDS : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES  
    [Header("Views:")]
    [SerializeField] private GameObject tutorialView;
    [SerializeField] private GameObject ingameView;
    [SerializeField] private GameObject victoryView;
    [SerializeField] private GameObject fadeCircleView;

    [Header("Game Objects:")] 
    [SerializeField] private GameObject roundObject;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()
    {
        StageManagerDeduceSign.OnFadeToPlay += HandleOnFadeToPlay;
        StageManagerDeduceSign.OnGotIt += HandleOnHasWin;
    }

    void OnDisable()
    {
        StageManagerDeduceSign.OnFadeToPlay -= HandleOnFadeToPlay;
        StageManagerDeduceSign.OnGotIt -= HandleOnHasWin;
    }

    void Start()
    {
        // Establecemos la visivilidad de las pantallas para que no influya como se hayan dejado en la edicion de escena de Unity
        tutorialView.SetActive(true);
        ingameView.SetActive(false);
        victoryView.SetActive(false);
        fadeCircleView.SetActive(false);
        roundObject.SetActive(false);
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnFadeToPlay(){
        StartCoroutine(FadeOutFadeIn());
    }

    private void HandleOnHasWin(){
        victoryView.SetActive(true);
    }



    // CORRUTINAS
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

}
