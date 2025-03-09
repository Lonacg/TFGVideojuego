using UnityEngine;

public class SetOperationDeduceSign : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Variables:")] // Definimos las variables publicas para que StageManager pueda acceder a ellas
    public int firstNumber;
    public int secondNumber;
    public int resultNumber;
    public string answerSign = "";



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()
    {
        GenerateOperation();
    }



    // MÉTODOS DE ESTA CLASE
    public void GenerateOperation(){
        int operatorChosen = Random.Range(0,4);
        
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
        //Debug.Log("La operacion es: " + firstNumber + answerSign + secondNumber + " = " + resultNumber);
    }   

}
