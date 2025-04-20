using UnityEngine;

public class EasyLevel : PieceCheck
{



    public override void StartLevel()
    {
        base.StartLevel();   // Llama al StartLevel de PieceCheck

        easyWidth = 1.5f;
   
    }


}
