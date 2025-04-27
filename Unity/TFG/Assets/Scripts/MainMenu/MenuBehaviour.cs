using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour
{
    [Header("Game Objects:")]
    [SerializeField] private GameObject firstButtonParking;
    [SerializeField] private GameObject secondButtonParking;
    [SerializeField] private GameObject firstButtonLR;
    [SerializeField] private GameObject secondButtonLR;
    [SerializeField] private GameObject firstButtonDS;
    [SerializeField] private GameObject secondButtonDS;
    [SerializeField] private GameObject firstButtonPuzzle;
    [SerializeField] private GameObject secondButtonPuzzle;
    
    [Header("Variables from GameChecker:")]
    private int amountGamesPlayed;
    private bool alreadyPlayed;
    private bool parkingPlayed = false;
    private bool laneRacePlayed = false;
    private bool deduceSignPlayed = false;

    private bool puzzlePlayed= false;


    //[Header("Variables:")]
    // // ESTAS VARIABLES NO VALEN PORQUE NO SOBREVIVEN AL CAMBIO DE ESCENA
    // private bool puzzleReady = false;
    // private bool parkingChanged = false;    
    // private bool laneRaceChanged = false;
    // private bool deduceSignChanged = false;
    // private bool puzzleChanged = false;





    void Start()
    {
        

        UpdateVariablesGameChecker();

        // Gestion del movimiento del ultimo boton y cambio de los botones
        if(!puzzlePlayed){
            Debug.Log("Entrando a puzzle no jugado");
            UpdateButtons();

            UpdateMovementDS();
        }
        else{
            if(!secondButtonPuzzle.activeSelf){
                StartCoroutine(ChangeButton(firstButtonPuzzle, secondButtonPuzzle));
            }
        }

    }


    private void UpdateButtons(){
        Debug.Log("Actualizacion de botones");
        if(parkingPlayed && !secondButtonParking.activeSelf){
            StartCoroutine(ChangeButton(firstButtonParking, secondButtonParking));
        }
        if(laneRacePlayed && !secondButtonLR.activeSelf){
            StartCoroutine(ChangeButton(firstButtonLR, secondButtonLR));
        }
        if(deduceSignPlayed && !secondButtonDS.activeSelf){
            StartCoroutine(ChangeButton(firstButtonDS, secondButtonDS));
        }
        
    }

    private void UpdateMovementDS(){
        Debug.Log("Actualizacion de movimiento");

        alreadyPlayed = GameChecker.Instance.GetBool(alreadyPlayed);

        if(amountGamesPlayed == 1 && !alreadyPlayed){
            
            Vector3 endPosition = transform.position;
            float newX = endPosition.x - 71.5f;
            endPosition.x = newX;

            StartCoroutine(MoveDSButton(startPosition: transform.position, endPosition));

        }
        else{
            if(amountGamesPlayed == 2 && !alreadyPlayed){
                // Segundo movimiento del boton DeduceSign
                Vector3 endPosition = transform.position;
                float newX = endPosition.x - 71.5f;
                endPosition.x = newX;

                StartCoroutine(MoveDSButton(startPosition: transform.position, endPosition));
            }
            else{
                if(amountGamesPlayed == 3){
                    // Tercer movimiento del boton DeduceSign y aparicion del boton puzzle
                    Vector3 endPosition = transform.position;
                    endPosition.x = 0;
                    StartCoroutine(MoveDSButton(startPosition: transform.position, endPosition));

                    StartCoroutine(PuzzleAppearance());
                }
            }
        }
    }


    private void UpdateVariablesGameChecker(){
        amountGamesPlayed = GameChecker.Instance.GetAmountGamesPlayed();

        parkingPlayed = GameChecker.Instance.GetBool(parkingPlayed);
        laneRacePlayed = GameChecker.Instance.GetBool(laneRacePlayed);
        deduceSignPlayed = GameChecker.Instance.GetBool(deduceSignPlayed);
        puzzlePlayed = GameChecker.Instance.GetBool(puzzlePlayed);
    }



// EN LAS CORRUTINAS HAY QUE COMPROBAR SI EE MOVIMIENTO YA SE HA HECHO, IGUAL ACCEDIENDO A L BOLL EN EL IF
    IEnumerator MoveDSButton(Vector3 startPosition, Vector3 endPosition){

        Debug.Log("Moviendo DS");
        // Añadir brillitos


        float elapsedTime = 0;
        float animationTime = 1;
        while(elapsedTime < animationTime){
            
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.position = endPosition;
    }


    IEnumerator SecondMove(){
        // Añadir mas brillitos
        yield return 0;

    }

    IEnumerator PuzzleAppearance(){
        firstButtonPuzzle.SetActive(true);

        yield return 0;

    }
    IEnumerator ChangeButton(GameObject firstButton, GameObject secondButton){
        Debug.Log("cambiando boton");
        secondButton.SetActive(true);
        firstButton.SetActive(false);

        yield return 0;

    }

    




}


