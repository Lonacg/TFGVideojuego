using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour
{

    private int amountGamesPlayed;

    void Awake()
    {
        amountGamesPlayed = GameChecker.Instance.GetAmountGamesPlayed();

        if(amountGamesPlayed == 1){
            StartCoroutine(FirstMove());
            Debug.Log("Primer movimiento");
        }
        else{
            if(amountGamesPlayed == 1){
                StartCoroutine(SecondMove());
                Debug.Log("Segundo movimiento");
            }
            else{
                // Cuando ya se han jugado los 3
                StartCoroutine(PuzzleAppearance());
                Debug.Log("APARICIOOOOOON");
            }

        
        }

        


    }


    IEnumerator FirstMove(){

        yield return 0;

    }
    IEnumerator SecondMove(){

        yield return 0;

    }

    IEnumerator PuzzleAppearance(){


        yield return 0;

    }

    




}


