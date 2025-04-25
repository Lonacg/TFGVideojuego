using UnityEngine;
using System.Collections;

public class HandAnimation : MonoBehaviour
{

    [SerializeField] private GameObject backgroundStamp;
    [SerializeField] private GameObject stampMaxi;


    public delegate void _OnStampSound();          
    public static event _OnStampSound OnStampSound;

    public void OnMakeStamp(){
    
        if(OnStampSound != null)  
            OnStampSound();  
        StartCoroutine(WaitAndGoOut());
    }
    
    IEnumerator WaitAndGoOut(){
        stampMaxi.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        gameObject.GetComponent<Animator>().SetTrigger("HandOut");

        //yield return new WaitForSeconds(1f);
        backgroundStamp.SetActive(true); 
    }
    
}
