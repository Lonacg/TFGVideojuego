using UnityEngine;
using System.Collections;



public class StageManagerMainMenu : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES 
    [Header("Game Objects:")]
    [SerializeField] private GameObject buttonsParking;
    [SerializeField] private GameObject buttonsLR;
    [SerializeField] private GameObject buttonsDS;
    [SerializeField] private GameObject buttonsPuzzle;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject particles1;
    [SerializeField] private GameObject particles2;
    
    [Header("Views:")]
    [SerializeField] private GameObject panelConfirmationView;
    [SerializeField] private GameObject titleView;

    [Header("Variables from GameChecker:")]
    private int amountGamesPlayed;
    private bool alreadyPlayed;
    private bool parkingPlayed = false;
    private bool laneRacePlayed = false;
    private bool deduceSignPlayed = false;
    private bool puzzlePlayed= false;



    // DECLARACIÓN DE EVENTOS
    public delegate void _OnMoveButtonDS();
    public static event _OnMoveButtonDS OnMoveButtonDS;

    public delegate void _OnFormulaAppearance();
    public static event _OnFormulaAppearance OnFormulaAppearance;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Start()
    {
        panelConfirmationView.SetActive(false);
        UpdateVariablesGameChecker();

        UpdateButtons();

        // Gestion del movimiento del ultimo boton
        if(!puzzlePlayed){
            UpdateMovementDS();
        }
        else{
            buttonsDS.transform.localPosition = new Vector3(0, -10.81f, 0);
        }
    }



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public void OnQuitButton(){
        // Mostramos el panel de confirmacion y desactivamos los botones de los minijuegos
        StartCoroutine(FadeCanvasGroup(panelConfirmationView, fromAlpha: 0, toAlpha: 1, changeButtons: true));

        // Hacemos desaparecer el banner del titulo y el boton de salir pulsado
        StartCoroutine(FadeCanvasGroup(titleView, fromAlpha: 1, toAlpha: 0));
        StartCoroutine(FadeCanvasGroup(quitButton, fromAlpha: 1, toAlpha: 0));
    }

    public void OnCancelQuitButton(){
        // Mostramos el panel de confirmacion y activamos los botones de los minijuegos
        StartCoroutine(FadeCanvasGroup(panelConfirmationView, fromAlpha: 1, toAlpha: 0, changeButtons: true));

        // Hacemos aparecer el banner del titulo y el boton de salir pulsado
        StartCoroutine(FadeCanvasGroup(titleView, fromAlpha: 0, toAlpha: 1));
        StartCoroutine(FadeCanvasGroup(quitButton, fromAlpha: 0, toAlpha: 1));
    }

    private void UpdateButtons(){
        if (parkingPlayed){
            buttonsParking.transform.GetChild(1).gameObject.SetActive(true);
            buttonsParking.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (laneRacePlayed){
            buttonsLR.transform.GetChild(1).gameObject.SetActive(true);
            buttonsLR.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (deduceSignPlayed){
            buttonsDS.transform.GetChild(1).gameObject.SetActive(true);
            buttonsDS.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (puzzlePlayed){
            buttonsPuzzle.transform.GetChild(1).gameObject.SetActive(true);
            buttonsPuzzle.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void UpdateMovementDS(){

        alreadyPlayed = GameChecker.Instance.GetAlreadyPlayed();

        if(!alreadyPlayed){
            // Se ha jugado por primera vez a un minijuego asi que hacemos la animacion correspondiente
            if(amountGamesPlayed == 1){
                // Esta en la posicion que debe tener, asi que solo activamos las particulas
                particles1.SetActive(true);

                // Primer movimiento del boton DeduceSign
                Vector3 endPosition = new Vector3(143.5f, -10.81f, 0);
                StartCoroutine(MoveDSButton(endPosition));
            }
            else{
                if(amountGamesPlayed == 2){
                    // Primero, actualizamos la posicion que debe tener y activamos las particulas
                    buttonsDS.transform.localPosition = new Vector3(143.5f, -10.81f, 0);
                    particles1.SetActive(true);
                    particles2.SetActive(true);
                    
                    // Hacemos el segundo movimiento del boton DeduceSign
                    Vector3 endPosition = new(72, -10.81f, 0);
                    StartCoroutine(MoveDSButton(endPosition));
                }
                else{
                    if(amountGamesPlayed == 3){
                        // Primero, actualizamos la posicion que debe tener y activamos las particulas
                        buttonsDS.transform.localPosition = new Vector3(72, -10.81f, 0);
                        particles1.SetActive(true);
                        particles2.SetActive(true);                        
                    
                        // Hacemos el tercer movimiento del boton DeduceSign y la aparicion del boton puzzle
                        Vector3 endPosition = new(0, -10.81f, 0);
                        StartCoroutine(MoveDSButton(endPosition));

                        StartCoroutine(PuzzleAppearance());
                    }
                }
            }
        }
        else{
            // No se ha jugado a ningun juego nuevo asi que solo actualizamos la posicion correcta del boton de DeduceSign
            if(amountGamesPlayed == 1){
                buttonsDS.transform.localPosition = new Vector3(143.5f, -10.81f, 0);
                particles1.SetActive(true);
            }
            else{
                if(amountGamesPlayed == 2){
                    buttonsDS.transform.localPosition = new Vector3(72, -10.81f, 0);
                    particles1.SetActive(true);
                    particles2.SetActive(true);
                }
                else{
                    if(amountGamesPlayed == 3){
                        buttonsDS.transform.localPosition = new Vector3(0, -10.81f, 0);
                        buttonsPuzzle.transform.GetChild(0).gameObject.transform.localScale =  Vector3.one;
                        buttonsPuzzle.transform.GetChild(0).gameObject.SetActive(true);

                    }
                }
            }            
        }
    }

    private void ChangeVisibilityButtons(bool boolDesired){
        buttonsParking.SetActive(boolDesired);
        buttonsLR.SetActive(boolDesired);
        buttonsDS.SetActive(boolDesired);
        buttonsPuzzle.SetActive(boolDesired);
    }

    private void UpdateVariablesGameChecker(){
        amountGamesPlayed = GameChecker.Instance.GetAmountGamesPlayed();

        parkingPlayed = GameChecker.Instance.GetParkingPlayed();
        laneRacePlayed = GameChecker.Instance.GetLaneRacePlayed();
        deduceSignPlayed = GameChecker.Instance.GetDeduceSignPlayed();
        puzzlePlayed = GameChecker.Instance.GetPuzzlePlayed();
    }



    // CORRUTINAS
    IEnumerator MoveDSButton(Vector3 endPosition){
        // Tiempo que tarda el fade in
        yield return new WaitForSeconds(1.4f);

        // Cuerpo de la corrutina
        float elapsedTime = 0;
        float animationTime = 1;
        Vector3 startPosition = buttonsDS.transform.localPosition;

        if(OnMoveButtonDS != null)                          
            OnMoveButtonDS(); 
        while(elapsedTime < animationTime){
            
            buttonsDS.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        buttonsDS.transform.localPosition = endPosition;
    }

    IEnumerator PuzzleAppearance(){
        yield return new WaitForSeconds(2.5f); // Tiempo (1) que tarda el boton DS en moverse + 1.5 de cambio de escena

        particles2.SetActive(false);
        particles1.SetActive(false);
        
        GameObject firstButtonPuzzle = buttonsPuzzle.transform.GetChild(0).gameObject;
        firstButtonPuzzle.SetActive(true);     // Se inicia con la escala en x en 0

        float elapsedTime = 0;
        float animationTime = 1;

        if(OnFormulaAppearance != null)                          
            OnFormulaAppearance(); 

        while(elapsedTime < animationTime){
            float newScale = Mathf.Lerp(0, 1, elapsedTime / animationTime);
            
            firstButtonPuzzle.transform.localScale = new Vector3(newScale, 1, 1);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        firstButtonPuzzle.transform.localScale = new Vector3(1, 1, 1);
    }
    
     IEnumerator FadeCanvasGroup(GameObject view, float fromAlpha, float toAlpha, bool changeButtons = false, float animationTime = 0.5f){ 
        // Corrutina reutilizada parcialmente del Minijuego Parking

        CanvasGroup canvasGroup = view.GetComponent<CanvasGroup>();

        if(toAlpha > 0){
            // Queremos activar el panel asi que activamos el panel y desactivamos los botones de los minijuegos
            view.SetActive(true);
            if(changeButtons){
                ChangeVisibilityButtons(false);
            }
        }
        else{
            if(changeButtons){
                // Queremos desactivar el panel asi que volvemos a restaurar los botones
                ChangeVisibilityButtons(true);
            }
        }

        float elapsedTime = 0;
        while(elapsedTime <= animationTime){
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / animationTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return 0;
        }
        canvasGroup.alpha = toAlpha;

        if(toAlpha == 0)
            view.SetActive(false);
    }

}
