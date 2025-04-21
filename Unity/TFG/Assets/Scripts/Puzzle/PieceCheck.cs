using System.Collections;
using UnityEngine;

public class PieceCheck : MonoBehaviour
{
    private float rayOffset;


    [Header("Tiles Width:")]
    protected float levelWidth; 


    protected bool easyLevel;
    protected bool mediumLevel;
    protected bool hardLevel;

    private Vector2 rayOriginUp;
    private Vector2 rayOriginDown;
    private Vector2 rayOriginRight;
    private Vector2 rayOriginLeft;

    private bool canClic= true;



    public virtual void StartLevel()
    {
        canClic = true;
    }

    void Update()
    {      

    
        //ShowRaycast();
    }


    private void OnMouseDown()
    {        
        UpdateRayOrigin();

        // Creamos un rayo para cada direccion y detectamos si está colisionando con algo o esta libre
        RaycastHit2D upHit = Physics2D.Raycast(rayOriginUp, Vector2.up, 0.2f);
        RaycastHit2D downHit = Physics2D.Raycast(rayOriginDown, Vector2.down, 0.2f);
        RaycastHit2D rightHit = Physics2D.Raycast(rayOriginRight, Vector2.right, 0.2f);
        RaycastHit2D leftHit = Physics2D.Raycast(rayOriginLeft, Vector2.left, 0.2f);
        
        
        if(canClic){
            if(upHit && downHit && rightHit && leftHit){
                // Choca con todos asi que no hay ningun hueco libre contiguo

                // HACER SHAKE
                Debug.Log("NO ME PUEDO MOVER");
            }
            else{
                // Hay un hueco libre luego comprobamos cual es y movemos la pieza a ese lugar
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

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + 2 * levelWidth * direction;
        while(elapsedTime < animationTime){
            
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationTime);

            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.position = endPosition;


        // Volvemos a permitir el pulsado
        canClic = true;
    }


}
