using UnityEngine;
using System.Collections;



public class HandAnimation : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [SerializeField] private GameObject backgroundStamp;
    [SerializeField] private GameObject stampMaxi;



    // DECLARACIÓN DE EVENTOS
    public delegate void _OnStampSound();          
    public static event _OnStampSound OnStampSound;



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public void OnMakeStamp(){
        // La animacion accede a ella mediante un evento interno
        if(OnStampSound != null)  
            OnStampSound();  
        StartCoroutine(WaitAndGoOut());
    }
    


    // CORRUTINAS
    IEnumerator WaitAndGoOut(){
        stampMaxi.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        gameObject.GetComponent<Animator>().SetTrigger("HandOut");

        backgroundStamp.SetActive(true); 
    }
    
}
