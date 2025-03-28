using UnityEngine;

public class ParkingTrigger : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    private int parkingReferences = 0; // Variable para contar los collider (sensores de aparcamiento) en los que esta dentro player



    // DECLARACIÓN DE EVENTOS
    public delegate void _OnWellParked(GameObject go);
    public static event _OnWellParked OnWellParked;

    public delegate void _OnWrongParked(GameObject go);
    public static event _OnWrongParked OnWrongParked;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player"){
            parkingReferences ++;
            CheckParking();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player"){
            parkingReferences --;
            CheckParking();
        }
    }



    // MÉTODOS DE ESTA CLASE
    public void CheckParking(){
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
