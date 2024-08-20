using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CarInstances : MonoBehaviour
{

    public GameObject[] cars;

    private GameObject parkingLot;
    string pLot = "ParkingLot";
    public List<GameObject> carPositions;



    public void intanciateCars(){

        // Asignamos las posiciones a la lista por codigo
        parkingLot = GameObject.Find(pLot);

        for (int i = 0 ; i < parkingLot.transform.childCount; i++ ){
            carPositions.Add(parkingLot.transform.GetChild(i).gameObject); 
        }

        
        // Instanciamos los prefabs
        foreach (var car in cars){
            // Lugar para instanciar
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

            // Eliminamos el GameObject de esa posicion y la quitamos de la lista
            Destroy(carPositions[i]);
            carPositions.RemoveAt(i);
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
