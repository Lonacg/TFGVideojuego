using System.Collections;
using UnityEngine;

public class PieceCheck : MonoBehaviour
{


    [Header("Tiles Width:")]
    protected float levelWidth; 

    [Header("Vectors:")]
    private Vector2 rayOriginUp;
    private Vector2 rayOriginDown;
    private Vector2 rayOriginRight;
    private Vector2 rayOriginLeft;
    [Header("Variables:")]
    private bool canClic= true;
    private bool movingPiece= false;
    [Header("Game Objects:")]
    [SerializeField] private GameObject stageManager;
    private StageManagerPuzzle scriptStageManager;


    public delegate void _OnMovingPiece();
    public static event _OnMovingPiece OnMovingPiece;


    public virtual void StartLevel()
    {
        // Se sobreescribe en cada clase que hereda de esta
    }


    void Awake()
    {
        scriptStageManager = stageManager.GetComponent<StageManagerPuzzle>();
    }



    void Update()
    {      

    
        //ShowRaycast();
    }


    private void OnMouseDown()
    {
        movingPiece = scriptStageManager.movingPiece;

        if(!movingPiece){ 

            UpdateRayOrigin();

            // Creamos un rayo para cada direccion y detectamos si está colisionando con algo o esta libre
            RaycastHit2D upHit = Physics2D.Raycast(rayOriginUp, Vector2.up, 0.2f);
            RaycastHit2D downHit = Physics2D.Raycast(rayOriginDown, Vector2.down, 0.2f);
            RaycastHit2D rightHit = Physics2D.Raycast(rayOriginRight, Vector2.right, 0.2f);
            RaycastHit2D leftHit = Physics2D.Raycast(rayOriginLeft, Vector2.left, 0.2f);
        
        
        
            if(upHit && downHit && rightHit && leftHit){
                // Choca con todos asi que no hay ningun hueco libre contiguo

                // HACER SHAKE
                Debug.Log("NO ME PUEDO MOVER");
            }
            else{
                // Hay un hueco libre luego comprobamos cual es, movemos la pieza a ese lugar y lanzamos el evento a StageManager para que cambie su bool movingPiece a true
                if(OnMovingPiece != null)                          
                    OnMovingPiece();


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

        

    }


    private void UpdateRayOrigin()
    {

        rayOriginUp = (Vector2)transform.position + Vector2.up * levelWidth;
        rayOriginDown = (Vector2)transform.position + Vector2.down * levelWidth;
        rayOriginRight = (Vector2)transform.position + Vector2.right * levelWidth;
        rayOriginLeft = (Vector2)transform.position + Vector2.left * levelWidth;


    }

    private void ShowRaycast(){

        UpdateRayOrigin();

        Debug.DrawLine(rayOriginUp, rayOriginUp + Vector2.up * 0.2f, Color.magenta);
        Debug.DrawLine(rayOriginDown, rayOriginDown + Vector2.down * 0.2f, Color.yellow);
        Debug.DrawLine(rayOriginRight, rayOriginRight + Vector2.right * 0.2f, Color.blue);
        Debug.DrawLine(rayOriginLeft, rayOriginLeft + Vector2.left * 0.2f, Color.green);
    }


    IEnumerator MovePiece(Vector3 direction){
        // Impedimos que se pueda pulsar este boton mientras realizamos el movimiento
        canClic = false;

        float elapsedTime = 0;
        float animationTime = 0.5f;
        bool notified = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + 2 * levelWidth * direction;
        while(elapsedTime < animationTime){
            
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

            // Cuando ya ha hecho mas de la mitad del recorrido, lanzamos el evento para que puedan moverse otras piezas, ya que lo que falta de reorrido no afecta (el rayo de deteccion sale en la mediatriz de cada arista)
            if( elapsedTime * 2 > animationTime && OnMovingPiece != null && !notified){
                OnMovingPiece();
                notified = true;
            }                          
                
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.position = endPosition;




    }


}
