using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class Instances : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject confetiParticles;
    [SerializeField] private GameObject xError;
    [SerializeField] private GameObject[] cars;

    [SerializeField] private GameObject parkingLot;
    //private string pLot = "ParkingLot";

    [SerializeField] private GameObject parkingNumber;
    //private string pNum = "ParkingNumbers";

    public List<GameObject> pLots;      // Necesita ser publica porque SetOperationParking accede a ella
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
        StartCoroutine(LaunchFireworks());
    }



    void Awake(){
        IntanciateCars();
    }



    public void IntanciateCars(){

        // Opcional: pasar la referencia por codigo en vez de en el inspector (menos coste como esta hecho)
        //parkingLot = GameObject.Find(pLot);
        //parkingNumber = GameObject.Find(pNum);

        // Asignamos las posiciones a las listas por codigo
        for (int i = 0 ; i < parkingLot.transform.childCount; i++ ){
            pLots.Add(parkingLot.transform.GetChild(i).gameObject); 
            pNumbers.Add(parkingNumber.transform.GetChild(i).gameObject);
        }

        // Instanciamos los prefabs
        foreach (var car in cars){
            // Lugar random para instanciar
            int i = Random.Range(0, pLots.Count);
            GameObject placeToInstantiate = pLots[i];

            // Rotacion del vehiculo prefab
            int j = Random.Range(0,2);
            int rotChosen;
            if(j == 0){
                rotChosen = 0;
            }
            else
                rotChosen = 180;

            // Instancia
            Instantiate(car, placeToInstantiate.transform.position, Quaternion.Euler(0, rotChosen, 0));

            // Eliminamos el Lot de esa posicion y lo quitamos de la lista
            Destroy(pLots[i]);
            pLots.RemoveAt(i);

            // Eliminamos ese numero de aparcamiento en ParkingNumbers
            Destroy(pNumbers[i]);
            pNumbers.RemoveAt(i);
        }
    }



    IEnumerator LaunchFireworks(){
        Instantiate(confetiParticles, player.transform.position, Quaternion.identity);

        yield return 0;             
    }

}
