using System.Collections;
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
    private Animator animatorGranny;
    private string currentAnimationGranny = "";


    
    // Declaracion de eventos:
    public delegate void _OnHi();
    public static event _OnHi OnHi;
    public delegate void _OnReady();
    public static event _OnReady OnReady;

    public delegate void _OnSteady();
    public static event _OnSteady OnSteady;

    public delegate void _OnGo();
    public static event _OnGo OnGo;

    public delegate void _OnFootstepSound();
    public static event _OnFootstepSound OnFootstepSound;

    public delegate void _OnGotIt();
    public static event _OnGotIt OnGotIt;

    public delegate void _OnParty();
    public static event _OnGo OnParty;

    public delegate void _OnQuitGame();
    public static event _OnQuitGame OnQuitGame;



    void OnEnable()
    {
        CanvasManagerLaneRace.OnStart         += HandleOnStart;
        StageManagerLaneRace.OnMiddleVelocity += HandleOnMiddleVelocity;
        StageManagerLaneRace.OnLowVelocity    += HandleOnLowVelocity;
        StageManagerLaneRace.OnVictory        += HandleOnVictory;
        TriggerFinalGate.OnFinalLine          += HandleOnFinalLine;
    }

    void OnDisable()
    {
        CanvasManagerLaneRace.OnStart         -= HandleOnStart;
        StageManagerLaneRace.OnMiddleVelocity -= HandleOnMiddleVelocity;
        StageManagerLaneRace.OnLowVelocity    -= HandleOnLowVelocity;
        StageManagerLaneRace.OnVictory        -= HandleOnVictory;
        TriggerFinalGate.OnFinalLine          -= HandleOnFinalLine;
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

        // Asignamos los Animator
        animatorGranny = GetComponent<Animator>();

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
        ChangeAnimationGranny("MiddleRunning");
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(10f);
    }

    public void HandleOnLowVelocity(){
        // Cambiamos la animacion a correr despacio y reducimos las particulas de polvo de los pies
        ChangeAnimationGranny("SlowRunning");
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(5f);
    }

    public void HandleOnVictory(){
        // Movemos a player al carril central e impedimos su movimiento
        canMove= false;
        speed = 6f;
        ChangeAnimationGranny("FastRunning"); 

        // Aumentamos el valor de las particulas a 15 que es el que deseamos en FastRunning
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(15f);
 
        StartCoroutine(WaitAndMoveToCentralGate(seconds: 1.5f));
    }

    public void HandleOnFinalLine(){
        StartCoroutine(Winning());
    }



    public void FootstepDone(){
        if(OnFootstepSound != null)   
            OnFootstepSound();
    }

    public void ChangeAnimationGranny(string animation, float transitionTime = 0.25f){
        if(currentAnimationGranny != animation){
            currentAnimationGranny = animation;
            animatorGranny.CrossFade(animation, transitionTime);
        }
    }



    IEnumerator ReadySteadyGo(){
        //yield return new WaitForSeconds(1.5f);   // Tiempo que tarda en aparecer el fade in

        yield return new WaitForSeconds(1);        // Tiempo para que quede bien el saludo mientras se mueve la camara
        
        ChangeAnimationGranny("Greeting");
        yield return new WaitForSeconds(1.5f);        // Tiempo para que quede bien el sonido de Hi
        if(OnHi != null)   
            OnHi();
        yield return new WaitForSeconds(4.1f);


        // READY
        if(OnReady != null)   
            OnReady();
        ChangeAnimationGranny("Crouched");        
        yield return new WaitForSeconds(2f);

        // STEADY (sigue esperando)
        if(OnSteady != null)   
            OnSteady();
        yield return new WaitForSeconds(2f);

        // GO
        if(OnGo != null)   
            OnGo();
        yield return new WaitForSeconds(0.4f); // Para que aparezca el GO! justo antes de empezar el mov (GroundMovemetn tambien lo tiene en el HandleOnGo)
        ChangeAnimationGranny("FastRunning", transitionTime: 0.1f); 
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

        // Animacion de correr despacio, reducimos particulas y reducimos sonido
        ChangeAnimationGranny("SlowRunning");
        runningEmission.rateOverTime = new ParticleSystem.MinMaxCurve(5f);
        yield return new WaitForSeconds(1.5f);

        ChangeAnimationGranny("Walking");
        runningParticles.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        ChangeAnimationGranny("StrongGesture", transitionTime: 0.2f); 
        yield return new WaitForSeconds(0.2f);  // Para cuadran mejor el sonido de Tanan con el gesto de fuerza
        // Musica de conseguido
        if(OnGotIt != null)   
            OnGotIt();
        yield return new WaitForSeconds(1.5f);


        ChangeAnimationGranny("Victory", transitionTime: 0.2f);  
        yield return new WaitForSeconds(0.3f);
        if(OnParty != null)   
            OnParty();
        yield return new WaitForSeconds(1.2f);

        ChangeAnimationGranny("Idle");
        yield return new WaitForSeconds(0.2f);
        if(OnQuitGame != null)   
            OnQuitGame();
    }

}