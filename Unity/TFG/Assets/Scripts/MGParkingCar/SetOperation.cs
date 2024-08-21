using UnityEngine;
using System.Collections.Generic;

public class SetOperation : MonoBehaviour
{

    public GameObject parkingNumber;

    private int firstNumber;
    private int secondNumber;
    private int sol;
    private int correctPlace;
    public GameObject[] parkingNumbers;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        // Generamos un numero aleatorio para decidir si sera una suma o una resta
        int i = Random.Range(0,2);

        if (i == 0){  // Generamos una suma cuyo resultado tenga como maximo 3 cifras
            firstNumber = Random.Range(110, 800);
            int limit = 999 - firstNumber;
            secondNumber = Random.Range(150,limit);

            sol = firstNumber + secondNumber;

        }
        else{         // Generamos una resta cuyo resultado tenga como maximo 3 cifras
            firstNumber = Random.Range(501, 999);
            secondNumber = Random.Range(0, firstNumber-50);

            sol = firstNumber - secondNumber;

        }
        Debug.Log("La operacion es: " + i + "La operacion es " + firstNumber + " y " + secondNumber);


        // Generamos un numero aleatorio para elegir en que aparcamiento se coloca la solucion correcta
        int totalParkings = parkingNumber.transform.childCount;
        correctPlace = Random.Range(0, totalParkings);

        // Asignamos el tag ParkedCorrectly al aparcamiento con la solucion correcta y ParkedIncorrectly al resto
        parkingNumbers = new GameObject[totalParkings];
        for (int j = 0 ; j < totalParkings ; j++ ){
            parkingNumbers[j] = parkingNumber.transform.GetChild(j).gameObject;
            
            if( j == correctPlace)
                parkingNumbers[j].tag = "ParkedCorrectly";
            else
                parkingNumbers[j].tag = "ParkedIncorrectly"; 
        }


        //gameObject.tag = "loquesea"
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
