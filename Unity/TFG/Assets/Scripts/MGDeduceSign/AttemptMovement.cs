using UnityEngine;
using TMPro;
using System.Collections;



public class AttemptMovement : MonoBehaviour
{

    private TextMeshProUGUI attemptPlace;




    public delegate void _OnPlaying();
    public static event _OnPlaying OnPlaying;


    void OnEnable()
    {

        attemptPlace = gameObject.GetComponent<TextMeshProUGUI>();

        ShowAttempt();


        // Eventos:

        

        // Movimiento con corrutina en vez de animaciones
        //StartCoroutine(TransformSizeFont(startSize: 0, endSize: 80.4f, animationTime: 1));

    }



    private void ShowAttempt(){

        StartCoroutine(WaitAndMoveAttempts());

    }



    IEnumerator WaitAndMoveAttempts(){
        yield return new WaitForSeconds(1.5f);

        // Cogemos el componente animator y lanzamos el trigger para que cambie de animacion
        attemptPlace.GetComponent<Animator>().SetTrigger("MoveAttempt");

        yield return new WaitForSeconds(0.5f);
        // Lanzamos el evento para que se active la operacion y los botones
        if(OnPlaying != null)   
            OnPlaying();


    }








    // Movimiento del texto
    // IEnumerator MovementText(TextMeshPro text, float animationTime){

    //     float elapsedTime = 0;

    //     Vector3 startPosition = transform.localPosition;
    //     Vector3 endPosition = transform.localPosition;

    //     startPosition.x = -650;
    //     endPosition.x =  0;

    //     while(elapsedTime < animationTime){

    //         transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

    //         elapsedTime += Time.deltaTime;
    //         yield return 0;
    //     }
    //     transform.localPosition = endPosition;

    // }

    // Agrandar el texto
    // IEnumerator TransformSizeFont(float startSize, float endSize, float animationTime){
    //     // Corrutina reutilizada de MGLaneRace

    //     float elapsedTime = 0;

    //     while(elapsedTime < animationTime){
    //         float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
    //         attempPlace.fontSize = newSize;
    //         elapsedTime += Time.deltaTime;
    //         yield return 0;
    //     }
    // }

    
}
