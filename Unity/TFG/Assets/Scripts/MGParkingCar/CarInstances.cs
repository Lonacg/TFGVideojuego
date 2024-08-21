using System;
using System.Collections.Generic;
using UnityEngine;

public class CarInstances : MonoBehaviour
{

    public GameObject[] cars;
    public List<GameObject> carPositions;


    public GameObject parkingLot;
    //private string pLot = "ParkingLot";

    public GameObject parkingNumber;
    //private string pNum = "ParkingNumbers";
    public List<GameObject> pNumbers;



    public void intanciateCars(){

        // Opcional: pasar la referencia por codigo en vez de en el inspector (menos coste no haciendolo)
        //parkingLot = GameObject.Find(pLot);
        //parkingNumber = GameObject.Find(pNum);

        // Asignamos las posiciones a las listas por codigo
        for (int i = 0 ; i < parkingLot.transform.childCount; i++ ){
            carPositions.Add(parkingLot.transform.GetChild(i).gameObject); 
            pNumbers.Add(parkingNumber.transform.GetChild(i).gameObject);
        }

        
        // Instanciamos los prefabs
        foreach (var car in cars){
            // Lugar random para instanciar
            int i = UnityEngine.Random.Range(0, carPositions.Count);
            GameObject placeToInstantiate = carPositions[i];

            // Rotacion del vehiculo prefab
            int j = UnityEngine.Random.Range(0,2);
            int rotChosen;
            if(j == 0){
                rotChosen = 0;
            }
            else
                rotChosen = 180;

            // Instancia
            Instantiate(car, placeToInstantiate.transform.position, Quaternion.Euler(0, rotChosen, 0));

            // Eliminamos el Lot de esa posicion y lo quitamos de la lista
            Destroy(carPositions[i]);
            carPositions.RemoveAt(i);

            // Eliminamos ese numero de aparcamiento en ParkingNumbers
            Destroy(pNumbers[i]);
            pNumbers.RemoveAt(i);
        }
    }

    void Awake()
    {
        // Instanciamos los vehiculos de atrezo
        intanciateCars();
    }

    void Update()
    {
        
    }





}
