using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Lavirint;

public class MazePath : MonoBehaviour
{
    public List<MazeTile> mazeTiles = new List<MazeTile>();
    Stack<MazeTile> currentPath = new Stack<MazeTile>();
    int formerTileID;
    public int mazeSize { private get; set; }

    public void GenerateMazePath() => 
        GenerateMazePath(mazeTiles[mazeSize + 2]);

    private void GenerateMazePath(MazeTile tile)
    {
        tile.visited = true;
        if (!currentPath.Contains(tile))
            currentPath.Push(tile);

        List<MazeTile> directions = new List<MazeTile>();
        directions.AddRange(AddDirs(tile));
        
        //Nema puta napred => Vrati se nazad ako je moguce... u suprotnom je ceo lavirint istrazen.
        if (directions.Count == 0)
        {
            if (currentPath.Count > 1)
            {
                Debug.Log(currentPath.Count);
                currentPath.Pop();
                GenerateMazePath(currentPath.Peek());
            }
            return;
        }
        //Idi napred ako nadjes put! :)
        var randDir = Random.Range(0, directions.Count);
        MazeTile nextTile = directions[randDir];
        CheckWallToCut(tile, nextTile);

        if(!currentPath.Contains(nextTile))
            GenerateMazePath(nextTile);
        
    }

    void CheckWallToCut(MazeTile prev, MazeTile next)
    {
        if (next.id > prev.id)
            prev.CutWall(next.id - 1 == prev.id ? 0 : 1);
        else
            next.CutWall(prev.id - 1 == next.id ? 0 : 1);
    }

    List<MazeTile> AddDirs(MazeTile tile)
    {
        List<MazeTile> d = new List<MazeTile>();

        int[] dirs = new int[] 
        { 
            tile.id - 1, tile.id + 1, tile.id - mazeSize, tile.id + mazeSize
        }; //Kuda mozemo da se krecemo. Respektivno: levo, desno, dole, gore

        for (int i = tile.id - mazeSize; i <= tile.id + mazeSize; i++)
            if (i >= 0 && dirs.Contains(mazeTiles[i].id) && !mazeTiles[i].visited && mazeTiles[i].walkable)
                d.Add(mazeTiles[i]);

        return d;
    }
    

    #region Ostalo

    public void AddMazeTile(MazeTile id) => mazeTiles.Add(id);
    #endregion ostalo />
}

