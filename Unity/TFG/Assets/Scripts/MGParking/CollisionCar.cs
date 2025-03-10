using UnityEngine;

public class CollisionCar : MonoBehaviour
{
    // DECLARACIÓN DE EVENTOS
    public delegate void _OnCollisionCar();
    public static event _OnCollisionCar OnCollisionCar;
  

    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player")){
            if(OnCollisionCar != null)   
                OnCollisionCar();
        }
    }

}
