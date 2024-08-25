using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;

public class SetOperation : MonoBehaviour
{

    public GameObject parkingNumber;
    public Instances scriptInstances;
    public TextMeshProUGUI operationText;

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






    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            secondNumber = Random.Range(150,limit);

            sol = firstNumber + secondNumber;

            symbol = "+";

        }
        else{               // Generamos una resta cuyo resultado tenga como maximo 3 cifras
            firstNumber = Random.Range(501, 999);
            secondNumber = Random.Range(0, firstNumber-50);

            sol = firstNumber - secondNumber;

            symbol = "-";
        }
        textFirstTry = "Aparca en:\n " + firstNumber + " " + symbol + " " + secondNumber;
        textAfterFail = "Aparca en:\n     " + firstNumber + "\n   " + symbol + " " + secondNumber;

        operationText.text = textFirstTry;

        Debug.Log(textFirstTry);        
        Debug.Log(textAfterFail);        
        Debug.Log("El resultado es: " + sol);        
    }

    void HandleOnWrongParked (GameObject go){
        // Sacar una X roja o algo asi


        // Cambiar la forma en la que esta la operacion
        operationText.text = textAfterFail;
        
        // DialogueManager (script) tambien esta suscrito, el hace que desaparezca y aparezca la operacion
    }
    

    public void ChooseWrongAnswer(TextMeshPro ptext, int indexWrongN){
        int incorrectSol = Random.Range(sol - 12, sol +13);

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
                parkingLots[i].tag = "ParkedCorrectly";
                ptext.text = sol.ToString();                // Numero con la solucion
            }
            else{
                parkingLots[i].tag = "ParkedIncorrectly";
                ChooseWrongAnswer(ptext, indexWrongN);      // Numeros sin la solucion
                indexWrongN ++; 
            }
        }        
    }



    // Update is called once per frame
    void Update()
    {

    }
}
