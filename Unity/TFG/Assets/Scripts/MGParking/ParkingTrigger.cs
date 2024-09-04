using UnityEngine;

public class ParkingTrigger : MonoBehaviour
{
    private int parkingReferences = 0;
    public delegate void _OnWellParked(GameObject go);
    public static event _OnWellParked OnWellParked;


    public delegate void _OnWrongParked(GameObject go);
    public static event _OnWrongParked OnWrongParked;



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

            if( gameObject.CompareTag("CorrectAnswer") ){       
                if(OnWellParked != null)                          // Si hay alguien suscrito al evento, le envia la info
                    OnWellParked(gameObject);
            }
            else{  
                if(OnWrongParked != null)   
                    OnWrongParked(gameObject);

                // Destruimos este aparcamiento en el que ha fallado para que no vuelva a detectar los colliders
                Destroy(gameObject);    
            }            
        }
    }

}
