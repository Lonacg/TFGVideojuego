using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class SetOperationDeduceSign : MonoBehaviour
{
    public int firstNumber;
    public int secondNumber;
    public int resultNumber;
    public string answerSign = "";
    private int operatorChosen;


    void OnEnable(){

        StageManagerDeduceSign.OnCorrectAnswer += HandleOnCorrectAnswer;
        StageManagerDeduceSign.OnHasWin += HandleOnHasWin;

    }

    void OnDisable(){
        StageManagerDeduceSign.OnCorrectAnswer -= HandleOnCorrectAnswer;
        StageManagerDeduceSign.OnHasWin -= HandleOnHasWin;
        
    }


    private void HandleOnCorrectAnswer(){
        GenerateOperation();
    }

    private void HandleOnHasWin(){
        
    }





    void Awake()  // Cada vez que se activa el objeto genera una nueva operacion
    {
        GenerateOperation();

    }



    public void GenerateOperation(){
        // Funcion reutilizada de MGLaneRace
        operatorChosen = Random.Range(0,4);
        
        // SUMA
        if(operatorChosen == 0){
            // Operandos y solucion
            firstNumber = Random.Range(13, 40);
            secondNumber = Random.Range(13,40);

            resultNumber = firstNumber + secondNumber;
            
            answerSign = "Addition";


        }
        else {
            // RESTA  
            if(operatorChosen == 1){
                firstNumber = Random.Range(13, 40);
                secondNumber = Random.Range(13, firstNumber - 6); 

                resultNumber = firstNumber - secondNumber;

                answerSign = "Subtraction";


            } 
            else{
                // MULTIPLICACION
                firstNumber = Random.Range(2, 10);
                secondNumber = Random.Range(2, 10);

                resultNumber = firstNumber * secondNumber;

                answerSign = "Multiplication";


                // DIVISION
                if(operatorChosen == 3 ){
                    int aux = firstNumber;

                    firstNumber = resultNumber;
                    resultNumber = secondNumber;
                    secondNumber = aux;

                    answerSign = "Division";

                }
            }         
        }
        // Escribimos la operacion por consola para facilitar el desarrollo del minijuego              
        Debug.Log("La operacion es: " + firstNumber + answerSign + secondNumber + " = " + resultNumber);
    }
    


    
}




