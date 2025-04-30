using UnityEngine;



public class GameChecker : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES (todas deben ser publicas porque estaa clase es un SINGLETON y el resto accede a ellas)
    [Header("Bool:")]
    public bool parkingPlayed = false;
    public bool laneRacePlayed = false;
    public bool deduceSignPlayed = false;
    public bool puzzlePlayed = false;
    public bool alreadyPlayed = false;
    public int amountGamesPlayed = 0;



    // DECLARACIÓN DEL SINGLETON
    public static GameChecker Instance { get; private set; } // Permitimos que se lea desde otros scripst pero solo se puede modificar desde este

    

    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Awake()
    {
        // Declaramos la instancia y nos aseguramos de que solo haya una
        if(Instance != null && Instance != this){
            Destroy(this);
        }
        else{
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public void ParkingOnPlay(){
        if(!parkingPlayed){
            amountGamesPlayed ++;
            parkingPlayed = true;
            alreadyPlayed = false;
        }
        else{
            alreadyPlayed = true;
        }        
    }

    public void LaneRaceOnPlay(){
        if(!laneRacePlayed){
            amountGamesPlayed ++;
            laneRacePlayed = true;
            alreadyPlayed = false;
        }
        else{
            alreadyPlayed = true;
        }
    }

    public void DeduceSignOnPlay(){
        if(!deduceSignPlayed){
            amountGamesPlayed ++;
            deduceSignPlayed = true;
            alreadyPlayed = false;
        }
        else{
            alreadyPlayed = true;
        }        
    }

    public void PuzzleOnPlay(){
        puzzlePlayed = true;
    }

    public void RestartGame(){
        parkingPlayed = false;
        laneRacePlayed = false;
        deduceSignPlayed = false;
        puzzlePlayed = false;
        alreadyPlayed = false;
        amountGamesPlayed = 0;
    }

    // Metodos para que otro script acceda a este valor
    public int GetAmountGamesPlayed(){
        return amountGamesPlayed;
    }

    public bool GetParkingPlayed(){
        return parkingPlayed;
    }

    public bool GetLaneRacePlayed(){
        return laneRacePlayed;
    }

    public bool GetDeduceSignPlayed(){
        return deduceSignPlayed;
    }

    public bool GetPuzzlePlayed(){
        return puzzlePlayed;
    }

    public bool GetAlreadyPlayed(){
        return alreadyPlayed;
    }

}

