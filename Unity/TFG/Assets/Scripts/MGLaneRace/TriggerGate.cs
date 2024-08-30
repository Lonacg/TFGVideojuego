using UnityEngine;

public class TriggerGate : MonoBehaviour
{

    public delegate void _OnWellSol(GameObject go);
    public static event _OnWellSol OnWellSol;

    public delegate void _OnWrongSol(GameObject go);
    public static event _OnWrongSol OnWrongSol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            if(gameObject.tag == "CorrectGate"){
                if(OnWellSol != null)   
                    OnWellSol(gameObject);
            }
            if(gameObject.tag == "IncorrectGate"){
                if(OnWrongSol != null)   
                    OnWrongSol(gameObject);
            }

        }
    }
}


/*
    void OnEnable(){
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
        ParkingTrigger.OnWellParked += HandleOnWellParked;

    }

    void OnDisable(){
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
        ParkingTrigger.OnWellParked -= HandleOnWellParked;
    }
*/

