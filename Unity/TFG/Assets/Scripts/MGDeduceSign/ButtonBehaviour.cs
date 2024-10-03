using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{


    private SpriteRenderer spriteRendererBase;
    private SpriteRenderer spriteRendererSign;


    private Color lightGreen;
    public bool canChooseButton = true;




    public delegate void _OnSignChosen(GameObject gameObject);
    public static event _OnSignChosen OnSignChosen;
    

    void OnEnable(){
        // Inicializamos que se pueden elegir los botones cuando el script se activa
        canChooseButton = true;


        // Eventos:
        StageManagerDeduceSign.OnChangedCanChoose += HandleOnChangedCanChoose;

    }

    void OnDisable(){

        StageManagerDeduceSign.OnChangedCanChoose -= HandleOnChangedCanChoose;

    }


    private void HandleOnChangedCanChoose(){
        canChooseButton = !canChooseButton;
    }



    void Start(){
        // Inicializamos que se pueden elegir los botones
        canChooseButton = true;

        // Asignamos el componente del color del sprite
        spriteRendererBase = gameObject.GetComponent<SpriteRenderer>();
        spriteRendererSign = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        // Establecemos el RGB de un verde claro 
        lightGreen = new Vector4(0.65f, 1, 0.65f, 1);
    }


    private void OnMouseEnter(){

        // Si pasa el raton por encima resaltamos el boton en verde claro
        if(canChooseButton){
            ChangeButtonColor(lightGreen);
        }
    }

    private void  OnMouseExit(){

        //  Cuando el raton sale del boton lo volvemos a blanco
        if(canChooseButton){

            ChangeButtonColor(Color.white);
        }
    }

    private void OnMouseDown(){

        if(canChooseButton){
            StartCoroutine(ClickAnimation(seconds: 0.2f));
            
            // ChangeButtonColor(Color.green);

            // Lanzamos el evento de que se ha escogido un signo (Stage Manager suscrito)
            if(OnSignChosen != null)   
                OnSignChosen(gameObject);
        }
    }

 
    public void ChangeButtonColor(Color newColor){
        spriteRendererBase.color = newColor;
        spriteRendererSign.color = newColor;
    }


    IEnumerator ClickAnimation(float seconds){
        float time = seconds / 2;
        StartCoroutine(TransformSizeButtom(startSize: 1, endSize: 0.85f, animationTime: time));
        yield return new WaitForSeconds(time);
        StartCoroutine(TransformSizeButtom(startSize: 0.85f, endSize: 1, animationTime: time));
    }

    IEnumerator TransformSizeButtom(float startSize, float endSize, float animationTime){
        // Funcion reutilizada de MGLaneRace
        float elapsedTime = 0;

        while(elapsedTime < animationTime){
            float newScale = Mathf.Lerp(startSize, endSize, elapsedTime / animationTime);
            
            gameObject.transform.localScale = new Vector3(newScale, newScale, 1);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

    }

}
