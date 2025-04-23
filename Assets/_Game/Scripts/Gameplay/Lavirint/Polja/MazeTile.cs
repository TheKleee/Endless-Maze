using UnityEngine;
using Lavirint;
using UnityEditor;
using UnityEngine.Rendering.RenderGraphModule;

[ExecuteInEditMode]
public class MazeTile : MonoBehaviour
{
    [Header("Tile Size:"), SerializeField, Range(0.0f, 5.0f)]
    float tileSize = 1f;
    Vector3 tileScale = Vector3.one;

    [Header("Walls"), SerializeField]
    GameObject[] Walls = new GameObject[2];

    public bool walkable = false;

    private void Awake()
    {
        tileScale *= tileSize.SetTileSize();


    }

    void CreateTile()
    {

    }

    void CutWall(int id)
    {

    }
}
