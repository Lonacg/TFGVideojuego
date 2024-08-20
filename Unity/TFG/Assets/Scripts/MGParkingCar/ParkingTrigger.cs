using UnityEngine;

public class ParkingTrigger : MonoBehaviour
{
    private int parkingReferences = 0;
    public delegate void _OnWellParked(GameObject go);
    public static event _OnWellParked OnWellParked;


    void OnTriggerEnter(Collider other){
        if (other.tag=="Player"){
            parkingReferences ++;
            checkParking();

        }
    }

    void OnTriggerExit(Collider other){
        if (other.tag=="Player"){
            parkingReferences --;
            checkParking();
        }
    }

    public void checkParking(){
        if(parkingReferences == 4){
            // Si hay alguien suscrito al evento, le envia la info
            if( OnWellParked != null)   
                OnWellParked(gameObject);
            
            // Aqui, habria que comprobar el tag que tenga el aparcamiento, para saber si es la solucion correcta o no
            Debug.Log("Conseguido! Está bien aparcado");
        }
    }

    /* 
    // En la ui, para suscribirse al evento hay que poner:
    void OnEnable(){
        ParkingTrigger.OnWellParked += HandleOnWellParked;
    }

    void OnDisable(){
        ParkingTrigger.OnWellParked -= HandleOnWellParked;
    }


    void HandleOnWellParked (GameObject go){
        
        
    }
    */

}
