using System;
using UnityEngine;
using TMPro;



public class StageManagerLaneRace : MonoBehaviour
{

    
    public TextMeshProUGUI scoreText;
    private int score;

    void OnEnable(){
        TriggerGate.OnWellSol += HandleOnWellSol;

    }



    void OnDisable(){
        TriggerGate.OnWellSol -= HandleOnWellSol;

    }


    void HandleOnWellSol(GameObject goGate){
        score ++;
        scoreText.text = score + "/3";

        Vector3 gatePosition = goGate.transform.position;

        Debug.Log(gatePosition.x + ", " + gatePosition.y + ", " + gatePosition.z);
        

    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(score == 3){

        }
    }




    public void InstanciateFinishLine(){
        //            Instantiate(car, placeToInstantiate.transform.position, Quaternion.Euler(0, rotChosen, 0));

        

    }

}