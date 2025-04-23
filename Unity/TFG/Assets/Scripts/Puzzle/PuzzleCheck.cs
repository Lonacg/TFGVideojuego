using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PuzzleCheck : MonoBehaviour
{


    [Header("Game Objects:")]
    [SerializeField] private GameObject stageManager;




    public delegate void _OnGotIt();
    public static event _OnGotIt OnGotIt;


    public Dictionary<string, Vector3> puzzleSolution = new();
    public Dictionary<string, Vector3> puzzleMerged = new();
    public Dictionary<string, Vector3> puzzlePlaying = new();   // Añadimos esta variable para guardar la configuracion del empiece, por si queremos poner el boton de deshacer todo en el futuro



    void OnEnable()
    {
        PieceCheck.OnMoveMade += HandleOnMoveMade;
    }

    void OnDisable()
    {
        PieceCheck.OnMoveMade -= HandleOnMoveMade;
    }



    private void HandleOnMoveMade(GameObject pieceMoved){
        ExchangePositionWithEmpty(pieceMoved);

        bool gotIt = CheckDictionary(newDictionary: puzzlePlaying);
        if(gotIt){
            if(OnGotIt != null)                          
                OnGotIt();
        }

    }


    private bool CheckDictionary(Dictionary<string, Vector3>  newDictionary){
        // Empezamos a comprobar los primeros numeros, que son las fichas superiores, y las ultimas en ser colocadas para resolverlo
        for( int piece = 0 ; piece < gameObject.transform.childCount ; piece++ ){

            newDictionary.TryGetValue(piece.ToString(), out Vector3 vectorPiece);
            puzzleSolution.TryGetValue(piece.ToString(), out Vector3 vectorCorrect);

            // Si una sola clave es diferente paramos de comprobar, ya son distintos
            if(vectorPiece != vectorCorrect){
                return false;
            }
        }
        return true;

    }



    private void ExchangePositionWithEmpty(GameObject pieceMoved){

        // Guardamos los valores de los vectores posicion de cada ficha (tiene los valores de antes de moverse)
        puzzlePlaying.TryGetValue(pieceMoved.name, out Vector3 vectorPieceMoved);
        puzzlePlaying.TryGetValue( "0", out Vector3 vectorPieceEmpty );

        // Intercambiamos los vectores posicion de cada ficha en el diccionario (para que tengan los valores de despues de moverse)
        puzzlePlaying[pieceMoved.name] = vectorPieceEmpty;
        puzzlePlaying["0"] = vectorPieceMoved; 

    }



    void Start()
    {
        // Creamos el diccionario inicial con las piezas (tiene que ser en el start o en el onenable porque en el awake stage manager los quita de la escena, y si no lo crea antes de eliminarlo)
        for(int i = 0 ; i < gameObject.transform.childCount ; i++ ){  

            Transform childTransform = gameObject.transform.GetChild(i);  
            puzzleSolution.Add(i.ToString(), childTransform.position);      

            //Debug.Log("Dicionario: " + i + ",  (" + childTransform.position.x + ", " + childTransform.position.y + ")");
        }

        puzzleMerged = new Dictionary<string, Vector3>(puzzleSolution);    // Asignamos a puzzleMerged los mismos datos que puzzleSolution, pero en memoria diferente (si no apuntan a lo mismo, y al modificar uno se modifica el otro)

        MergePuzzle();

    }


    private void MergePuzzle(){

        // Accedemos al numero de trasposiciones que haremos para mezclar el puzzle, en funcion de la dificultad escogida
         int transpositions = stageManager.GetComponent<StageManagerPuzzle>().transpositions;


        // Cambiamos las posiciones de las piezas un numero par de veces, dejando la libre en su sitio
        for(int piece = 1 ; piece <= transpositions ; piece++ ){ 

            int pieceRandom = ChooseRandom(samplePiece: piece);

            // Guardamos los valores de los vectores posicion de cada ficha
            puzzleMerged.TryGetValue(piece.ToString(), out Vector3 vectorPiece);
            puzzleMerged.TryGetValue(pieceRandom.ToString(), out Vector3 vectorPieceRandom);


            // Intercambiamos los vectores posicion de cada ficha en el diccionario
            puzzleMerged[piece.ToString()] = vectorPieceRandom;
            puzzleMerged[pieceRandom.ToString()] = vectorPiece;

            // Intercambiamos las posiciones de las fichas en el puzzle real
            gameObject.transform.GetChild(piece).transform.position = vectorPieceRandom;
            gameObject.transform.GetChild(pieceRandom).transform.position = vectorPiece;

        }
        puzzlePlaying = new Dictionary<string, Vector3>(puzzleMerged); 

        // Comprobamos que el diccionario resultado no haya quedado igual a la solucion por azar. En el improbable caso de que si lo volvemos a mezclar
        bool samePuzzle = CheckDictionary(newDictionary: puzzleMerged);
        if(samePuzzle){
            MergePuzzle();
        }

    }

    private int ChooseRandom(int samplePiece){
        
        int pieceRandom = Random.Range(1, gameObject.transform.childCount);  // No cambiamos la posicion de la vacia y es hasta el numero de hijos porque el ultimo no se incluye (Ej para 9 piezas (9 hijos): la 0 no, y de la 1 a la 8 si)

        // Tenemos que impedir las trasposiciones de cada pieza consigo misma, porque eso haria que no cuenten y podriamos obtener un numero total de trasposiciones impar
        if(pieceRandom == samplePiece){
            return ChooseRandom(samplePiece);
        }
        else{
            return pieceRandom;
        }

    }


}
