using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public GameObject introPanel;
    public TextMeshProUGUI introDialogue;
    public GameObject operationPanel;

    //public List<string> lines;
    public string[] lines;
    public int totalLines = 2;

    public float textSpeed = 0.1f;
    private int index = 0;

    void Start()
    {   
        //string line1 = "¿Podrás aparcar en el lugar correcto?";
        //string line2 = "3... 2... 1.... ¡YA!";
        //lines.Add(line1);
        //lines.Add(line2);
        
         
        introDialogue.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        // Si pulsamos la tecla espacio pasa a la siguiente linea o acaba de completar la frase
        // if(Input.GetKeyDown(KeyCode.Space)){
        //     NextLine();
        // }
        // else{
        //     StopAllCoroutines();
        //     introDialogue.text = lines[index];
        // }
    }


    public void NextLine(){
        if (index < lines.Length){
            introDialogue.text = "";
            StartCoroutine(WriteLine());
        }
        else{
            operationPanel.SetActive(true);
            introPanel.SetActive(false);
        }
    }


    IEnumerator WriteLine(){
        foreach (char letter in lines[index].ToCharArray()){
            introDialogue.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        index ++;
        NextLine();
        

    }

    public void StartDialogue(){
        index = 0;

        StartCoroutine(WriteLine());
    
    }






}
