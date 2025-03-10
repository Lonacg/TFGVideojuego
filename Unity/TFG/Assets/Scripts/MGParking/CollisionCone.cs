using UnityEngine;

public class CollisionCone : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnCollisionCone();
    public static event _OnCollisionCone OnCollisionCone;
  


    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("ImmobileCar")){
            if(OnCollisionCone != null)   
                OnCollisionCone();
        }
    }

}
