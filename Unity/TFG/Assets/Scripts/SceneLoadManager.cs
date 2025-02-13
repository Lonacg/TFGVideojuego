using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private GameObject fadeScene;



    void Awake()
    {
        fadeScene = transform.GetChild(0).gameObject;
    }


    void OnEnable()
    {
        CanvasManagerParking.OnReturnToMenu += HandleOnReturnToMenu;
        
        // Cuando volvemos al menu, mostramos el fadeIn y cambiamos el order in layer del Fade para que se quede por detras y se puedan pulsar los botones de los juegos
        StartCoroutine(HideFadeBehind());      
    } 

    void OnDisable()
    {
        CanvasManagerParking.OnReturnToMenu -= HandleOnReturnToMenu;
    } 

    

    void HandleOnReturnToMenu(){
        StartCoroutine(LoadScene(sceneString: "MainMenu"));
    }



    public void OnLoadParkingButton(){
        StartCoroutine(LoadScene(sceneString: "Parking"));
    }

    public void OnLaneRaceButton(){
        StartCoroutine(LoadScene(sceneString: "LaneRace"));
    }
    
    public void OnDeduceSignButton(){
        StartCoroutine(LoadScene(sceneString: "DeduceSign"));
    }



    IEnumerator LoadScene(string sceneString){
        // Situamos el objeto Fade en el orden uno para que se muestre delante del canvas y se vea el oscurecimiento de la pantalla
        fadeScene.GetComponent<Canvas>().sortingOrder = 1;

        // Activamos la animacion de oscurecer la pantalla
        fadeScene.GetComponent<Animator>().SetTrigger("FadeOutScene");
        yield return new WaitForSeconds(1.5f); // Tiempo que dura la animacion de FadeOutScene

        // Cargamos la escena
        SceneManager.LoadScene(sceneString);

    }

    IEnumerator HideFadeBehind(){
        yield return new WaitForSeconds(1.5f); // Tiempo que dura la animacion de FadeInScene

        // Situamos el Objeto Face en el orden 0 para que se quede detras del canvas con los botones y que estos puedan pulsarse
        fadeScene.GetComponent<Canvas>().sortingOrder = 0;
    }
}
