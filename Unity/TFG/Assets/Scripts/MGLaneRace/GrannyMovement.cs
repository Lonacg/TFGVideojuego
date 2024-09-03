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
    private bool canMove;

    void OnEnable(){
        TriggerFinalGate.OnFinalLine += HandleOnFinalLine;
        CanvasManager.OnStart += HandleOnStart;

    }

    void OnDisable(){
        TriggerFinalGate.OnFinalLine -= HandleOnFinalLine;
        CanvasManager.OnStart -= HandleOnStart;

    }


    public void HandleOnFinalLine(){
        StartCoroutine(Winning());
    }

    public void HandleOnStart(){
        // Animacion del personaje al empezar
        StartCoroutine(ReadySteadyGo());

    }


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

    }



    void Update()
    {
        if(canMove){
            // Entradas del teclado
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)){
                if(currentIndex > 0)
                    currentIndex --;
            }
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)){
                if(currentIndex < lanesParent.childCount -1)
                    currentIndex ++;
            }    
        }


        // Actualizamos posicion
        currentLane = lanes[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentLane.position, speed * Time.deltaTime);
    }




    public void ChangeAnimation(string animation, float crossfade = 0.2f){
        if(currentAnimation != animation){
            currentAnimation = animation;
            animator.CrossFade(animation, crossfade);
        }
    }

    IEnumerator ReadySteadyGo(){

        
        // Intro texto y camara
        ChangeAnimation("Idle");
        yield return new WaitForSeconds(1);


        // BORRAR ESTE DE ABAJO
        yield return new WaitForSeconds(8);

        // READY
        ChangeAnimation("Crouched");        
        yield return new WaitForSeconds(1);

        // STEADY (sigue esperando)
        yield return new WaitForSeconds(1);


        ChangeAnimation("Levantandose", 0);        
        yield return new WaitForSeconds(0.1f);

        // GO
        ChangeAnimation("FastRunning", 0.1f);  

        // Permitimos el movimiento del jugador
        canMove = true;
        
    }

    IEnumerator Winning(){
        // Impedimos el movimiento del jugador
        canMove = false;

        yield return new WaitForSeconds(1f);

        ChangeAnimation("Walking");        
        yield return new WaitForSeconds(1.5f);

        ChangeAnimation("StrongGesture"); 
        yield return new WaitForSeconds(1.5f);

        ChangeAnimation("Victory");  
        yield return new WaitForSeconds(1.5f);

        ChangeAnimation("Idle");     
    }


}
