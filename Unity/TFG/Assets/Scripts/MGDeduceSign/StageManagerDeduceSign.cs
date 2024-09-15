using UnityEngine;
using TMPro;
using System.Collections;

public class StageManagerDeduceSign : MonoBehaviour
{


    
    
    [Header("Variables")]
    public int numberCorrectAnswers = 0;
    public int numberIncorrectAnswers = 0;
    public int scoreNedeed;

    [Header("Game Objects:")]
    public GameObject emptySign;


    [Header("Script:")]
    public SetOperationDeduceSign scriptSetOperationDeduceSign;


    [Header("Text:")]
    public TextMeshProUGUI textOperationPlace;
    public TextMeshProUGUI scoreText;


    void Start()
    {
        ChangeOperation();
    }

    void Update()
    {
        
    }


    public void ChangeOperation(){  

        int firstNumber = scriptSetOperationDeduceSign.firstNumber;
        int secondNumber = scriptSetOperationDeduceSign.secondNumber;

        int operatorChosen = scriptSetOperationDeduceSign.operatorChosen;

        textOperationPlace.text = "";
        textOperationPlace.text = firstNumber + " ? " + secondNumber + " = " + operatorChosen;
    }


    IEnumerator TransformSizeFont(float startSize, float endSize, float animationTime){
        // Funcion reutilizada de MGLaneRace
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            textOperationPlace.fontSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
    }


    IEnumerator ShowNewOperation(){
        // Funcion reutilizada de MGLaneRace
        StartCoroutine(TransformSizeFont(startSize: 54.4f, endSize: 0, animationTime: 1));
        yield return new WaitForSeconds(1);
        ChangeOperation();
        StartCoroutine(TransformSizeFont(startSize: 0, endSize: 54.4f, animationTime: 1));
    }











}
