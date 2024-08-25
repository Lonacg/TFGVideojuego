using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ErrorXBehaviour : MonoBehaviour
{

    public TextMeshPro textX;

    private float biggerSize = 60;
    private float endSize = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StartCoroutine(MakeAppearance());
    }

    IEnumerator MakeAppearance(){
        StartCoroutine(TransformSizeFont(0, biggerSize));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(TransformSizeFont(biggerSize, endSize));


    }


    IEnumerator TransformSizeFont(float startSize, float endSize){
        float animationTime = 1.5f;
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            textX.fontSize = newSize;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

    }
}
