using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GrannyMovement : MonoBehaviour
{


    // Para animaciones
    private Animator animator;
    private string currentAnimation = "";


    // Para movimiento
    public Transform lanesParent;
    public Transform[] lanes;
    
    public int currentIndex = 1;
    public float speed = 8f;

    private Transform currentLane;


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

        // Animacion del personaje al empezar
        StartCoroutine(ReadySteadyGo());

    }



    void Update()
    {
        // Entradas del teclado
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)){
            if(currentIndex > 0)
                currentIndex --;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)){
            if(currentIndex < lanesParent.childCount -1)
                currentIndex ++;
        }    

        // Actualizamos posicion
        currentLane = lanes[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentLane.position, speed * Time.deltaTime);
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
