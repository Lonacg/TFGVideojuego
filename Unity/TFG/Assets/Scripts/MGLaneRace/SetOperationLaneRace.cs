using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetOperationLaneRace : MonoBehaviour
{

    public int firstNumber;
    public int secondNumber;
    public int sol;
    public int operatorChosen;
    private int[] wrongSols;
    private int[] allWrongSols;


    private int totalGates;



    
    void Start(){
        // Contamos cuantos carriles tenemos para generalizar y poder ampliarlos en el futuro
        GameObject parent = transform.parent.gameObject;
        totalGates = parent.transform.childCount;

        wrongSols = new int[totalGates - 1];
        allWrongSols = new int[wrongSols.Count() * 2];

    }


    void OnEnable()  // Cada vez que se activa el objeto genera una nueva operacion
    {
        GenerateOperation();

        // Actualizar numeros de las gates

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateOperation(){

        operatorChosen = Random.Range(0,4);

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

        wrongSols = ChoseSomeWrong(nElements);

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

        wrongSols = ChoseSomeWrong(nElements);

        return wrongSols;
    }

    public int[] IncorrectDivision(){ // Tiene la misma estructura que la suma y la resta
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

        wrongSols = ChoseSomeWrong(nElements);

        return wrongSols;
    }


    public int[] ChoseSomeWrong(int amount){
        // De las opciones que hay, elegimos TotalGates numeros consecutivos en la lista, para que en el juego sean por ejemplo si hay 3 puertas, nElements es 2:  -, --, sol ; -, sol, + ; sol, +, ++ 
        int start = Random.Range(0, amount);
        for (int j = 0 ; j < amount ; j ++)
        {
            wrongSols[j] = allWrongSols[start];
            start ++;

        }      

        return wrongSols;
    }














}




