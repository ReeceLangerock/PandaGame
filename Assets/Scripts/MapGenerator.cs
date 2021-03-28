using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    // private List<Tilemap> tilemap;
    [SerializeField] public Tile topTile;
    [SerializeField] public Tile middleTile;
    [SerializeField] private Grid grid;
    private Tilemap tilemap;

    private int height = 8;
    private int width = 20;
    private int lastX = 0;
    private int lastY = 0;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = grid.GetComponentInChildren<Tilemap>();

        Debug.Log("Tilemap");

        StartCoroutine(Straight());
        StartCoroutine(Gap());
        StartCoroutine(AngleUp());


    }


    IEnumerator Straight()
    {
        Debug.Log("Straight" + lastX + "-" + lastY);

        for (int x = lastX; x < width + lastX; x++)
        {
            for (int y = lastY; y < height + lastY; y++)
            {
                Tile tile = y == lastY ? topTile : middleTile;
                tilemap.SetTile(new Vector3Int(x, -y, 0), tile);

            }

        }
        lastX += width;
        yield return null;

    }

    IEnumerator Gap()
    {
        Debug.Log("Gap" + lastX + "-" + lastY);

        for (int x = lastX; x < width + lastX; x++)
        {
            for (int y = lastY; y < height + lastY; y++)
            {
                Tile tile = y == lastY ? topTile : middleTile;
                if (x > lastX + width / 3 && x < (lastX + (width / 3)) + 4)
                {
                    break;
                }
                tilemap.SetTile(new Vector3Int(x, -y, 0), tile);

            }

        }
        lastX += width;
        yield return null;
    }

    IEnumerator AngleUp()
    {
        Debug.Log("Gap" + lastX + "-" + lastY);
        int offset = 0;
        for (int x = lastX; x < width + lastX; x++)
        {
            for (int y = lastY; y < height + lastY; y++)
            {
                Tile tile = y == lastY ? topTile : middleTile;

                tilemap.SetTile(new Vector3Int(x, -y + offset, 0), tile);

            }
            if(offset < 5){

            offset++;
            }

        }
        lastX += width;
        lastY -= height / 2;
        yield return null;
    }

}
