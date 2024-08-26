using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform lanesParent;
    public Transform[] lanes;
    
    public int currentIndex = 1;
    public float speed = 10f;

    private Transform currentLane;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Llenamos el array de carriles por codigo
        int totalLanes = lanesParent.childCount;
        lanes = new Transform[totalLanes];
        for(int i = 0 ; i < totalLanes ; i++){
            lanes[i] = lanesParent.GetChild(i);
        }

        currentLane = lanes[currentIndex];
    }


    void Update()
    {
        // Entradas del teclado
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            if(currentIndex > 0)
                currentIndex --;
            
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            if(currentIndex < lanesParent.childCount -1)
                currentIndex ++;
        }    

        // Actualizamos posicion
        currentLane = lanes[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentLane.position, speed * Time.deltaTime);


    }



    












}
