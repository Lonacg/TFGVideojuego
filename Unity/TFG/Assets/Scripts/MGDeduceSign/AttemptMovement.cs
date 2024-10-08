using UnityEngine;
using TMPro;
using System.Collections;



public class AttemptMovement : MonoBehaviour
{

    private TextMeshProUGUI attempPlace;


    void OnEnable()
    {

        attempPlace = gameObject.GetComponent<TextMeshProUGUI>();

        StartCoroutine(TransformSizeFont(startSize: 0, endSize: 80.4f, animationTime: 1));

    }

    IEnumerator MovementText(TextMeshPro text, float animationTime){

        float elapsedTime = 0;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition;

        startPosition.x = -650;
        endPosition.x =  0;

        while(elapsedTime < animationTime){

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = endPosition;

    }


    IEnumerator TransformSizeFont(float startSize, float endSize, float animationTime){
        // Corrutina reutilizada de MGLaneRace

        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            attempPlace.fontSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
    }

    
}
