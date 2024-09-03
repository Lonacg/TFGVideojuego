using UnityEngine;

public class NextOperationTrigger : MonoBehaviour
{
    public delegate void _OnNextOperation();
    public static event _OnNextOperation OnNextOperation;
  


    void OnTriggerEnter(Collider other){
        if (other.tag == "Player")
            if(OnNextOperation != null)   
                OnNextOperation();
    }

}
