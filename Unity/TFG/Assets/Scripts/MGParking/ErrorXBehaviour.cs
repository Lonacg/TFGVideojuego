using System.Collections;
using TMPro;
using UnityEngine;

public class ErrorXBehaviour : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    private TextMeshPro textX;
    private float biggerSize = 60;
    private float endSize = 30;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()
    {
        textX = GetComponentInChildren<TextMeshPro>();
        StartCoroutine(MakeAppearance());
    }

    void Start()
    {
        biggerSize = 60;
        endSize = 30;
    }



    // CORRUTINAS
    IEnumerator MakeAppearance(){
        StartCoroutine(TransformSizeFont(0, biggerSize));
        yield return new WaitForSeconds(1f);
        StartCoroutine(TransformSizeFont(biggerSize, endSize));
    }

    IEnumerator TransformSizeFont(float startSize, float endSize){
        float animationTime = 1f;
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            textX.fontSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
    }

}
