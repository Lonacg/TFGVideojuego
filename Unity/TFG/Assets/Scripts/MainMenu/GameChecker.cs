using UnityEngine;

public class GameChecker : MonoBehaviour
{
    [Header("Bool:")]
    public bool parkingPlayed = false;
    public bool laneRacePlayed = false;
    public bool deduceSignPlayed = false;
    public bool puzzlePlayed = false;
    public bool alreadyPlayed = false;
    




    public int amountGamesPlayed = 0;




    public static GameChecker Instance { get; private set; } // Permitimos que se lea desde otros scripst pero solo se puede modificar desde este

    


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



    // Metodos para que otro script acceda a este valor
    public int GetAmountGamesPlayed(){
        // Metodo para que otro script acceda a este valor
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

