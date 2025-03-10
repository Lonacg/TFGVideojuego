using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    private int lengthGroundPiece = 30;
    private int numberOfPieces;
    private GameObject parent;
    private GameObject childrenGates;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Start()
    {
        lengthGroundPiece = 30; 

        // Pasamos por codigo las referencias al padre y al hijo
        parent = transform.parent.gameObject;
        numberOfPieces = parent.transform.childCount - 1;       // -1 porque el terreno tambien es hijo de Ground, pero no es una pieza que se mueve

        childrenGates = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        // Cuando se lanza es porque ya no se ve este suelo
        if (other.tag=="Player"){
            Vector3 newPosition = new Vector3(0, 0, lengthGroundPiece * numberOfPieces);

            // Movemos esa porcion del suelo hacia atras
            gameObject.transform.position += newPosition;

            // Activamos las puertas para que se genere una nueva operacion
            childrenGates.SetActive(false);
            childrenGates.SetActive(true);
        }
    }

}
