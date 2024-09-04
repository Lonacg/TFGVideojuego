using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class SetOperationParking : MonoBehaviour
{

    public GameObject parkingNumber;
    public Instances scriptInstances;
    public TextMeshProUGUI operationFirstTryText;
    public TextMeshProUGUI operationSecondTryText;

    public List<GameObject> parkingNumbers;
    public List<GameObject> parkingLots;

    private int firstNumber;
    private int secondNumber;
    private int sol;
    private int numberFreeParkings;
    private int[] incorrectNumbers;

    private string symbol;
    private string textFirstTry;
    private string textAfterFail;



    void OnEnable(){
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
    }

    void OnDisable(){
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
    }



    void HandleOnWrongParked (GameObject go){
        StartCoroutine(ChangeTextSecondTry());
    }



    void Start()
    {   
        parkingNumbers = scriptInstances.pNumbers;
        parkingLots = scriptInstances.pLots;
        numberFreeParkings = parkingLots.Count;
        
        GenerateOperation();
        // Llamar a la funcion que muestre la operacion
        ChangeNumberParking();
    }



    public void GenerateOperation(){
        // Generamos un numero aleatorio para decidir si sera una suma o una resta
        int i = Random.Range(0,2);

        if (i == 0){        // Generamos una suma cuyo resultado tenga como maximo 3 cifras
            firstNumber = Random.Range(110, 800);
            int limit = 999 - firstNumber;
            secondNumber = Random.Range(11, limit);

            sol = firstNumber + secondNumber;

            symbol = "+";

        }
        else{               // Generamos una resta cuyo resultado tenga como maximo 3 cifras
            firstNumber = Random.Range(501, 999);
            secondNumber = Random.Range(11, firstNumber - 50);
            
            sol = firstNumber - secondNumber;

            symbol = "-";
        
        }

        textFirstTry = firstNumber + " " + symbol + " " + secondNumber;
        textAfterFail = firstNumber + "\n" + symbol + " " + secondNumber;        

        // Rellenamos con espacios a la izq para que el texto AfterFail quede bien representado (secondPart no es igual a la suma, firstPart si)
        if(secondNumber < 100)
            textAfterFail = firstNumber + "\n" + symbol + "  " + secondNumber;  

        // Ponemos en el cuadro de texto la operacion
        operationFirstTryText.text = textFirstTry;

        //Debug.Log("Operacion: " + textFirstTry + " = " + sol);          
    }

    public void ChooseWrongAnswer(TextMeshPro ptext, int indexWrongN){

        // Elegimos el intervalo en el que se generaran las soluciones incorrectas
        int min = sol - 12;
        if(min < 0)
            min = 0;

        int max = sol + 13;  // Sumo uno mas porque Random.Range no toma el limite superior
        if(max > 999)
            max = 999;
        
        int incorrectSol = Random.Range(min, max);

        // Evitamos poner numeros repetidos o la solucion correcta
        bool duplicated = incorrectNumbers.Contains(incorrectSol);

        if (duplicated || incorrectSol == sol)      // MUY POCO EFICIENTE, REVISAR!!!!!!!!!!!!!!!!!!!
            ChooseWrongAnswer(ptext, indexWrongN);
        else{
            ptext.text = incorrectSol.ToString();
            incorrectNumbers[indexWrongN] = incorrectSol;
        }
    }

    public void ChangeNumberParking(){ // Cambia el Tag del parking y escribe el numero de la solucion

        // Generamos un numero aleatorio para elegir en que aparcamiento se coloca la solucion correcta
        int correctPlace = Random.Range(0, numberFreeParkings);

        // Asignamos el tag ParkedCorrectly al aparcamiento con la solucion correcta y ParkedIncorrectly al resto
        incorrectNumbers = new int[numberFreeParkings -1];
        int indexWrongN = 0;
        for (int i = 0 ; i < numberFreeParkings ; i++ ){

            // Accedemos al numero de la plaza de aparcamiento para luego cambiarlo
            TextMeshPro ptext = parkingNumbers[i].GetComponentInChildren<TextMeshPro>();
            
            // Actualizamos el tag del aparcamiento y el numero de la plaza
            if( i == correctPlace){
                parkingLots[i].tag = "CorrectAnswer";
                ptext.text = sol.ToString();                // Numero con la solucion
            }
            else{
                parkingLots[i].tag = "IncorrectAnswer";
                ChooseWrongAnswer(ptext, indexWrongN);      // Numeros sin la solucion
                indexWrongN ++; 
            }
        }        
    }



    IEnumerator ChangeTextSecondTry(){
        yield return new WaitForSeconds(1);
        // Cambiamos la forma en la que esta la operacion para ayudar al jugador
        operationFirstTryText.gameObject.SetActive(false);
        operationSecondTryText.text = textAfterFail;
        operationSecondTryText.gameObject.SetActive(true);
        
        // DialogueManager (script) tambien esta suscrito, el hace que desaparezca y aparezca la operacion
    }

}
