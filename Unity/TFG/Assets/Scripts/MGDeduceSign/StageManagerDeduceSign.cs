using UnityEngine;
using TMPro;
using System.Collections;

public class StageManagerDeduceSign : MonoBehaviour
{


    
    
    [Header("Variables")]
    public int numberCorrectAnswers = 0;
    public int numberIncorrectAnswers = 0;
    public int scoreNedeed;




    [Header("Text References:")]
    [SerializeField] private TextMeshPro firsNumberText;
    [SerializeField] private TextMeshPro secondNumberText;
    [SerializeField] private TextMeshPro resultNumberText;


    [Header("GameObjects:")]
    [SerializeField] private GameObject operationParent;    
    [SerializeField] private GameObject plusSign;
    [SerializeField] private GameObject minusSign;
    [SerializeField] private GameObject xSign;

    [SerializeField] private GameObject divisionSign;



    public delegate void _OnChangedCanChoose();
    public static event _OnChangedCanChoose OnChangedCanChoose;



    void OnEnable(){
        ButtonBehaviour.OnSignChosen += HandleOnSignChosen;

    }

    void OnDisable(){
        ButtonBehaviour.OnSignChosen -= HandleOnSignChosen;

    }


    private void HandleOnSignChosen(GameObject buttonChosenGO){
        if(OnChangedCanChoose != null)   
            OnChangedCanChoose();
    }



    void Start()
    {

        // Inicializamos la operacion en "invisible" (escala 0 en y)
        operationParent.transform.localScale = new Vector3(1, 0, 1);

        // Actualizamos el texto de la operacion y la mostramos
        ChangeOperation();
        StartCoroutine(TransformSizeOperation(startSize: 0, endSize: 1, animationTime: 1));

    }

    void Update()
    {
        
    }


    public void ChangeOperation(){  

        // Buscamos los nuevos numeros
        SetOperationDeduceSign scriptSetOperationDeduceSign = operationParent.GetComponent<SetOperationDeduceSign>();
        int firstNumber = scriptSetOperationDeduceSign.firstNumber;
        int secondNumber = scriptSetOperationDeduceSign.secondNumber;
        int resultNumber = scriptSetOperationDeduceSign.resultNumber;

        // Actualizamos los textos
        firsNumberText.text = firstNumber.ToString();
        secondNumberText.text = secondNumber.ToString();
        resultNumberText.text = resultNumber.ToString();

    }


    IEnumerator TransformSizeOperation(float startSize, float endSize, float animationTime){
        // Funcion reutilizada de MGLaneRace
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newScale = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            
            operationParent.transform.localScale = new Vector3(1, newScale, 1);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

    }


    IEnumerator ShowNewOperation(){
        // Funcion reutilizada de MGLaneRace
        StartCoroutine(TransformSizeOperation(startSize: 1, endSize: 0, animationTime: 1));
        yield return new WaitForSeconds(1);
        ChangeOperation();
        StartCoroutine(TransformSizeOperation(startSize: 0, endSize: 1, animationTime: 1));
    }











}
