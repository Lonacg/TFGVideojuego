using UnityEngine;

public class SetOperationDeduceSign : MonoBehaviour
{
    public int firstNumber;
    public int secondNumber;
    public int sol;
    public string symbol = "";
    public int operatorChosen;



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

            sol = firstNumber + secondNumber;
            
            symbol = " + ";

        }
        else {
            // RESTA  
            if(operatorChosen == 1){
                firstNumber = Random.Range(13, 40);
                secondNumber = Random.Range(13, firstNumber - 6); 

                sol = firstNumber - secondNumber;

                symbol = " - ";

            } 
            else{
                // MULTIPLICACION
                firstNumber = Random.Range(2, 10);
                secondNumber = Random.Range(2, 10);

                sol = firstNumber * secondNumber;

                symbol = " x ";

                // DIVISION
                if(operatorChosen == 3 ){
                    int aux = firstNumber;

                    firstNumber = sol;
                    sol = secondNumber;
                    secondNumber = aux;

                    symbol = " / ";
                }
            }         
        }
        // Escribimos la operacion por consola para facilitar el desarrollo del minijuego              
        Debug.Log("La operacion es: " + firstNumber + symbol + secondNumber + " = " + sol);
    }
    


    
}




