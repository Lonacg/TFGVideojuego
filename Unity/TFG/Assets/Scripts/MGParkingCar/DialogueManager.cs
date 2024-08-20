using UnityEngine;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI introDialogue;

    public string[] lines;

    public float textSpeed = 0.1f;
    int index;

    void Start()
    {
        introDialogue.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        // Si pulsamos la tecla espacio pasa a la siguiente linea o acaba de completar la frase
        if(Input.GetKeyDown(KeyCode.Space)){
            NextLine();
        }
        else{
            StopAllCoroutines();
            introDialogue.text = lines[index];
        }
    }


    public void StartDialogue(){
        index = 0;

        StartCoroutine(WriteLine());
    
    
    }


    IEnumerator WriteLine(){
        foreach (char letter in lines[index].ToCharArray()){
            introDialogue.text += letter;
            yield return new WaitForSeconds(textSpeed);
            NextLine();
        }

    }

    public void NextLine(){
        if (index < lines.Length -1){
            index ++;
            introDialogue.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else{
            gameObject.SetActive(false);
        }
    }


}
