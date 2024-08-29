using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    private int lengthGroundPiece = 30;
    private int numberOfPieces;
    private GameObject parent;

    public float groundSpeed = 5 ;


    void Start(){

        parent = transform.parent.gameObject;
        numberOfPieces = parent.transform.childCount;
    }


    void Update()
    {

        transform.Translate(0, 0, Time.deltaTime * -groundSpeed );
        
    }


    void OnTriggerEnter(Collider other){
        if (other.tag=="Player"){
            Vector3 newPosition = new Vector3(0, 0, lengthGroundPiece * numberOfPieces);
            gameObject.transform.position += newPosition;
        }
    }


}
