using System.Collections;
using UnityEngine;

public class CarTravelling : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private int timeTravelling;
    


    void Awake()
    {
        startPosition = Vector3.zero;
        endPosition = new Vector3(0, 0, 18.5f);
        timeTravelling = 3; // Si se cambia, hay que actualizarlo tambien en PoolingParking.cs (RoundCar()), por no tener StageManager.cs
    }

    void OnEnable()
    {
        StartCoroutine(Travelling());
    }



    void RestartPosition(){
        transform.localPosition = startPosition;
        gameObject.SetActive(false);
    }



    IEnumerator Travelling(){
        // Cuerpo general de una corrutina, con la posicion local de los vehiculos en vez de la global (mas simple el montaje de la escena en este caso)
        float elapsedTime = 0;
        while(elapsedTime < timeTravelling){
            
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / timeTravelling);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = endPosition;

        // Reseteamos la posicion del vehículo para la proxima vez que vuelva a salir
        RestartPosition();
    }

}