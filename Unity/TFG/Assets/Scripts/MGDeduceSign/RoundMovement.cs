using System.Collections;
using TMPro;
using UnityEngine;

public class RoundMovement : MonoBehaviour
{


    [SerializeField] AnimationCurve curve;
    [SerializeField] TextMeshProUGUI attempText;




    void OnEnable()
    {
        TextMeshPro roundText = GetComponent<TextMeshPro>();

        StartCoroutine(MovementText(roundText, animationTime: 1));

    }

    IEnumerator MovementText(TextMeshPro text, float animationTime){

        float elapsedTime = 0;

        Vector3 startPosition = transform.localPosition;
        Vector3 endPosition = transform.localPosition;

        startPosition.x = -650;
        endPosition.x =  0;

        while(elapsedTime < animationTime){

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(elapsedTime / animationTime));

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.localPosition = endPosition;

        // Activamos el texto de "Intentos"
        attempText.gameObject.SetActive(true);
    }


}


