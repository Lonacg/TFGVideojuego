using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StageManagerDeduceSign : MonoBehaviour
{


    
    
    [Header("Variables")]
    private int numberCorrectAnswers = 0;
    //private int numberIncorrectAnswers = 0;
    private int scoreNedeed;
    public bool canChooseButton = true;
    private string answerSign;




    [Header("Text References:")]
    [SerializeField] private TextMeshPro firsNumberText;
    [SerializeField] private TextMeshPro secondNumberText;
    [SerializeField] private TextMeshPro resultNumberText;


    [Header("GameObjects:")]
    [SerializeField] private GameObject operationParent; 
    public List<GameObject> buttonsChosen;   




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
            
            // Metemos el boton en una lista para luego volver a activarles el script
            buttonsChosen.Add(buttonChosenGO.gameObject); // me dice null reference, por que? Sol: igual que arriba, aunque goSing sea un game object, cuando se mete a la lista tenemos que decir que queremos meterlo como .gameObject
            
            // Le decimos a los botones que se ha seleccionado uno de los signos, para que cambien su bool y no se pueda pulsar otro
            if(OnChangedCanChoose != null){
                OnChangedCanChoose();
            }

            canChooseButton = false;

            // Comprobamos si el simbolo escogido es el correcto
            CheckAnswer(buttonChosenGO);
        }

            
    }


    void Start()
    {
        canChooseButton = true;
        scoreNedeed = 3;
        

        // Inicializamos la operacion en "invisible" (escala 0 en y)
        operationParent.transform.localScale = new Vector3(1, 0, 1);


        // Actualizamos el texto de la operacion y la mostramos
        ChangeOperation();
        StartCoroutine(TransformSizeOperation(startSize: 0, endSize: 1, animationTime: 0.5f));

    }

 
    public void ChangeOperation(){  

        // Buscamos los nuevos numeros
        SetOperationDeduceSign scriptSetOperation = operationParent.GetComponent<SetOperationDeduceSign>();
        int firstNumber = scriptSetOperation.firstNumber;
        int secondNumber = scriptSetOperation.secondNumber;
        int resultNumber = scriptSetOperation.resultNumber;
        answerSign = scriptSetOperation.answerSign;



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

            ManageWrongAnswer(goSign);

            if(OnWrongAnswer != null){
                OnWrongAnswer();
            }

        }

        // Avisamos a los botones de que ya se pueden pulsar
        if(OnChangedCanChoose != null){
            OnChangedCanChoose();
        }
        canChooseButton = true;
    }


    public void ManageCorrectAnswer(GameObject goSign){

        numberCorrectAnswers ++;

        // Cambiamos el boton a verde y deshabilitamos el script
        StartCoroutine(ButtonGreenAndNewOpertion(goSign));

    }

    public void ManageWrongAnswer(GameObject goSign){

        // Cambiamos el boton a rojo y deshabilitamos el script
        ButtonBehaviour scriptButton = goSign.GetComponent<ButtonBehaviour>();
        Vector4 darkRed = new Vector4(0.8f, 0, 0, 1);
        scriptButton.ChangeButtonColor(darkRed);
        scriptButton.enabled = false; 

    } 





    public void RestartButtons(){
        if(buttonsChosen[0] != null){
            foreach(GameObject goSign in buttonsChosen){
                // ButtonBehaviour scriptButton = goSign.GetComponent<ButtonBehaviour>(); // NO SE PUEDE PONER ESTO porque en este momento el script esta desactivado asi que lanza Null reference
                goSign.GetComponent<ButtonBehaviour>().enabled = true; 
                goSign.GetComponent<ButtonBehaviour>().ChangeButtonColor(Color.white);
                
            }
            buttonsChosen = new List<GameObject>();
        }
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
        operationParent.transform.localScale = new Vector3(1, endSize, 1);

    }

    IEnumerator ButtonGreenAndNewOpertion(GameObject goSign){

        // Cambiamos el color a verde oscuro y desactivamos el Script para que no cambia automaticamente de color si se quita el raton de encima
        ButtonBehaviour buttonScript = goSign.GetComponent<ButtonBehaviour>();
        buttonScript.ChangeButtonColor(Color.green);
        buttonScript.enabled = false;
        yield return new WaitForSeconds(1f);

        StartCoroutine(ShowNewOperation(animationTime: 0.5f));
        buttonScript.enabled = true;
        buttonScript.ChangeButtonColor(Color.white);


        RestartButtons();
        
    }

    IEnumerator ShowNewOperation(float animationTime){

        // Funcion reutilizada de MGLaneRace
        StartCoroutine(TransformSizeOperation(startSize: 1, endSize: 0, animationTime: animationTime));
        yield return new WaitForSeconds(animationTime);

        ChangeOperation();

        StartCoroutine(TransformSizeOperation(startSize: 0, endSize: 1, animationTime: animationTime));

    }











}
