using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GrannyMovement : MonoBehaviour
{



    public Animator animator;
    //public GameObject player;

    public float x = 0;
    public float y = 0;


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


        // Lanzamos la corrutina para que el personaje haga un Ready, Steady, go
        StartCoroutine(ReadySteadyGo());

    }

    // Update is called once per frame
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


        // Animaciones
        // x = 0;
        // y = 1;


        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);





    }



    IEnumerator ReadySteadyGo(){
        // player.GetComponent<GrannyMovement>().enabled = false;
        
        Debug.Log("Idle");
        yield return new WaitForSeconds(2);

        y = 0.25f;
        Debug.Log("Agachandome");
        yield return new WaitForSeconds(1);

        y = 0.5f;
        Debug.Log("Agachada");
        yield return new WaitForSeconds(2);

        //y = 0.75f;
        Debug.Log("Levantandome");
        //yield return new WaitForSeconds(0.5f);      

        y = 1;
        Debug.Log("Corriendo");
        


    }








}
