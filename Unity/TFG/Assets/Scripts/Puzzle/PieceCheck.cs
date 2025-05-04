using System.Collections;
using UnityEngine;



public class PieceCheck : MonoBehaviour
{
    // DECLARACIÓN DE ELEMENTOS GLOBALES
    [Header("Tiles Width:")]
    protected float levelWidth; 

    [Header("Vectors:")]
    private Vector2 rayOriginUp;
    private Vector2 rayOriginDown;
    private Vector2 rayOriginRight;
    private Vector2 rayOriginLeft;
    [Header("Variables:")]
    private bool movingThisPiece = false;       // Bool para que una misma pieza no se pulse dos veces seguidas y reproduzca varias veces su movimiento
    private bool movingSomePiece= false;        // Bool para que otras piezas no puedan moverse hasta que esta este terminando
    [Header("Game Objects:")]
    [SerializeField] private GameObject stageManager;
    private StageManagerPuzzle scriptStageManager;



    // DECLARACIÓN DE EVENTOS
    public delegate void _OnMovingSomePiece();
    public static event _OnMovingSomePiece OnMovingSomePiece;

    public delegate void _OnStartingMovement();
    public static event _OnStartingMovement OnStartingMovement;

    public delegate void _OnShakePiece();
    public static event _OnShakePiece OnShakePiece;
    public delegate void _OnMoveMade(GameObject pieceMoved);
    public static event _OnMoveMade OnMoveMade;



    // MÉTODOS HEREDADOS DE MONOBEHAVIOUR
    void Awake()
    {
        scriptStageManager = stageManager.GetComponent<StageManagerPuzzle>();
        movingThisPiece = false;
    }

    void OnEnable()
    {
        PuzzleCheck.OnGotIt += HandleOnGotIt;
    }

    void OnDisable()
    {
        PuzzleCheck.OnGotIt -= HandleOnGotIt;
    }

    void Update()
    {      
        //ShowRaycast();
    }

    private void OnMouseDown()
    {
        movingSomePiece = scriptStageManager.movingSomePiece;

        if(!movingSomePiece && !movingThisPiece){ 
            movingThisPiece = true;
            PieceMovement();
        }
    }



    // MÉTODOS EN RESPUESTA A EVENTOS
    private void HandleOnGotIt(){
        // Impedimos que se pulsen mas piezas
        movingThisPiece = true;
    }



    // MÉTODOS ESPEFICICOS DE ESTA CLASE
    public virtual void StartLevel(){
        // Se sobreescribe en cada clase que hereda de esta
    }

    private void PieceMovement(){
        
        // Creamos un rayo para cada direccion y detectamos si está colisionando con algo, para saber si tiene la pieza libre al lado o no
        UpdateRayOrigin();
        float rayOffset = levelWidth / 2;
        RaycastHit2D upHit = Physics2D.Raycast(rayOriginUp, Vector2.up, rayOffset);
        RaycastHit2D downHit = Physics2D.Raycast(rayOriginDown, Vector2.down, rayOffset);
        RaycastHit2D rightHit = Physics2D.Raycast(rayOriginRight, Vector2.right, rayOffset);
        RaycastHit2D leftHit = Physics2D.Raycast(rayOriginLeft, Vector2.left, rayOffset);
    
        if(upHit && downHit && rightHit && leftHit){
            // Choca con todos asi que no hay ningun hueco libre contiguo
            StartCoroutine(ShakePiece());
        }
        else{
            // Hay un hueco libre luego comprobamos cual es, movemos la pieza a ese lugar y lanzamos el evento a StageManager para que cambie su bool movingSomePiece a true
            if(OnMovingSomePiece != null)                          
                OnMovingSomePiece();

            if(!upHit){
                // Arriba esta libre
                StartCoroutine(MovePiece(Vector3.up));
            }
            else{
                if(!downHit){
                    // Abajo esta libre
                    StartCoroutine(MovePiece(Vector3.down));
                }
                else{
                    if(!rightHit){
                        // A la derecha esta libre
                        StartCoroutine(MovePiece(Vector3.right));
                    }
                    else{
                        // A la izquieda esta libre
                        StartCoroutine(MovePiece(Vector3.left));
                    }
                }
            }
        }
    }

    private void UpdateRayOrigin(){
        rayOriginUp = (Vector2)transform.position + Vector2.up * levelWidth;
        rayOriginDown = (Vector2)transform.position + Vector2.down * levelWidth;
        rayOriginRight = (Vector2)transform.position + Vector2.right * levelWidth;
        rayOriginLeft = (Vector2)transform.position + Vector2.left * levelWidth;
    }

    private void ShowRaycast(){
        UpdateRayOrigin();
        float rayOffset = levelWidth;
        Debug.DrawLine(rayOriginUp, rayOriginUp + Vector2.up * rayOffset, Color.magenta);
        Debug.DrawLine(rayOriginDown, rayOriginDown + Vector2.down * rayOffset, Color.yellow);
        Debug.DrawLine(rayOriginRight, rayOriginRight + Vector2.right * rayOffset, Color.blue);
        Debug.DrawLine(rayOriginLeft, rayOriginLeft + Vector2.left * rayOffset, Color.green);
    }

 

    // CORRUTINAS
    IEnumerator MovePiece(Vector3 direction){
        // Notificamos para aumentar el contador de movimientos y reproducir el sonido
        if(OnStartingMovement != null)                          
            OnStartingMovement();

        // Corrutina de movimiento
        float elapsedTime = 0;
        float animationTime = 0.4f;
        bool notified = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + 2 * levelWidth * direction;
        while(elapsedTime < animationTime){
            
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

            // Cuando ya ha hecho mas de dos tercios del recorrido, lanzamos el evento para que puedan moverse otras piezas, ya que lo que falta de reorrido no afecta (el rayo de deteccion sale en la mediatriz de cada arista)
            if(!notified && elapsedTime > animationTime * 2 / 3 && OnMovingSomePiece != null){
                OnMovingSomePiece();
                notified = true;
            }                          
                
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.position = endPosition;
        
        movingThisPiece = false;

        if(OnMoveMade != null)                          
            OnMoveMade(gameObject);        
    }

    IEnumerator ShakePiece(){
        // Notificamos para reproducir el sonido
        if(OnShakePiece != null)                          
            OnShakePiece();

        Vector3 originalPosition = transform.position;
        Vector3 startPosition;
        Vector3[] directions = new Vector3[]{Vector3.up, Vector3.right, Vector3.down, Vector3.left, Vector3.up, Vector3.down};

        for(int i = 0 ; i < 6; i++ ){
            startPosition = transform.position;
            Vector3 endPosition = originalPosition + directions[i] * (levelWidth / 9f);   // (levelWidth / 7.5f) para que el movimiento sea proporcional al tamaño de la pieza

            float elapsedTime = 0;
            float animationTime = 0.05f;
            while(elapsedTime < animationTime){
                
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);
                        
                elapsedTime += Time.deltaTime;
                yield return 0;
            }
        }
        transform.position = originalPosition;

        movingThisPiece = false;
    }

}
