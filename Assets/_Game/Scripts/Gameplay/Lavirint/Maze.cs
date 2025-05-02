using UnityEngine;

public class Maze : MonoBehaviour
{
    [Header("Maze Tile:"), SerializeField]
    MazeTile mazeTile;
    [Space]
    [Header("Maze Size:"), SerializeField]
    int mazeSize = 5; //5 - easy, 7 - medium, 9 - hard
    int tileID = 0;
    bool walkable = false;
    [SerializeField] GameObject skyDome;
    bool skyDomeSet = false;
    private void Awake() => CreateMaze();
    
    void CreateMaze()
    {
        for (int i = 1; i <= mazeSize; i++)
            for (int j = 1; j <= mazeSize; j++)
            {
                walkable = i != 1 && i != mazeSize && j != 1 && j != mazeSize;
                CreateNode(tileID, mazeSize, walkable);
                tileID++;
            }
    }


    void CreateNode(int id, int n, bool walkable)
    {
        MazeTile node = Instantiate(mazeTile);
        node.transform.parent = transform;
        node.PlaceTile(id, n, walkable);
        if (!skyDomeSet)
            SkyDomePosition(node.tileSize);
    }

    void SkyDomePosition(int tileSize)
    {
        skyDomeSet = true;
        skyDome.transform.localPosition = new Vector3(mazeSize / 2 * tileSize, 0, mazeSize / 2 * tileSize);
    }
}
