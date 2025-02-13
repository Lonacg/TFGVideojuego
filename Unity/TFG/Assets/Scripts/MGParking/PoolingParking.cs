using System.Collections;
using UnityEngine;

public class PoolingParking : MonoBehaviour
{
    [SerializeField] private GameObject[] carsRoad;  // Se rellena desde el inspector de Unity con los 10 prefabs



    void Start()
    {
        StartCoroutine(RoundCar());
    }

    

    private int RandomSeconds(){
        int seconds = Random.Range(1, 4);
        return seconds;
    }

    private void Shuffle(){
        // Usamos una variante del algoritmo de Fisher-Yates para desordenar el array y que los prefabs se coloquen de forma aleatoria
        for(int i = carsRoad.Length - 1 ; i > 0 ; i --){
            int indexRandom = Random.Range(0, i + 1); 

            // Intercambiamos las posiciones de 2 elementos de la lista en cada vuelta del bucle
            GameObject aux  = carsRoad[i];
            carsRoad[i] = carsRoad[indexRandom];
            carsRoad[indexRandom] = aux;
        }
    }



    IEnumerator RoundCar(bool firstCarAfterRound = false){
        // Desordenamos el array de prefabs 
        Shuffle();

        // Activamos una ronda de los coches, es decir, apareceran los 10 prefabs mientras el jugador no haya ganado
        int secondsToWait;
        for(int i = 0 ; i < carsRoad.Length ; i ++ ){
            if(firstCarAfterRound){   // Ponemos este If para que al coche de la ultima ronda le de tiempo a cruzar por completo, por si acaso el ultimo coche de una ronda es el primero de la siguiente
                secondsToWait = 3;       // 3 es la variable timeTravelling de CarTarvelling.cs. No se puede poner que acceda a la variable de ese script ya que ese script esta en cada uno de los coche, no en uno unicamente. Este valor deberia estar en un script independienteStageManager.cs y que ambos accedieran a el
                firstCarAfterRound = false;
            }
            else{
                secondsToWait = RandomSeconds(); // Activaremos un coche cada secondsToWait segundos
            }
            
            yield return new WaitForSeconds(secondsToWait);
            carsRoad[i].SetActive(true);
        }   
                    
        // Volvemos a empezar una nueva ronda
        StartCoroutine(RoundCar(firstCarAfterRound: true));
    }

}