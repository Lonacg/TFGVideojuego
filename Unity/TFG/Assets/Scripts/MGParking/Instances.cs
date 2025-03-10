using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class Instances : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject confettiParticles;
    [SerializeField] private GameObject xError;
    [SerializeField] private GameObject[] cars;     // Como minimo tenemos que tener 2 prefabs, para que haya al menos uno instanciado de cara y otro marcha atras
    [SerializeField] private GameObject parkingNumbers;
    //private string pNum = "ParkingNumbers";
    private bool inLowerParking;
    public List<GameObject> pLots;       // Necesita ser publica porque SetOperationParking accede a ella
    public List<GameObject> pNumbers;    // Necesita ser publica porque SetOperationParking accede a ella



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Awake()
    {
        InstanciateCars();
    }

    void OnEnable()
    {
        ParkingTrigger.OnWrongParked += HandleOnWrongParked;
        ParkingTrigger.OnWellParked  += HandleOnWellParked;
    }

    void OnDisable()
    {
        ParkingTrigger.OnWrongParked -= HandleOnWrongParked;
        ParkingTrigger.OnWellParked  -= HandleOnWellParked;
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnWrongParked(GameObject go){
        Instantiate(xError, go.transform.position,  Quaternion.Euler(0, 0, 0));
    }

    private void HandleOnWellParked(GameObject go){
        player.GetComponent<CarMovement>().enabled = false;
        LaunchConfetti();
    }



    // MÉTODOS DE ESTA CLASE
    public void InstanciateCars(){

        // Opcional: asignar el objeto por codigo en vez de en el inspector (es menos coste como esta hecho, por evitar el .Find() )
        //parkingNumbers = GameObject.Find(pNum);

        // Asignamos las posiciones a las listas por codigo
        for(int i = 0 ; i < gameObject.transform.childCount; i++ ){  // gameObject es parkingLots, que es el objeto donde esta este script
            pLots.Add(gameObject.transform.GetChild(i).gameObject); 
            pNumbers.Add(parkingNumbers.transform.GetChild(i).gameObject);
        }

        // Instanciamos los prefabs
        for(int intCar = 0 ; intCar < cars.Length ; intCar++){

            // Lugar random para instanciar
            int place = Random.Range(0, pLots.Count);
            GameObject placeToInstantiate = pLots[place];

            // Rotacion del vehiculo prefab. Los dos primeros los fijamos, para que siempre haya uno de cara y otro de culo segun si estan en el parking de arriba o en el de abajo, y el resto seran aleatorios
            int rotChosen = ChooseRotation(place, intCar);

            // Instancia
            Instantiate(cars[intCar], placeToInstantiate.transform.position, Quaternion.Euler(0, rotChosen, 0));

            // Eliminamos el Lot de esa posicion y lo quitamos de la lista
            Destroy(pLots[place]);
            pLots.RemoveAt(place);

            // Eliminamos ese numero de aparcamiento en ParkingNumbers
            Destroy(pNumbers[place]);
            pNumbers.RemoveAt(place);
        }
    }

    private int ChooseRotation(int place, int intCar){
        int option = Random.Range(0,2);
        int rotChosen;

        // Primer coche (el blanco) aparcado marcha atras
        if(intCar == 0){     
            // El angulo del modelo depende de si el coche esta en los parkings inferiores (place del 0 al 3) o en los superiores (place del 4 al 9)
            if(place <= 3){
                // Esta en los parkings inferiores
                rotChosen = 0;
                inLowerParking = true;
            }
            else{
                // Esta en los parking superiores
                rotChosen = 180;
                inLowerParking = false;
            }
        }
        else{
            // Segundo coche (el verde) aparcado de cara
            if(intCar == 1){  
                if(inLowerParking){
                    // El primer coche se ha aparcado en los de abajo y se ha eliminado esa plaza, asi que ahora los parking de abajo libres van del 0 al 2 y los de arriba del 3 al 8
                    if(place <= 2){
                        // Esta en los parkings inferiores
                        rotChosen = 180;
                    }
                    else{
                        // Esta en los parking superiores
                        rotChosen = 0;
                    }                        
                
                }
                else{
                    // El primer coche se ha aparcado en los de arriba y se ha eliminado esa plaza, asi que ahora los parking de abajo libres van del 0 al 3 y los de arriba del 4 al 8
                    if(place <= 3){
                        // Esta en los parkings inferiores
                        rotChosen = 180;
                    }
                    else{
                        // Esta en los parking superiores
                        rotChosen = 0;
                    }   
                }
            } 
            else{   
                // Si no es ni el primero ni el segundo coche, entonces le asignamos la orientacion random que haya salido
                if(option == 0){
                    rotChosen = 0;
                }
                else{
                    rotChosen = 180;
                }
            }
        }

        return rotChosen;
    }

    private void LaunchConfetti(){
        Instantiate(confettiParticles, player.transform.position, Quaternion.identity);
    }

}
