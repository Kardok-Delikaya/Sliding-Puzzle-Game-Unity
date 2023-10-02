//https://github.com/kardok1231
//By Ömer Faruk Yerlikaya

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Puzzle : MonoBehaviour
{
    [Header("Moving Pieces")]
    public Piece emptyTile;
    public Piece selectedTile;
    public bool isMoving;

    [Header("List of pieces on puzzle")]
    public List<Piece> pieces;
    public List<Piece> emptyPieces;

    [Header("Puzzle Values")]
    public Transform enterPoint; //This one should be putted at (0,0) location of the puzzle.
    public int tileSize; //Length or height of your square matrix.
    public int tileArtID; //ID number of picture background.

    [Header("Handling Win")]
    public GameObject winAlert;
    bool youWon;
    List<int> ids = new List<int>();
    List<bool> bools=new List<bool>();

    private void Start()
    {
        //Adds empty and picture pieces to lists.
        pieces.AddRange(GetComponentsInChildren<Piece>());
        for (int i = 0; i < pieces.Count; i++)
        {
            //Removes from pieces if piece is not in puzzle or a picture piece.
            if (pieces[i].emptyTile)
            {
                emptyPieces.Add(pieces[i]);
                pieces.Remove(pieces[i]);
                i--;
            }
            else if (pieces[i].extraTile)
            {
                pieces.Remove(pieces[i]);
                i--;
            }
        }
        //To make the win easier, I closed randomizing enter point position.
        //enterPoint.transform.localPosition = new Vector2(0,Random.Range(0,-tileSize));
    }

    private void FixedUpdate()
    {
        //This part moves the tiles to they new cordinates.
        if (isMoving)
        {
            emptyTile.transform.localPosition = Vector2.MoveTowards(emptyTile.transform.localPosition,new Vector2 (emptyTile.x, emptyTile.y),Time.fixedDeltaTime);
            selectedTile.transform.localPosition = Vector2.MoveTowards(selectedTile.transform.localPosition,new Vector2 (selectedTile.x, selectedTile.y),Time.fixedDeltaTime);
            if (emptyTile.transform.localPosition == new Vector3(emptyTile.x, emptyTile.y)&& selectedTile.transform.localPosition == new Vector3(selectedTile.x, selectedTile.y))
            {
                StartCoroutine(MoveCoolDown());
            }
        }
    }

    public void TryToMove()
    {
        //Checks if empty and selected tile are next to each other.
        if (emptyTile.x ==  selectedTile.x + 1&& emptyTile.y== selectedTile.y)
        {
            MoveTiles(emptyTile, selectedTile);
        }
        else if(emptyTile.x ==  selectedTile.x - 1 && emptyTile.y ==  selectedTile.y)
        {
            MoveTiles(emptyTile, selectedTile);
        }
        else if (emptyTile.y ==  selectedTile.y + 1 && emptyTile.x ==  selectedTile.x)
        {
            MoveTiles(emptyTile, selectedTile);
        }
        else if (emptyTile.y ==  selectedTile.y - 1 && emptyTile.x ==  selectedTile.x)
        {
            MoveTiles(emptyTile, selectedTile);
        }
        else
        {
            emptyTile = null;
            selectedTile = null;
        }
    }

    void MoveTiles(Piece piece, Piece target)
    {
        //Changes values of pieces cortinates.
        int pieceX = piece.x;
        int pieceY = piece.y;
        piece.x =  target.x;
        piece.y =  target.y;
        target.x = pieceX;
        target.y = pieceY;
        StartCoroutine(MoveWait());
    }

    IEnumerator MoveWait()
    {
        //Waits a little bit before movemment
        yield return new WaitForSeconds(.5f);
        isMoving = true;
    }

    IEnumerator MoveCoolDown()
    {
        //Waits a little bit after movemment
        yield return new WaitForSeconds(.2f);
        emptyTile = null;
        selectedTile = null;
        isMoving = false;
        CheckWin();
    }

    public void CheckWin()
    {
        if (pieces.Count == tileSize * tileSize)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                //If all pieces are not belong to actual picture, code breaks.
                if (pieces[i].ArtID == tileArtID)
                {
                    if (!ids.Contains(pieces[i].pieceID))
                    {
                        ids.Add(pieces[i].pieceID);
                        bools.Add(false);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (ids.Count == tileSize * tileSize)
            {
                for(int i=0; i < pieces.Count; i++)
                {
                    //Checks if pieces position on puzzle are correct.
                    if (pieces[i].x - pieces[i].y*3 == pieces[i].pieceID)
                    {
                        bools[i]=true;
                    }
                    else
                    {
                        bools[i] = false;
                    }
                }

                youWon = true;
                foreach(bool checkBool in bools)
                {
                    if (!checkBool)
                    {
                        youWon = false;
                    }
                }
            }
            if (youWon)
            {
                winAlert.SetActive(true);
            }
        }
    }
}