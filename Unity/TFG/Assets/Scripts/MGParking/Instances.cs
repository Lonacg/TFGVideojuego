using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class Instances : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject confetiParticles;
    [SerializeField] private GameObject xError;
    [SerializeField] private GameObject[] cars;     // Como minimo tenemos que tener 2 prefabs, para que haya al menos uno instanciado de cara y otro de culo
    [SerializeField] private GameObject parkingNumbers;
    //private string pNum = "ParkingNumbers";

    public List<GameObject> pLots;       // Necesita ser publica porque SetOperationParking accede a ella
    public List<GameObject> pNumbers;    // Necesita ser publica porque SetOperationParking accede a ella



    void OnEnable(){
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
        ParkingTrigger.OnWellParked += HandleOnWellParked;
    }

    void OnDisable(){
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
        ParkingTrigger.OnWellParked -= HandleOnWellParked;
    }



    private void HandleOnWrongParked(GameObject go){
        Instantiate(xError, go.transform.position,  Quaternion.Euler(0, 0, 0));
    }

    private void HandleOnWellParked(GameObject go){
        player.GetComponent<CarMovement>().enabled = false;
        LaunchFireworks();
    }



    void Awake(){
        IntanciateCars();
    }



    public void IntanciateCars(){

        // Opcional: asignar el objeto por codigo en vez de en el inspector (es menos coste como esta hecho, por evitar el .Find() )
        //parkingNumbers = GameObject.Find(pNum);

        // Asignamos las posiciones a las listas por codigo
        for(int i = 0 ; i < gameObject.transform.childCount; i++ ){  // gameObjet es parkingLots, que es el objeto dodne esta este script
            pLots.Add(gameObject.transform.GetChild(i).gameObject); 
            pNumbers.Add(parkingNumbers.transform.GetChild(i).gameObject);
        }

        // Instanciamos los prefabs
        for(int i = 0 ; i < cars.Length ; i++){
        //foreach(var car in cars){
            // Lugar random para instanciar
            int place = Random.Range(0, pLots.Count);
            GameObject placeToInstantiate = pLots[place];

            // Rotacion del vehiculo prefab. Los dos primeros los fijamos, para que siempre haya uno de cara y otro de culo, y el resto seran aleatorios
            int option = Random.Range(0,2);
            int rotChosen;
            if(i == 0){
                rotChosen = 0;
            }
            else{  
                if(i == 1){
                    rotChosen = 180;
                }
                else{
                    if(option == 0){
                        rotChosen = 0;
                    }
                    else{
                        rotChosen = 180;
                    }
                }
            }    

            // Instancia
            Instantiate(cars[i], placeToInstantiate.transform.position, Quaternion.Euler(0, rotChosen, 0));

            // Eliminamos el Lot de esa posicion y lo quitamos de la lista
            Destroy(pLots[place]);
            pLots.RemoveAt(place);

            // Eliminamos ese numero de aparcamiento en ParkingNumbers
            Destroy(pNumbers[place]);
            pNumbers.RemoveAt(place);
        }
    }



    private void LaunchFireworks(){
        Instantiate(confetiParticles, player.transform.position, Quaternion.identity);
    }

}
