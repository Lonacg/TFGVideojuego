using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour
{
    [Header("Game Objects:")]

    [SerializeField] private GameObject buttonsDS;
    [SerializeField] private GameObject firstButtonParking;
    [SerializeField] private GameObject secondButtonParking;
    [SerializeField] private GameObject firstButtonLR;
    [SerializeField] private GameObject secondButtonLR;
    [SerializeField] private GameObject firstButtonDS;
    [SerializeField] private GameObject secondButtonDS;
    [SerializeField] private GameObject firstButtonPuzzle;
    [SerializeField] private GameObject secondButtonPuzzle;
    [SerializeField] private GameObject particles1;
    [SerializeField] private GameObject particles2;
    
    [Header("Variables from GameChecker:")]
    private int amountGamesPlayed;
    private bool alreadyPlayed;
    private bool parkingPlayed = false;
    private bool laneRacePlayed = false;
    private bool deduceSignPlayed = false;

    private bool puzzlePlayed= false;






    void Start()
    {
        
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


    private void UpdateButtons(){

        if(parkingPlayed){
            secondButtonParking.SetActive(true);
            firstButtonParking.SetActive(false);
        }
        if(laneRacePlayed){
            secondButtonLR.SetActive(true);
            firstButtonLR.SetActive(false);
        }
        if(deduceSignPlayed){
            secondButtonDS.SetActive(true);
            firstButtonDS.SetActive(false);
        }
        if(puzzlePlayed){
            secondButtonPuzzle.SetActive(true);
            firstButtonPuzzle.SetActive(false);
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
            if(amountGamesPlayed == 1)
                buttonsDS.transform.localPosition = new Vector3(143.5f, -10.81f, 0);
            else{
                if(amountGamesPlayed == 2)
                    buttonsDS.transform.localPosition = new Vector3(72, -10.81f, 0);
                else
                    if(amountGamesPlayed == 3){
                        buttonsDS.transform.localPosition = new Vector3(0, -10.81f, 0);
                        firstButtonPuzzle.SetActive(true);
                    }
                
            }            

        }
    }


    private void UpdateVariablesGameChecker(){
        amountGamesPlayed = GameChecker.Instance.GetAmountGamesPlayed();

        parkingPlayed = GameChecker.Instance.GetParkingPlayed();
        laneRacePlayed = GameChecker.Instance.GetLaneRacePlayed();
        deduceSignPlayed = GameChecker.Instance.GetDeduceSignPlayed();
        puzzlePlayed = GameChecker.Instance.GetPuzzlePlayed();
    }



    IEnumerator MoveDSButton(Vector3 endPosition){

        // Añadir brillitos

        // Tiempo que tarda el fade in
        yield return new WaitForSeconds(1.4f);

        // Cuerpo de la corrutina
        float elapsedTime = 0;
        float animationTime = 1;
        Vector3 startPosition = buttonsDS.transform.localPosition;
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
        
        firstButtonPuzzle.SetActive(true); // Se inicia con la escala en x en 0

        float elapsedTime = 0;
        float animationTime = 1;

        while(elapsedTime < animationTime){
            float newScale = Mathf.Lerp(0, 1, elapsedTime / animationTime);
            
            firstButtonPuzzle.transform.localScale = new Vector3(newScale, 1, 1);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        firstButtonPuzzle.transform.localScale = new Vector3(1, 1, 1);
    }

}


