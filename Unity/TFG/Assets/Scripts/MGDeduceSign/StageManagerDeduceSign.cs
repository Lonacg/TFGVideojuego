using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System;

public class StageManagerDeduceSign : MonoBehaviour
{


    
    
    [Header("Variables")]
    private int numberCorrectAnswers = 0;
    //private int numberIncorrectAnswers = 0;
    public int totalRounds = 3;
    public int maxAttempts;
    public int attemptsNumber;
    private string answerSign;
    public int indexRound = 1;
    public int tryNumber = 2;
    private bool firstOperation;
    private float animationsTime = 0.5f;



    [Header("Text References:")]
    [SerializeField] private TextMeshPro firsNumberText;
    [SerializeField] private TextMeshPro secondNumberText;
    [SerializeField] private TextMeshPro resultNumberText;
    [SerializeField] private TextMeshProUGUI attemptPlace;

    


    [Header("GameObjects:")]
    [SerializeField] private GameObject buttonsParent;     
    [SerializeField] private GameObject operationParent; 
    [SerializeField] private GameObject confetyParticles;



    public List<GameObject> buttonsChosen;   




    public delegate void _OnChangeBoolCanChoose();
    public static event _OnChangeBoolCanChoose OnChangeBoolCanChoose;



    public delegate void _OnWrongAnswer();
    public static event _OnWrongAnswer OnWrongAnswer;



    public delegate void _OnHasWin();
    public static event _OnHasWin OnHasWin;


    public delegate void _OnNewRound(bool sameRound);
    public static event _OnNewRound OnNewRound;



    void OnEnable(){
        ButtonBehaviour.OnSignChosen += HandleOnSignChosen;
        RoundBehaviour.OnShowAttempt += HandleOnShowAttempt;
        AttemptMovement.OnPlaying += HandleOnPlaying;

    }

    void OnDisable(){
        ButtonBehaviour.OnSignChosen -= HandleOnSignChosen;
        RoundBehaviour.OnShowAttempt -= HandleOnShowAttempt;
        AttemptMovement.OnPlaying -= HandleOnPlaying;
    }


    private void HandleOnSignChosen(GameObject buttonChosenGO){

        // Metemos el boton en una lista para luego volver a activarles el script
        buttonsChosen.Add(buttonChosenGO.gameObject); // me dice null reference, por que? Sol: igual que arriba, aunque goSing sea un game object, cuando se mete a la lista tenemos que decir que queremos meterlo como .gameObject
        
        // Avisamos a los botones para que cambien a false
        if(OnChangeBoolCanChoose != null){
            OnChangeBoolCanChoose();
        }


        // Comprobamos si el simbolo escogido es el correcto
        CheckAnswer(buttonChosenGO);
         
    }

    
    private void HandleOnShowAttempt(){
        attemptPlace.gameObject.SetActive(false);
        attemptPlace.gameObject.SetActive(true);
    }


    private void HandleOnPlaying(){
        buttonsParent.SetActive(false);
        buttonsParent.SetActive(true);

        operationParent.SetActive(false);
        operationParent.SetActive(true);
        StartCoroutine(FadeInOperation(animationsTime));

    }


    void Awake(){
        buttonsParent.SetActive(false);
        operationParent.SetActive(false);
        attemptPlace.gameObject.SetActive(false);

        SetAttemptsNumber();

    }



    void Start()
    {
        animationsTime = 0.5f;
        totalRounds = 3;
        firstOperation = true;
        

        // Inicializamos la operacion en "invisible" (escala 0 en y) para que al activarlo no se vea
        operationParent.transform.localScale = new Vector3(1, 0, 1);
       
        
    }


    private void SetAttemptsNumber(){
        if(totalRounds > 4)
            maxAttempts = 4;
        else
            maxAttempts = totalRounds;
            
        attemptsNumber = maxAttempts;
    }


 
    public void UpdateNumbers(){  

        // Buscamos los nuevos numeros generados en el script SetOperation
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
        
        // Solucion correcta
        if((goSign.tag == answerSign) || (goSign.tag == answerSign) || (goSign.tag == answerSign) || (goSign.tag == answerSign)){        // Terrible para leer: if((goSign.tag == "Addition" && answerSign == 0) || (goSign.tag == "Subtraction" && answerSign == 1) || (goSign.tag == "Multiplication" && answerSign == 2) || (goSign.tag == "Division" && answerSign == 3));

            ManageCorrectAnswer(goSign);

        }
        else{ // Solucion incorrecta

            ManageWrongAnswer(goSign);

            if(OnWrongAnswer != null){
                OnWrongAnswer();
            }

            // Avisamos a los botones para que cambien a true
            if(OnChangeBoolCanChoose != null){
                OnChangeBoolCanChoose();
            }

        }

    }


    public void ManageCorrectAnswer(GameObject goSign){

        MakeButtonGreen(goSign);

        numberCorrectAnswers ++;

        if(numberCorrectAnswers == totalRounds){
            Debug.Log("VICTORIA FINAL!!!!!!!!!!!");

            // Evento de victoria: se ocultan los elementos de la pizarra y canvas muestra el conseguido
            if(OnHasWin != null) 
                OnHasWin();
            StartCoroutine(FadeOutOperation(animationsTime));
            buttonsParent.GetComponent<Animator>().SetTrigger("FadeOut");

            StartCoroutine(LaunchFireworks());
        }
        else{
            StartCoroutine(WaitAndNewRound(sameRound:false));
        }

        maxAttempts --;
        attemptsNumber = maxAttempts;

    }

    public void ManageWrongAnswer(GameObject goSign){
        attemptsNumber --;
        
        MakeButtonRed(goSign);

        if(attemptsNumber == 0){
            
            StartCoroutine(WaitAndNewRound(sameRound:true));
            attemptsNumber = maxAttempts;

        }

    } 

    public void MakeButtonGreen(GameObject goSign){
        goSign.GetComponent<ButtonBehaviour>().ChangeButtonColor(Color.green);
    }


    public void MakeButtonRed(GameObject goSign){
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


    IEnumerator FadeOutOperation(float animationTime){

        StartCoroutine(TransformSizeOperation(startSize: 1, endSize: 0, animationTime: animationTime));
        yield return 0;
    }

    IEnumerator FadeInOperation(float animationTime){

        if(firstOperation){
            firstOperation = false;
        }
        else{
            RestartButtons();
        }

        UpdateNumbers();

        StartCoroutine(TransformSizeOperation(startSize: 0, endSize: 1, animationTime: animationTime));
        yield return new WaitForSeconds(animationTime); 

        // Avisamos para que cambien a true
        if(OnChangeBoolCanChoose != null){
            OnChangeBoolCanChoose();
        }
    }

 

    IEnumerator WaitAndNewRound(bool sameRound){
        yield return new WaitForSeconds(animationsTime);

        if(OnNewRound != null){
            OnNewRound(sameRound);
        }

        buttonsParent.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(FadeOutOperation(animationsTime));
    }

    IEnumerator LaunchFireworks(){
        yield return new WaitForSeconds(animationsTime);
        confetyParticles.SetActive(true); 

    }






}
