using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{


    private SpriteRenderer spriteRendererBase;
    private SpriteRenderer spriteRendererSign;


    private Color lightGreen;
    private bool selectedSign = false;




    public delegate void _OnSignChosen(GameObject gameObject);
    public static event _OnSignChosen OnSignChosen;
    

    void Start()
    {
        // Asignamos el componente del color del sprite
        spriteRendererBase = gameObject.GetComponent<SpriteRenderer>();
        spriteRendererSign = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        // Establecemos el RGB de un verde claro 
        lightGreen = new Vector4(0.65f, 1, 0.65f, 1);
    }

    void OnEnable(){
        // Inicializamos el booleano como que no se ha seleccionado aun ningun signo
        selectedSign = false;
    }


    private void OnMouseEnter(){
        // Este if no hara falta ponerlo una vez que gestione la accion al pulsar el primer signo, ya que habra que desactivar este script po algo asi
        if( !selectedSign ){
            spriteRendererBase.color = lightGreen;
            spriteRendererSign.color = lightGreen;
        }
    }

    private void  OnMouseExit(){
        // Si pasa el raton por encima resaltamos el boton en verde claro
        if( !selectedSign ){
            spriteRendererBase.color = Color.white;
            spriteRendererSign.color = Color.white;
        }
    }

    private void OnMouseDown(){
        // Si selecciona este signo fijamos el color a verde
        selectedSign = true;
        spriteRendererBase.color = Color.green;
        spriteRendererSign.color = Color.green;

        // Lanzamos el evento de que se ha escogido un signo
        if(OnSignChosen != null)   
            OnSignChosen(gameObject);
    }

}
