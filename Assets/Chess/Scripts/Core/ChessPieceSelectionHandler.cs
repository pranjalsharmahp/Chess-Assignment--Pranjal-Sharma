using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chess.Scripts.Core;

public class ChessPieceSelectionHandler : MonoBehaviour
{
    int currentRow;
    int currentColumn;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            ChessBoardPlacementHandler.Instance.ClearHighlights();
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit: " + hit.collider.name);
                ChessPlayerPlacementHandler chessPlayerplacementHandler=hit.collider.gameObject.GetComponent<ChessPlayerPlacementHandler>();
                currentRow=chessPlayerplacementHandler.row;
                currentColumn=chessPlayerplacementHandler.column;
                switch(hit.collider.name){
                    case "King":
                        HighlightKingLegalMoves();
                        break;
                    case "Queen":
                        HighlightQueenLegalMoves();
                        break;
                    case "Knight":
                        HighlightKnightLegalMoves();
                        break;
                    case "Bishop":
                        HighlightBishopLegalMoves();
                        break;
                    case "Rook":
                        HighlightRookLegalMoves();
                        break;
                    case "Pawn":
                        HighlightPawnLegalMoves();
                        break;
                }
            }
        }
    }

    void HighlightPawnLegalMoves(){
        if(currentRow==1){
            TryHighlight(currentRow+1,currentColumn);
            TryHighlight(currentRow+2,currentColumn);
        }

        Debug.Log("Pawn Moves Highlighted");
    }

    void HighlightQueenLegalMoves(){
        Debug.Log("Queen Moves Highlighted");
        HighlightDirectionalMoves(1,1);
        HighlightDirectionalMoves(-1,1);
        HighlightDirectionalMoves(1,-1);
        HighlightDirectionalMoves(-1,-1);
        HighlightDirectionalMoves(0,1);
        HighlightDirectionalMoves(1,0);
        HighlightDirectionalMoves(0,-1);
        HighlightDirectionalMoves(-1,0);
    }  

    void HighlightKingLegalMoves()
{
    int[,] moves = {
        {1, 0}, {1, 1}, {0, 1}, {-1, 1},
        {-1, 0}, {-1, -1}, {0, -1}, {1, -1}
    };

    for (int i = 0; i < moves.GetLength(0); i++){
        TryHighlight(currentRow + moves[i, 0], currentColumn + moves[i, 1]);
    }

    Debug.Log("King Moves Highlighted");
}

    void HighlightKnightLegalMoves(){
        int[,] moves = {
            {2, 1}, {1, 2}, {-1, 2}, {-2, 1},
            {-2, -1}, {-1, -2}, {1, -2}, {2, -1}
        };

        for (int i = 0; i < moves.GetLength(0); i++){
            TryHighlight(currentRow + moves[i, 0], currentColumn + moves[i, 1]);
        }
        Debug.Log("Knight Moves Highlighted");
    }
    
    void HighlightBishopLegalMoves(){
        HighlightDirectionalMoves(1,1);
        HighlightDirectionalMoves(-1,1);
        HighlightDirectionalMoves(1,-1);
        HighlightDirectionalMoves(-1,-1);
        Debug.Log("Bishop Moves Highlighted");
    }

    void HighlightRookLegalMoves(){
        HighlightDirectionalMoves(0,1);
        HighlightDirectionalMoves(1,0);
        HighlightDirectionalMoves(0,-1);
        HighlightDirectionalMoves(-1,0);
        Debug.Log("Rook Moves Highlighted");
    }

    bool isOccupiedByPlayer(int row,int column){
        if(ChessBoardPlacementHandler.Instance._chessPiecePosition[row,column]!=null &&ChessBoardPlacementHandler.Instance._chessPiecePosition[row,column].tag=="Black"){
            return true;
        }
        return false;
    }
    
    bool TryHighlight(int row, int column){
        if(!isValidMove(row,column)){
            return false;
        }
            
        if( isOccupiedByEnemy(row,column)){
            ChessBoardPlacementHandler.Instance.HighlightEnemy(row,column);
            return false;
        }
        if(isOccupiedByPlayer(row,column)){
            return false;
        }

        ChessBoardPlacementHandler.Instance.Highlight(row,column);
        return true;
    }

    bool isOccupiedByEnemy(int row,int column){
        if(ChessBoardPlacementHandler.Instance._chessPiecePosition[row,column]!=null &&ChessBoardPlacementHandler.Instance._chessPiecePosition[row,column].tag=="White"){
            return true;
        }
        return false;
    }

    bool isValidMove(int row,int column){
        if( row>=0&&row<8 && column>=0 && column<8){
            return true;
        }
        return false;
    }

    void HighlightDirectionalMoves(int rowIncrement,int columnIncrement){
        for(int i=currentRow+rowIncrement,j=currentColumn+columnIncrement; (isValidMove(i,j)); i+=rowIncrement,j+=columnIncrement){
            if(!TryHighlight(i,j)){
                break;
            };
        }
    }
}
