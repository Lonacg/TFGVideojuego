using System.Collections;
using UnityEngine;

public class CarTravelling : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    private Vector3 startPosition;
    private Vector3 endPosition;
    private int timeTravelling;
    


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
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



    // MÉTODOS DE ESTA CLASE
    void RestartPosition(){
        transform.localPosition = startPosition;
        gameObject.SetActive(false);
    }



    // CORRUTINAS
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
