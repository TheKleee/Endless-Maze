using System;
using UnityEditorInternal;
using UnityEngine;
using Lavirint;

public class MazeGrid : MonoBehaviour
{
    
    //Calculate and Set the maze grid positions

    [SerializeField]
    Vector3[] mazeGridPoistions;

    int maxGridXY = 0;
    float gridStep = 0;


    public Vector3[] SetMazeGridPositions(int gridSize)
    {
        for (int i = 0; i < gridSize; i++)
           mazeGridPoistions[i] = SetMazeGridPoistion(i);

        return mazeGridPoistions;
    }

    Vector3 SetMazeGridPoistion(int gridID)
    {
        //Set the poistion of mazeGridPositions[gridID]

        throw new NotImplementedException();
    } 
    
    public Vector3 GetMazeGridPosition(int gridID)
    {
        //Get the poistion of mazeGridPositions[gridID]

        throw new NotImplementedException();
    }    
}
