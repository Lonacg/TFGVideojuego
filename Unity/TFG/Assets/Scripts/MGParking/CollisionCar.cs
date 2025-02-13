using UnityEngine;

public class CollisionCar : MonoBehaviour
{
    public delegate void _OnCollisionCar();
    public static event _OnCollisionCar OnCollisionCar;
  


    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player")){
            if(OnCollisionCar != null)   
                OnCollisionCar();
        }
    }

}