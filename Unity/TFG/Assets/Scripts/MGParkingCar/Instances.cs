using System.Collections.Generic;
using UnityEngine;


public class Instances : MonoBehaviour
{

    public GameObject[] cars;

    public GameObject parkingLot;
    //private string pLot = "ParkingLot";

    public GameObject parkingNumber;
    //private string pNum = "ParkingNumbers";
    public List<GameObject> pLots;
    public List<GameObject> pNumbers;


    void OnEnable(){
        ParkingTrigger.OnWellParked += HandleOnWrongParked;
    }

    void OnDisable(){
        ParkingTrigger.OnWellParked -= HandleOnWrongParked;
    }

    private void HandleOnWrongParked(GameObject go)
    {
        //  Instanciar X de pequeño a grande y de grande a un poco mas pequeño

    }

  

    public void intanciateCars(){

        // Opcional: pasar la referencia por codigo en vez de en el inspector (menos coste no haciendolo)
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

    void Awake()
    {
        // Instanciamos los vehiculos de atrezo

        // Opcional: pasar la referencia por codigo en vez de en el inspector (menos coste no haciendolo)
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
    void Update()
    {
        
    }





}
