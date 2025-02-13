using UnityEngine;

public class CollisionCone : MonoBehaviour
{
    public delegate void _OnCollisionCone();
    public static event _OnCollisionCone OnCollisionCone;
  


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ImmobileCar")){
            if(OnCollisionCone != null)   
                OnCollisionCone();
        }
    }

}