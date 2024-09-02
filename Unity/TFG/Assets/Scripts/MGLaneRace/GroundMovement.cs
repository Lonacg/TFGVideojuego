using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{

    public delegate void _OnGo();
    public static event _OnGo OnGo;

    public float groundSpeed = 5 ;
    private float elapsedTime;
    private bool wantMove = false;

    void Start(){
        wantMove = false;
        StartCoroutine(WaitForXSeconds(3f));
    }

    void Update()
    {
        if(wantMove)
            transform.Translate(0, 0, Time.deltaTime * -groundSpeed );
        
    }

    IEnumerator WaitForXSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        if(OnGo != null)   
            OnGo();
        wantMove = true;

    }


}
