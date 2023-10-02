//https://github.com/kardok1231
//By Ömer Faruk Yerlikaya

using UnityEngine;
using System;

public class Piece : MonoBehaviour
{
    Puzzle puzzle;

    [Header("Cordinates")] // x and y values localPositions x and y to make it easier to get and saves space on code.
    public int x;
    public int y;

    [Header("Piece Values")]
    public bool emptyTile; //What kinde puzzle piece.
    public bool extraTile; //Where is puzzle piece.
    public int ArtID; //pieceID should begin from 0 and it should increase from left to right.
    public int pieceID; //pieceID should begin from 0 and it should increase from left to right.
    
    private void Awake()
    {
        puzzle = GetComponentInParent<Puzzle>();
        SetCordinates(transform);
    }
    void OnMouseDown()
    {
        if (!puzzle.isMoving)
        {
            //Checks to find which kind of tile is this one. 
            if (!emptyTile&&!extraTile)
            {
                puzzle.selectedTile = this;
                if (puzzle.emptyTile != null)
                {
                    puzzle.TryToMove();
                }
            }
            else if (extraTile)
            {
                if (!emptyTile)
                {
                    for (int i = 0; i < puzzle.emptyPieces.Count; i++)
                    {
                        //Checks if there is an empty puzzle piece at the entry point.
                        if (puzzle.emptyPieces[i].x == puzzle.enterPoint.localPosition.x && puzzle.emptyPieces[i].y == puzzle.enterPoint.localPosition.y)
                        {
                            //Takes empty pieces and puts it on extra tile potition.
                            puzzle.emptyPieces[i].transform.SetParent(this.transform.parent);
                            puzzle.emptyPieces[i].transform.localPosition = transform.localPosition;
                            puzzle.emptyPieces[i].extraTile = true;
                            puzzle.emptyPieces.Remove(puzzle.emptyPieces[i]);

                            //Takes extra tile to ender point position.
                            transform.SetParent(puzzle.transform);
                            transform.localPosition = new Vector2(puzzle.enterPoint.localPosition.x, puzzle.enterPoint.localPosition.y);
                            puzzle.pieces.Add(this);
                            SetCordinates(puzzle.enterPoint);
                            this.extraTile = false;
                        }
                    }
                }
                else
                {
                    if (puzzle.selectedTile!=null&&puzzle.selectedTile.x == puzzle.enterPoint.localPosition.x && puzzle.selectedTile.y == puzzle.enterPoint.localPosition.y)
                    {
                        //If there is a puzzle piece at the enter point and it's selected, it swaps with empty piece at extra tiles.
                        puzzle.selectedTile.transform.SetParent(this.transform.parent);
                        puzzle.selectedTile.transform.localPosition = transform.localPosition;
                        puzzle.selectedTile.extraTile = true;
                        transform.SetParent(puzzle.transform);
                        puzzle.emptyPieces.Add(this);
                        transform.localPosition = new Vector2(puzzle.enterPoint.localPosition.x, puzzle.enterPoint.localPosition.y);
                        puzzle.pieces.Remove(puzzle.selectedTile);
                        SetCordinates(puzzle.enterPoint);
                        this.extraTile = false;
                    }
                }
                puzzle.emptyTile = null;
                puzzle.selectedTile = null;
                puzzle.CheckWin();
            }
            else
            {
                puzzle.emptyTile = this;
                if (puzzle.selectedTile != null)
                {
                    puzzle.TryToMove();
                }
            }
        }
    }

    void SetCordinates(Transform targetTransform)
    {
        x = (int)targetTransform.localPosition.x;
        y = (int)targetTransform.localPosition.y;
    }
}