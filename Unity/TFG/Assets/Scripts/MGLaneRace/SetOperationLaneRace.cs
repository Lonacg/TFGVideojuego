using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


public class SetOperationLaneRace : MonoBehaviour
{
    public int firstNumber;
    public int secondNumber;
    public int sol;
    public int operatorChosen;
    public int[] wrongSols;
    public int[] allWrongSols;

    public List<GameObject> gates;
    private int totalGates;



    


    void OnEnable()  // Cada vez que se activa el objeto genera una nueva operacion
    {
        // Contamos cuantos carriles tenemos para generalizar y poder ampliarlos en el futuro
        totalGates = transform.childCount;

        wrongSols = new int[totalGates -1];
        allWrongSols = new int[(totalGates -1) * 2];

        
        // Asignamos las puertas a una lista
        gates = new List<GameObject>();
        for (int i = 0 ; i < totalGates; i++ ){
            gates.Add(transform.GetChild(i).gameObject); 
        }


        // Generamos la nueva operacion y actualizamos los numeros en el juego
        GenerateOperation();
        WriteNumbers();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShuffleWrongSols(){
        
        // Usamos una variante del algoritmo de Fisher-Yates para desordenar la lista y que los numeros se coloquen de forma aleatoria
        for(int i = wrongSols.Count() - 1 ; i > 0 ; i --){
            int indexRandom = Random.Range(0, i + 1); 

            // Intercmbiamos las posiciones de 2 elementos de la lista en cada vuelta del bucle
            int aux  = wrongSols[i];
            wrongSols[i] = wrongSols[indexRandom];
            wrongSols[indexRandom] = aux;
        }

    }

    public void WriteNumbers(){
        // Elegimos donde colocar la solucion
        int correctPlace = Random.Range(0, totalGates);

        int indexWrongN = 0;
        for(int i = 0 ; i < totalGates ; i ++){
            //Accedemos a la referencia donde esta el numero del texto
            TextMeshPro numberText = gates[i].GetComponentInChildren<TextMeshPro>();

            // Actualizamos el tag y el texto
            if(i == correctPlace){
                gates[i].tag = "CorrectAnswer";
                numberText.text = sol.ToString();
            }
            else{
                gates[i].tag = "IncorrectAnswer";
                numberText.text = wrongSols[indexWrongN].ToString();
                indexWrongN ++;
            }

        }

    }



    public void GenerateOperation(){

        //ACTIVAR ESTA operatorChosen = Random.Range(0,4);
        operatorChosen = Random.Range(0,2);

        // SUMA
        if(operatorChosen == 0){
            // Operandos y solucion
            firstNumber = Random.Range(4, 13);
            secondNumber = Random.Range(4, 13);

            sol = firstNumber + secondNumber;


            // Soluciones incorrectas
            wrongSols = IncorrectAdditionSubtractionOrDivision();


        }
        else {
            // RESTA  
            if(operatorChosen == 1){
                firstNumber = Random.Range(11, 21);
                secondNumber = Random.Range(4, firstNumber - 2); // -2 para tener al menos 2 opciones incorrectas menores

                sol = firstNumber - secondNumber;

                wrongSols = IncorrectAdditionSubtractionOrDivision();
            } 
            else{
                // MULTIPLICACION
                int firstNumber = Random.Range(2, 10);
                int secondNumber = Random.Range(2, 10);

                int sol = firstNumber * secondNumber;
                wrongSols = IncorrectMultiplication();

                // DIVISION
                if(operatorChosen == 3 ){
                    int aux = firstNumber;

                    firstNumber = sol;
                    sol = secondNumber;
                    secondNumber = aux;

                    wrongSols = IncorrectAdditionSubtractionOrDivision();

                }
            }         

        }

        // Escribimos la operacion por consola para facilitar el desarrollo del minijuego
        string symbol = "";
        if(operatorChosen == 0)
            symbol = " + ";
        if(operatorChosen == 1)
            symbol = " - ";
        if(operatorChosen == 2)
            symbol = " x ";
        if(operatorChosen == 3)
            symbol = " / ";                        
        Debug.Log("La operacion es: " + firstNumber + symbol + secondNumber + " = " + sol);
    }
    
    public int[] IncorrectAdditionSubtractionOrDivision(){
        // Cogemos x numeros inmediatamente inferiores y superiores a la solucion, con x el numero de pasillos incorrectos que hay
        int nElements = totalGates - 1;
        int num = sol - nElements;
        int i = 0;
        while(num <= sol + nElements){
            if(num == sol){
                num ++;
                continue;
            }
            allWrongSols[i] = num;
            i ++;
            num ++;    
        }

        wrongSols = ChooseSomeWrong(nElements);

        return wrongSols;
    }

    public int[] IncorrectMultiplication(){ // Tiene la misma estructura que las otras pero cambia respecto a que numero se hace. Se puede refactorifar pasando como parametro los cambios pero creo que pierde mucha legibilidad
        // Cogemos x numeros que sean la solucion del producto por encima y por debajo de la solucion
        int nElements = totalGates - 1;
        int num = secondNumber - nElements;
        int i = 0;
        while(num <= secondNumber + nElements){
            if(num == secondNumber){
                num ++ ;
                continue;
            }
            allWrongSols[i] = firstNumber * num;
            i ++;
            num ++;    
        }  

        wrongSols = ChooseSomeWrong(nElements);

        return wrongSols;
    }




    public int[] ChooseSomeWrong(int amount){
        // De las opciones que hay, elegimos TotalGates numeros consecutivos en la lista, para que en el juego sean por ejemplo si hay 3 puertas, nElements es 2:  -, --, sol ; -, sol, + ; sol, +, ++ 
        int start = Random.Range(0, amount);
        for (int j = 0 ; j < amount ; j ++)
        {
            wrongSols[j] = allWrongSols[start];
            start ++;

        }      

        //ShuffleWrongSols();

        return wrongSols;
    }














}




