using UnityEngine;
using Lavirint;
using UnityEditor;
using UnityEngine.Rendering.RenderGraphModule;


public class MazeTile : MonoBehaviour
{
    public int tileSize { get; private set; }

    [Header("Walls"), SerializeField]
    GameObject[] Walls = new GameObject[2];

    [Header("Walkable Material"), SerializeField]
    Material walkMat;
    public bool walkable { get; private set; }
    public int id { get; private set; }
    Vector3 tilePos = Vector3.zero;

    private void Awake()
    {
        tileSize = (int)transform.GetChild(0).localScale.x;
    }

    void CutWall(int id)
    {
        //TODO: Prilikom prolaska kroz maze, proveravamo da li smo bili na tom id-u
        //ako nismo mozemo da isecemo jedan zid i pomerimo se ka toj lokaciji, ili se pomerimo na lokaciju i uklonimo taj zid. (zavisi od id-a)
    }

    public void PlaceTile(int id, int n, bool walkable)
    {
        this.id = id;
        this.walkable = walkable;
        if (walkable) SetWalkableMaterial(transform);
        TilePos(id, n);
    }

    void TilePos(int id, int n)
    {
        tilePos.x = id % n * tileSize;
        tilePos.z = Mathf.Floor(id / n) * tileSize;
        transform.localPosition = tilePos;
    }

    void SetWalkableMaterial(Transform trans)
    {
        foreach (Transform c in trans)
        {
            if (c.childCount > 0)
                    SetWalkableMaterial(c);
            if(c.GetComponent<Renderer>() != null)
                c.GetComponent<Renderer>().material = walkMat;
        }
    }
}
