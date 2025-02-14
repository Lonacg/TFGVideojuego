using UnityEngine;

public class TriggerIncreaseCurrentGround : MonoBehaviour
{
    public delegate void _OnIncreaseCurrentGround();
    public static event _OnIncreaseCurrentGround OnIncreaseCurrentGround;
  


    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
            if(OnIncreaseCurrentGround != null)   
                OnIncreaseCurrentGround();
    }
}
