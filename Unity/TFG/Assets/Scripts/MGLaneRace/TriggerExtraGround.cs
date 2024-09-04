using UnityEngine;

public class TriggerExtraGround : MonoBehaviour
{

    public delegate void _OnNewGround();
    public static event _OnNewGround OnNewGround;
  


    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            if(OnNewGround != null)   
                OnNewGround();

        }

    }

}

