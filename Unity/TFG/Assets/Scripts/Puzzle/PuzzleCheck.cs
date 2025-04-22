using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PuzzleCheck : MonoBehaviour
{


    public delegate void _OnGotIt();
    public static event _OnGotIt OnGotIt;


    public Dictionary<string, Vector3> puzzleSolution = new();
    public Dictionary<string, Vector3> puzzleMerged = new();
    public Dictionary<string, Vector3> puzzlePlaying = new();   // Añadimos esta variable para guardar la configuracion del empiece, por si queremos poner el boton de deshacer todo en el futuro

    void Awake()
    {

            
    }


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

        bool gotIt = CheckDictionary();
        if(gotIt){
            if(OnGotIt != null)                          
                OnGotIt();
        }

    }


    private bool CheckDictionary(){
        // Empezamos a comprobar los primeros numeros, que son las fichas superiores, y las ultimas en ser colocadas para resolverlo
        for( int piece = 0 ; piece < gameObject.transform.childCount ; piece++ ){

            puzzlePlaying.TryGetValue(piece.ToString(), out Vector3 vectorPiece);
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


    void MergePuzzle(){
        // Variante de Fisher-Yates para diccionarios
        for(int piece = gameObject.transform.childCount - 1; piece > 0 ; piece-- ){ 

            int pieceRandom = Random.Range(1, piece + 1);       // La pieza cero no se cambia porque es el hueco libre y debe estar en la misma posicion al empezar

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
    }




}
