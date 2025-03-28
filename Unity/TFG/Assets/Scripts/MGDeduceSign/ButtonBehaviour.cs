using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Sprites:")]
    private SpriteRenderer spriteRendererBase;
    private SpriteRenderer spriteRendererSign;

    [Header("Variables:")]
    private Color lightGreen;
    private bool canChooseButton;



    // DECLARACIÓN DE EVENTOS
    public delegate void _OnSignChosen(GameObject gameObject);
    public static event _OnSignChosen OnSignChosen;
    


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()
    {
        // Inicializamos que no se pueden elegir los botones al activarse
        canChooseButton = false;

        // Eventos:
        StageManagerDeduceSign.OnChangeBoolCanChoose += HandleOnChangeBoolCanChoose;
    }

    void OnDisable()
    {

        StageManagerDeduceSign.OnChangeBoolCanChoose -= HandleOnChangeBoolCanChoose;
    }

    void Start()
    {
        // Asignamos el componente del color del sprite
        spriteRendererBase = gameObject.GetComponent<SpriteRenderer>();
        spriteRendererSign = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        // Establecemos el RGB de un verde claro 
        lightGreen = new Vector4(0.65f, 1, 0.65f, 1);
    }

    private void OnMouseOver()
    {
        // Se usa esta en vez de OnMouseEnter(), aunque conlleve mas coste, porque si ha hecho click encima de la correcta en la pantalla anterior y no ha movido el raton, cuando aparece el boton en la siguiente ronda sigue ahi encima, y con OnMouseEnter no se colorea porque realmente no ha entrado, ya estaba ahi
        if(canChooseButton && spriteRendererBase.color == Color.white){
            spriteRendererBase.color = lightGreen;
        }
    }

    private void  OnMouseExit()
    {
        //  Cuando el raton sale del boton lo volvemos a blanco
        if(canChooseButton){
            spriteRendererBase.color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        if(canChooseButton){
            StartCoroutine(ClickAnimation(seconds: 0.2f));
            
            // El color se cambia en Stage Manager, que es el que sabe si es un boton correcto o incorrecto

            // Lanzamos el evento de que se ha escogido un signo (Stage Manager suscrito)
            if(OnSignChosen != null)   
                OnSignChosen(gameObject);
        }
    }


    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnChangeBoolCanChoose(){
        canChooseButton = !canChooseButton;
    }


    // MÉTODOS DE ESTA CLASE
    public void ChangeButtonColor(Color newColor){
        Vector4 blackboardColor = new Vector4(0.063f, 0.29f, 0.386f, 1);
        spriteRendererBase.color = newColor;
        if(newColor == Color.white)
            spriteRendererSign.color = newColor;
        else
            spriteRendererSign.color = blackboardColor;
    }


    // CORRUTINAS
    IEnumerator ClickAnimation(float seconds){

        float originalScale = gameObject.transform.localScale.x;
        float desiredScale = originalScale - 0.15f;

        float animTime = seconds / 2;
        StartCoroutine(TransformSizeButtom(startSize: originalScale, endSize: desiredScale, animationTime: animTime));
        yield return new WaitForSeconds(animTime);
        StartCoroutine(TransformSizeButtom(startSize: desiredScale, endSize: originalScale, animationTime: animTime));
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
        gameObject.transform.localScale = new Vector3(endSize, endSize, 1);;
    }
    
}
