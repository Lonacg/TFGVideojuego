using UnityEngine;

public class MediumLevel : PieceCheck
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()  // Cambiar a Awake despues de las pruebas
    {
        StartLevel();
    }

    public override void StartLevel()
    {
        base.StartLevel();   // Ejecuta el StartLevel de PieceCheck

        levelWidth = 1.125f;
   
    }
}
