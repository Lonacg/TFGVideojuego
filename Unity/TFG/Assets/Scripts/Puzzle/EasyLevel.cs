using UnityEngine;

public class EasyLevel : PieceCheck
{

    void OnEnable()  // Cambiar a Awake despues de las pruebas
    {
        StartLevel();
    }

    public override void StartLevel()
    {
        base.StartLevel();   //     Ejecuta el StartLevel de PieceCheck

        levelWidth = 1.5f;
   
    }


}
