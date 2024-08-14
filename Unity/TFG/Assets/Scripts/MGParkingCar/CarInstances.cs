using System;
using System.Collections.Generic;
using UnityEngine;

public class CarInstances : MonoBehaviour
{

    public GameObject[] cars;

    public List<GameObject> carPositions;



    public void intanciateCars(){

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Instanciamos los vehiculos de atrezo
        foreach (var car in cars){
            // Lugar para instanciar
            int i = UnityEngine.Random.Range(0, carPositions.Count);
            GameObject placeToInstantiate = carPositions[i];

            // Rotacion del vehiculo
            int j = UnityEngine.Random.Range(0,2);
            int rotChosen;
            if(j == 0){
                rotChosen = 0;
            }
            else
                rotChosen = 180;

            // Instancia
            Instantiate(car, placeToInstantiate.transform.position, Quaternion.Euler(0, rotChosen, 0));

            // Eliminamos la posicion ocupada de la lista
            Destroy(carPositions[i]);
            carPositions.RemoveAt(i);



        }
            

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
