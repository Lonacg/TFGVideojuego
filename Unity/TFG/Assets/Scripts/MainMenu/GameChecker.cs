using UnityEngine;

public class GameChecker : MonoBehaviour
{

    private bool parkingYes = false;
    private bool laneRaceYes = false;
    private bool deduceSignYes = false;

    private int amountGamesPlayed = 0;

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



    public void ParkingPlayed(){
        if(!parkingYes){
            amountGamesPlayed ++;
        }
        parkingYes = true;
        
    }

    public void LaneRacePlayed(){
        if(!laneRaceYes){
            amountGamesPlayed ++;
        }
        laneRaceYes = true;

    }

    public void DeduceSignPlayed(){
        if(!deduceSignYes){
            amountGamesPlayed ++;
        }
        deduceSignYes = true;
    }


    public int GetAmountGamesPlayed(){
        // Metodo para que otro script acceda a este valor
        return amountGamesPlayed;
    }

}

