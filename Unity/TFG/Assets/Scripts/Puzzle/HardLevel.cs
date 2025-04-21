using UnityEngine;

public class HardLevel : PieceCheck
{
    void OnEnable()  // Cambiar a Awake despues de las pruebas
    {
        StartLevel();
    }

    public override void StartLevel()
    {
        base.StartLevel();   //     Ejecuta el StartLevel de PieceCheck

        levelWidth = 0.9f;
   
    }
}
