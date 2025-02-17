using System.Collections;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class GrannyMovement : MonoBehaviour
{


    [Header("Movement:")]
    [SerializeField] private Transform lanesParent;
    [SerializeField] private Transform[] lanes;
    [SerializeField] private GameObject runningParticles;
    private ParticleSystem.EmissionModule runningEmission;  // El valor no se lee, pero si se modifica asi que no hacer caso a la indicacion
    private float speed = 8f; 
    private int currentIndex = 1;
    private bool canMove;
    private Transform currentLane;
    private Animator animator;
    private string currentAnimation = "";

    


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
        StageManagerLaneRace.OnMiddleVelocity += HandleOnMiddleVelocity;
        StageManagerLaneRace.OnLowVelocity += HandleOnLowVelocity;
        StageManagerLaneRace.OnVictory += HandleOnVictory;
        TriggerFinalGate.OnFinalLine += HandleOnFinalLine;
    }

    void OnDisable(){
        CanvasManager.OnStart -= HandleOnStart;
        StageManagerLaneRace.OnMiddleVelocity -= HandleOnMiddleVelocity;
        StageManagerLaneRace.OnLowVelocity -= HandleOnLowVelocity;
        StageManagerLaneRace.OnVictory -= HandleOnVictory;
        TriggerFinalGate.OnFinalLine -= HandleOnFinalLine;
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

        // Asignamos las variables
        speed = 8f;
        runningParticles.SetActive(false);
        runningEmission = runningParticles.GetComponent<ParticleSystem>().emission;

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





    public void HandleOnStart(){
        // Animacion del personaje al empezar
        StartCoroutine(ReadySteadyGo());
    }

    public void HandleOnMiddleVelocity(){
        // Cambiamos la animacion a correr normal y reducimos las particulas de polvo de los pies
        ChangeAnimation("Running");
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(10f);
    }

    public void HandleOnLowVelocity(){
        // Cambiamos la animacion a correr despacio y reducimos las particulas de polvo de los pies
        ChangeAnimation("SlowRunning");
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(5f);
    }

    public void HandleOnVictory(){
        // Movemos a player al carril central e impedimos su movimiento
        canMove= false;
        speed = 6f;
        ChangeAnimation("FastRunning"); 

        // Aumentamos el valor de las particulas a 15 que es el que deseamos en FastRunning
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(15f);
 

        StartCoroutine(WaitAndMoveToCentralGate(seconds: 1.5f));
    }

    public void HandleOnFinalLine(){
        StartCoroutine(Winning());
    }





    public void ChangeAnimation(string animation, float transitionTime = 0.25f){
        if(currentAnimation != animation){
            currentAnimation = animation;
            animator.CrossFade(animation, transitionTime);
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
        ChangeAnimation("FastRunning", transitionTime: 0.1f); 
        runningParticles.SetActive(true); 

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
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(5f);        
        yield return new WaitForSeconds(1.5f);

        ChangeAnimation("Walking");
        runningParticles.SetActive(false);        
        yield return new WaitForSeconds(1.5f);

        ChangeAnimation("StrongGesture", transitionTime: 0.2f); 
        yield return new WaitForSeconds(1.7f);


        ChangeAnimation("Victory", transitionTime: 0.2f);  
        yield return new WaitForSeconds(0.3f);
        if(OnParty != null)   
            OnParty();
        yield return new WaitForSeconds(1.2f);

        ChangeAnimation("Idle");     
    }
    
}
