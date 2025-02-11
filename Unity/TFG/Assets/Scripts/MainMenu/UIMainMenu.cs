using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{


    public void OnParkingButton(){
        SceneManager.LoadScene("Parking");
    }

    public void OnLaneRaceButton(){
        SceneManager.LoadScene("LaneRace");
    }
    
    public void OnDeduceSignButton(){
        SceneManager.LoadScene("DeduceSign");
    }
}
