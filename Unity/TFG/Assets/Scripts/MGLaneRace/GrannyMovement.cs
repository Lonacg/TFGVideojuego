using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GrannyMovement : MonoBehaviour
{


    // Para animaciones
    private Animator animator;
    private Rigidbody rb;
    private Vector2 movement = Vector2.zero;
    private string currentAnimation = "";
    private int currentRSG = 0; // Current Ready/Steady/Go animacion




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

        // Asignamos el Animator y el Rigidbody
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();


        // Animacion del personaje al empezar
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

        // animator.SetFloat("VelX", x);
        // animator.SetFloat("VelY", y);

        //StartCoroutine(WaitForXSeconds(2, newY: 1));

        //CheckAnimation();

    }

    private void ChangeAnimation(string animation, float crossfade = 0.2f){
        if(currentAnimation != animation){
            currentAnimation = animation;
            animator.CrossFade(animation, crossfade);
        }
    }

    // private void CheckAnimation(){
    //     if(movement.y == 1){
    //         ChangeAnimation("Run");
    //     }
    //     else{
    //         // AQUI HABRIA QUE DISTINGUIR SI ESTA PARADO AL PRINCIPIO O AL FINAL, PARA LA ANIMACION DE VISTORIA
    //         ChangeAnimation("Idle");
    //     }
    // }


    // IEnumerator WaitForXSeconds(float seconds, float newY){
    //     yield return new WaitForSeconds(seconds);
    //     movement.y = newY;
    // }

    IEnumerator ReadySteadyGo(){

        // Intro texto y camara
        ChangeAnimation("Idle");
        yield return new WaitForSeconds(1);

        // READY
        ChangeAnimation("Crouched");        
        yield return new WaitForSeconds(1);

        // STEADY (sigue esperando)
        yield return new WaitForSeconds(1);

        // GO
        ChangeAnimation("Running");  
        
    }


}
