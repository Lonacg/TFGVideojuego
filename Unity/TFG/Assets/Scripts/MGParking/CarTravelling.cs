using System.Collections;
using UnityEngine;

public class CarTravelling : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private int timeTravelling;
    


    void Awake(){
        startPosition = Vector3.zero;
        endPosition = new Vector3(0, 0, 18.5f);
        timeTravelling = 3; // Si se cambia, hay que actualizarlo tambien en PoolingParking.cs (RoundCar()), por no tener StageManager.cs

    }

    void OnEnable(){
        StartCoroutine(Travelling());
    }


    void RestartPosition(){
        transform.localPosition = startPosition;
        gameObject.SetActive(false);
    }


    IEnumerator Travelling(){
        float elapsedTime = 0;

        while(elapsedTime < timeTravelling){
            
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / timeTravelling);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }

        RestartPosition();
    }
}
