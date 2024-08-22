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
    //public GameObject[] parkingNumbers;
    public CarInstances scriptCarInstances;

    public int numberFreeParkings;
    public List<GameObject> parkingNumbers;

    private int firstNumber;
    private int secondNumber;
    private int sol;
    private int[] incorrectNumbers;


    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        numberFreeParkings = scriptCarInstances.pNumbers.Count;
        parkingNumbers = scriptCarInstances.pNumbers;

        GenerateOperation();
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

        }
        else{               // Generamos una resta cuyo resultado tenga como maximo 3 cifras
            firstNumber = Random.Range(501, 999);
            secondNumber = Random.Range(0, firstNumber-50);

            sol = firstNumber - secondNumber;
        }

        Debug.Log("La operacion es: " + i + ". La operacion es " + firstNumber + " y " + secondNumber);        
        Debug.Log("El resultado es: " + sol);        
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
        Debug.Log("El sitio correcto es: " + correctPlace);

        // Asignamos el tag ParkedCorrectly al aparcamiento con la solucion correcta y ParkedIncorrectly al resto
        //parkingNumbers = new GameObject[numberFreeParkings];
        incorrectNumbers = new int[numberFreeParkings -1];
        int indexWrongN = 0;
        for (int i = 0 ; i < numberFreeParkings ; i++ ){
            //parkingNumbers[i] = parkingNumber.transform.GetChild(i).gameObject;

            // Accedemos al numero de la plaza de aparcamiento para luego cambiarlo
            TextMeshPro ptext = parkingNumbers[i].GetComponentInChildren<TextMeshPro>();
            
            // Actualizamos el tag del aparcamiento y el numero de la plaza
            if( i == correctPlace){
                parkingNumbers[i].tag = "ParkedCorrectly";
                ptext.text = sol.ToString();                // Numero con la solucion
            }
            else{
                parkingNumbers[i].tag = "ParkedIncorrectly";
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
