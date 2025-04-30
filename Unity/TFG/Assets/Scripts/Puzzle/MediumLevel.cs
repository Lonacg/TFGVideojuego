using UnityEngine;



public class MediumLevel : PieceCheck
{
    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void OnEnable()  // Cambiar a Awake despues de las pruebas
    {
        StartLevel();
    }



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public override void StartLevel()
    {
        base.StartLevel();   // Ejecuta el StartLevel de PieceCheck

        levelWidth = 1.125f;
    }
}
