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
        CanvasManagerParking.OnReturnToMenu   += HandleOnReturnToMenu;
        StageManagerLaneRace.OnReturnToMenu   += HandleOnReturnToMenu;
        StageManagerDeduceSign.OnReturnToMenu += HandleOnReturnToMenu;
        StageManagerPuzzle.OnReturnToMenu     += HandleOnReturnToMenu;
        
        StartCoroutine(HideFadeBehind());
    } 

    void OnDisable()
    {
        CanvasManagerParking.OnReturnToMenu   -= HandleOnReturnToMenu;
        StageManagerLaneRace.OnReturnToMenu   -= HandleOnReturnToMenu;
        StageManagerDeduceSign.OnReturnToMenu -= HandleOnReturnToMenu;
        StageManagerPuzzle.OnReturnToMenu     -= HandleOnReturnToMenu;
    } 

    

    void HandleOnReturnToMenu(){
        StartCoroutine(LoadScene(sceneString: "MainMenu"));
    }

    void HandleOnReturnToCover(){
        StartCoroutine(LoadScene(sceneString: "Cover"));
    }



    public void OnStartButton(){
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

    public void OnPuzzleButton(){
        StartCoroutine(LoadScene(sceneString: "Puzzle"));
    }

    public void OnQuitButton(GameObject button){
        StartCoroutine(ClickAnimation(button, seconds: 0.5f));
        StartCoroutine(LoadScene(sceneString: "Cover"));
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
        fadeScene.GetComponent<Canvas>().sortingOrder = -3;
    }


    IEnumerator ClickAnimation(GameObject gObject, float seconds){
        // Corrutina reutilizada de DeduceSign
        float originalScale = gObject.transform.localScale.x;
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
