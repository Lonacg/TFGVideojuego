using UnityEngine;

public class TriggerExtraTerrain : MonoBehaviour
{

    public delegate void _OnNewGround();
    public static event _OnNewGround OnNewGround;
  


    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            if(OnNewGround != null)   
                OnNewGround();
        }
    }
}