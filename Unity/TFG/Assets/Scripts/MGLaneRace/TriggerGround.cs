using UnityEngine;

public class TriggerGround : MonoBehaviour
{
    private int lengthGroundPiece = 30;
    private int numberOfPieces;
    private GameObject parent;




    void Start(){

        parent = transform.parent.gameObject;
        numberOfPieces = parent.transform.childCount;
    }



    void OnTriggerEnter(Collider other){
        if (other.tag=="Player"){
            Vector3 newPosition = new Vector3(0, 0, lengthGroundPiece * numberOfPieces);
            gameObject.transform.position += newPosition;
        }
    }
}
