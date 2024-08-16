using UnityEngine;

public class ParkingTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
        
    

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other){
        if (other.tag=="Player"){
            Debug.Log ("Player esta dentro");
        }
        }



}
