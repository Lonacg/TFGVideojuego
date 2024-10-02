using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StageManagerDeduceSign : MonoBehaviour
{


    
    
    [Header("Variables")]
    private int numberCorrectAnswers = 0;
    private int numberIncorrectAnswers = 0;
    private int scoreNedeed;
    private bool canChooseButton = true;
    private string answerSign;





    [Header("Text References:")]
    [SerializeField] private TextMeshPro firsNumberText;
    [SerializeField] private TextMeshPro secondNumberText;
    [SerializeField] private TextMeshPro resultNumberText;


    [Header("GameObjects:")]
    [SerializeField] private GameObject operationParent; 
    private List<GameObject> incorrectButtonsChosen;   




    public delegate void _OnChangedCanChoose();
    public static event _OnChangedCanChoose OnChangedCanChoose;


    public delegate void _OnCorrectAnswer();
    public static event _OnCorrectAnswer OnCorrectAnswer;


    public delegate void _OnWrongAnswer();
    public static event _OnWrongAnswer OnWrongAnswer;



    void OnEnable(){
        ButtonBehaviour.OnSignChosen += HandleOnSignChosen;

    }

    void OnDisable(){
        ButtonBehaviour.OnSignChosen -= HandleOnSignChosen;

    }


    private void HandleOnSignChosen(GameObject buttonChosenGO){

        // Con este if(canChooseButton), no contabilizamos los clicks que haga en los botones cuando NO se pueden pulsar
        if(canChooseButton){

            // Le decimos a los botones que se ha seleccionado uno de los signos, para que cambien su bool y no se pueda pulsar otro
            if(OnChangedCanChoose != null){
                OnChangedCanChoose();
                canChooseButton = false;
            }

            // Comprobamos si el simbolo escogido es el correcto
            CheckAnswer(buttonChosenGO);
        }

            
    }



    void Start()
    {
        canChooseButton = true;

        // Inicializamos la operacion en "invisible" (escala 0 en y)
        operationParent.transform.localScale = new Vector3(1, 0, 1);
        
        // Obtenemos los numeros de la operacion generada
        SetOperationDeduceSign scriptSetOperation = operationParent.GetComponent<SetOperationDeduceSign>();
        answerSign = scriptSetOperation.answerSign;


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

    public void CheckAnswer(GameObject goSign){
        // Terrible para leer: if((goSign.tag == "Addition" && answerSign == 0) || (goSign.tag == "Subtraction" && answerSign == 1) || (goSign.tag == "Multiplication" && answerSign == 2) || (goSign.tag == "Division" && answerSign == 3));
        
        // Solucion correcta
        if((goSign.tag == answerSign) || (goSign.tag == answerSign) || (goSign.tag == answerSign) || (goSign.tag == answerSign)){

            
            ManageCorrectAnswer(goSign);

            if(OnCorrectAnswer != null){
                OnCorrectAnswer();
            }
        }
        // Solucion incorrecta
        else{ 

            if(OnWrongAnswer != null){
                OnWrongAnswer();
            }

            ManageWrongAnswer(goSign);

        }

        if(OnChangedCanChoose != null){
            OnChangedCanChoose();
            canChooseButton = true;
        }
    }

    public void ManageCorrectAnswer(GameObject goSign){

        numberCorrectAnswers ++;
        

        // Cambiamos el boton a verde y deshabilitamos el script
        goSign.GetComponent<ButtonBehaviour>().ChangeButtonColor(Color.green);

        ShowNewOperation();


    }

    public void ManageWrongAnswer(GameObject goSign){
        // Metemos el boton en una lista para luego volver a activarles el script
        // incorrectButtonsChosen.Add(goSign); me dice null reference, por que?

        // Cambiamos el boton a rojo y deshabilitamos el script
        ButtonBehaviour scriptButton = goSign.GetComponent<ButtonBehaviour>();
        scriptButton.ChangeButtonColor(Color.red);
        scriptButton.enabled = false; 
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
