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
    public bool canMove;
    private Transform currentLane;
    


    public delegate void _OnReady();
    public static event _OnReady OnReady;

    public delegate void _OnSteady();
    public static event _OnSteady OnSteady;

    public delegate void _OnGo();
    public static event _OnGo OnGo;

    public delegate void _OnParty();
    public static event _OnGo OnParty;



    void OnEnable(){
        CanvasManager.OnStart += HandleOnStart;
        StageManagerLaneRace.OnVictory += HandleOnVictory;
        TriggerFinalGate.OnFinalLine += HandleOnFinalLine;
    }

    void OnDisable(){
        CanvasManager.OnStart -= HandleOnStart;
        StageManagerLaneRace.OnVictory -= HandleOnVictory;
        TriggerFinalGate.OnFinalLine -= HandleOnFinalLine;
    }



    public void HandleOnStart(){
        // Animacion del personaje al empezar
        StartCoroutine(ReadySteadyGo());
    }

    public void HandleOnVictory(){
        // Movemos a player al carril central e impedimos su movimiento
        canMove= false;
        speed = 6f;
        ChangeAnimation("FastRunning"); 

        StartCoroutine(WaitAndMoveToCentralGate(seconds: 1.5f));
    }

    public void HandleOnFinalLine(){
        StartCoroutine(Winning());
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

        // Impedimos que player se mueva hasta que acaben las animaciones del inicio
        canMove = false;
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
        yield return new WaitForSeconds(1);
        
        ChangeAnimation("Greeting");
        yield return new WaitForSeconds(5.1f);


        // READY
        if(OnReady != null)   
            OnReady();
        ChangeAnimation("Crouched");        
        yield return new WaitForSeconds(2f);

        // STEADY (sigue esperando)
        if(OnSteady != null)   
            OnSteady();
        yield return new WaitForSeconds(2f);

        // GO
        if(OnGo != null)   
            OnGo();
        yield return new WaitForSeconds(0.4f); // Para que aparezca el GO! justo antes de empezar el mov (GroundMovemetn tambien lo tiene en el HandleOnGo)
        ChangeAnimation("FastRunning");  

        // Permitimos el movimiento del jugador
        canMove = true;
    }

    IEnumerator WaitAndMoveToCentralGate(float seconds){
        yield return new WaitForSeconds(seconds);
        currentIndex = 1;
    }

    IEnumerator Winning(){
        // Impedimos el movimiento del jugador
        canMove = false;

        ChangeAnimation("SlowRunning");        
        yield return new WaitForSeconds(1.5f);

        ChangeAnimation("Walking");        
        yield return new WaitForSeconds(1.5f);

        ChangeAnimation("StrongGesture"); 
        yield return new WaitForSeconds(1.7f);


        ChangeAnimation("Victory");  
        yield return new WaitForSeconds(0.3f);
        if(OnParty != null)   
            OnParty();
        yield return new WaitForSeconds(1.2f);

        ChangeAnimation("Idle");     
    }
    
}
