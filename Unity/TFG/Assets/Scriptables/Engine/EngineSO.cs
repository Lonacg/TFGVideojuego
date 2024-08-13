using UnityEngine;



[CreateAssetMenu(fileName ="Engine",menuName = "Scriptable/Engine")]

// ScriptableObject porque es solo un contenedor de datos

public class EngineSO : ScriptableObject 
{
    public float speed;
    public float maxSpeed;
    public float turnSpeed;
}
