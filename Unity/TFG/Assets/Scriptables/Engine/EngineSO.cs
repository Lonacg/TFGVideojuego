using UnityEngine;



[CreateAssetMenu(fileName ="Engine",menuName = "Scriptable/Engine")]

// ScriptableObject porque es solo un contenedor de datos

public class EngineSO : ScriptableObject 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed;
    public float maxSpeed;
    public float turnSpeed;
}
