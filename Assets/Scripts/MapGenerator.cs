using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] public RuleTile ruleTile;
    [SerializeField] private Grid grid;
    private Tilemap tilemap;

    private const int height = 12;
    private const int width = 20;
    private int lastX = 0;
    private int lastY = 0;
    private string lastSection = "";
    private int lastPlantedX;
    private bool noPlants;

    [SerializeField] Star starPrefab;
    [SerializeField] GameObject sliderPrefab;
    [SerializeField] GameObject elevatorPrefab;
    [SerializeField] GameObject boxPrefab;
    [SerializeField] GameObject chasmPrefab;
    [SerializeField] GameObject treePrefab1;
    [SerializeField] GameObject treePrefab2;
    [SerializeField] GameObject plantPrefab1;
    [SerializeField] GameObject plantPrefab2;
    [SerializeField] GameObject plantPrefab3;
    [SerializeField] GameObject plantPrefab4;
    [SerializeField] GameObject plantPrefab5;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = grid.GetComponentInChildren<Tilemap>();
        Debug.Log("Tilemap");
        StartCoroutine(GenerateLevel());

    }

    IEnumerator GenerateLevel()
    {
        List<string> functions = new List<string>();

        functions.Add("Straight");
        functions.Add("Gap");
        functions.Add("AngleUp");
        functions.Add("AngleDown");
        functions.Add("Slider");
        functions.Add("Pyramid");
        functions.Add("Elevator");
        functions.Add("BoxPush");
        functions.Add("Platforms");

        StartCoroutine(StartSection());
        yield return null;

        for (int i = 0; i < 10; i++)
        {
            int random = (int)Mathf.Floor(Random.Range(0, functions.Count));
            string randomFunc = functions[random];
            Debug.Log("adding - " + randomFunc);
            if (randomFunc == "Straight" && lastSection == "Straight")
                randomFunc = functions[random + 1];

            if (randomFunc == "Straight")
            {
                StartCoroutine(Straight());
            }
            else
            {
                StartCoroutine(randomFunc);
            }

            lastSection = randomFunc;
            yield return null;
        }
        StartCoroutine(End());

    }

    void addRandomTree(int x, int yOverride = -1)
    {
        int random = Random.Range(0, 100);
        if (random > 50 && x > lastPlantedX + 2)
        {
            List<GameObject> plants = new List<GameObject>();
            plants.Add(treePrefab1);
            plants.Add(treePrefab2);
            plants.Add(treePrefab1);
            plants.Add(treePrefab2);
            plants.Add(plantPrefab1);
            plants.Add(plantPrefab2);
            plants.Add(plantPrefab3);
            plants.Add(plantPrefab4);
            plants.Add(plantPrefab5);
            int y = yOverride != -1 ? -yOverride : -lastY;
            float randomScale = Random.Range(.5f, 1.2f);
            int randomFlip = Mathf.RoundToInt(Random.Range(0, 1f));
            GameObject randomPlant = plants[(int)Random.Range(0, plants.Count)];
            GameObject newPlant = Instantiate(randomPlant, new Vector3(x, y + .95f, 0), Quaternion.identity);
            newPlant.transform.localScale = new Vector3(randomScale, randomScale, 1);
            lastPlantedX = x;
            if (randomFlip == 1)
            {
                newPlant.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

    }


    IEnumerator Straight(int width = 20, int height = 8)
    {
        for (int x = lastX; x < width + lastX; x++)
        {
            if (x > lastX + 1 && x < width + lastX - 1 && !noPlants)
            {

                addRandomTree(x);
            }
            for (int y = lastY; y < height + lastY; y++)
            {
                tilemap.SetTile(new Vector3Int(x, -y, 0), ruleTile);
            }
        }
        lastX += width;
        yield return null;
    }

    IEnumerator Gap()
    {
        StartCoroutine(Straight(6));
        Instantiate(starPrefab, new Vector3(lastX + 2, -lastY + 5, 0), Quaternion.identity);
        Instantiate(chasmPrefab, new Vector3(lastX + 2.5f, -lastY + .5f, 0), Quaternion.identity);
        lastX += 5;
        StartCoroutine(Straight(6));
        yield return null;
    }

    IEnumerator Slider()
    {

        StartCoroutine(Straight(5));
        GameObject slider = Instantiate(sliderPrefab, new Vector3(lastX + 2, -lastY + .75f, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
        Instantiate(starPrefab, new Vector3(lastX + 5, -lastY + 3, 0), Quaternion.identity);
        GameObject chasm = Instantiate(chasmPrefab, new Vector3(lastX + 2.5f, -lastY + .5f, 0), Quaternion.identity);
        chasm.GetComponent<BoxCollider2D>().offset = new Vector2(2.5f, -4f);
        chasm.GetComponent<BoxCollider2D>().size = new Vector2(12f, 1f);
        slider.GetComponent<SlideController>().end = new Vector2(lastX + 8, lastY + .75f);
        lastX += 10;
        StartCoroutine(Straight(5));
        yield return null;

    }

    IEnumerator AngleUp()
    {
        int offset = 0;
        int heightChange = 4;
        bool addStar = false;
        for (int x = lastX; x < width + lastX; x++)
        {
            addStar = x == lastX + offset + width / 2;
            if (offset == heightChange && x > lastX + heightChange + 2 && x < width + lastX - 1)
            {
                addRandomTree(x, lastY - offset);

            }
            for (int y = lastY; y < height + lastY; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, -y + offset, 0);
                tilemap.SetTile(tilePos, ruleTile);
                if (addStar)
                {
                    Instantiate(starPrefab, new Vector3(lastX + 9, -y + offset + 3, 0), Quaternion.identity);
                    addStar = false;
                }
            }
            if (offset <= heightChange)
            {
                offset++;
            }

        }

        lastX += width;
        lastY -= offset;
        yield return null;
    }

    IEnumerator AngleDown()
    {
        int offset = 0;
        int heightChange = -5;
        bool addStar = false;

        for (int x = lastX; x < width + lastX; x++)
        {
            addStar = x == lastX + offset + width / 2;
            if (offset == heightChange && x > lastX + heightChange + 2 && x < width + lastX - 1)
            {
                addRandomTree(x, lastY - offset);
            }
            for (int y = lastY; y < height + lastY; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, -y + offset, 0);
                tilemap.SetTile(tilePos, ruleTile);
                if (addStar)
                {
                    Instantiate(starPrefab, new Vector3(lastX + 8, -y + offset + 3, 0), Quaternion.identity);
                    addStar = false;
                }
            }
            if (offset > heightChange)
            {
                offset--;
            }

        }

        lastX += width;
        lastY -= offset;
        yield return null;
    }

    IEnumerator Elevator()
    {
        StartCoroutine(Straight(6));
        GameObject slider = Instantiate(elevatorPrefab, new Vector3(lastX + 2.5f, -lastY + .75f, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
        slider.GetComponent<SlideController>().end = new Vector2(lastX + 2.5f, -lastY + 10.755f);
        Instantiate(chasmPrefab, new Vector3(lastX + 2.5f, -lastY + .5f, 0), Quaternion.identity);
        lastY -= 10;
        lastX += 5;
        StartCoroutine(Straight(7, 16));
        Instantiate(starPrefab, new Vector3(lastX + 3, -lastY + 3, 0), Quaternion.identity);

        yield return null;
    }

    IEnumerator BoxPush()
    {
        StartCoroutine(Straight(4, 16));
        lastY += 2;
        StartCoroutine(Straight(2, 16));
        lastY += 1;
        Instantiate(boxPrefab, new Vector3(lastX + 4, -lastY + 2, 0), Quaternion.identity);
        StartCoroutine(Straight(10));
        lastY -= 5;
        StartCoroutine(Straight(5, 18));
        Instantiate(starPrefab, new Vector3(lastX - 1, -lastY + 3, 0), Quaternion.identity);
        yield return null;
    }

    IEnumerator Platforms()
    {
        noPlants = true;
        // top platforms
        int topCount = Mathf.RoundToInt(Random.Range(1, 3));
        int xStart = lastX;
        // mid platforms
        lastY -= 8;
        lastX += topCount == 1 ? 10 : 5;

        StartCoroutine(Straight(5, 1));
        if (topCount == 2)
        {
            int beforeAfter = Mathf.RoundToInt(Random.Range(0, 2));
            lastX += 5;
            StartCoroutine(Straight(5, 1));
            Instantiate(starPrefab, new Vector3(lastX - 2.5f - (beforeAfter * 10), -lastY + 3, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(starPrefab, new Vector3(lastX - 2.5f, -lastY + 3, 0), Quaternion.identity);

        }

        // mid platforms
        lastX -= 10;
        lastY += 4;
        StartCoroutine(Straight(5, 1));
        if (topCount == 1)
        {
            lastX += 5;
            StartCoroutine(Straight(5, 1));
        }

        noPlants = false;

        // bottom
        lastY += 4;
        lastX = xStart;
        StartCoroutine(Straight(23));

        yield return null;
    }

    IEnumerator Pyramid()
    {
        StartCoroutine(Straight(5));
        lastY -= 2;
        StartCoroutine(Straight(3));
        lastY -= 2;
        StartCoroutine(Straight(3, 14));
        lastY -= 2;
        StartCoroutine(Straight(3, 16));
        Instantiate(starPrefab, new Vector3(lastX, -lastY + 3, 0), Quaternion.identity);
        lastY += 2;
        StartCoroutine(Straight(3, 14));
        lastY += 2;
        StartCoroutine(Straight(5));
        lastY += 2;
        yield return null;
    }

    IEnumerator End()
    {
        StartCoroutine(Straight(8));

        Vector3 starPos = new Vector3(lastX - 3, -lastY + 5, 0);
        Star star = Instantiate(starPrefab, starPos, Quaternion.identity);
        star.name = "LastStar";
        GameManager.Instance.lastStarPosition = starPos;
        lastY -= 8;
        StartCoroutine(Straight(2, 16));
        lastY -= 2;

        for (int i = 0; i < 5; i++)
        {
            lastY -= 1;
            StartCoroutine(Straight(1, 20 + i));
        }
        StartCoroutine(Straight(8, 20));

        yield return null;

    }

    IEnumerator StartSection()
    {
        lastY -= 16;
        StartCoroutine(Straight(12, 22));
        lastY += 16;
        StartCoroutine(Straight(8));
        yield return null;

    }



    // ??? 
    // IEnumerator DoubleLoop(int xEnd, int yEnd)
    // {
    //     doneLooping = false;
    //     for (int x = xEnd; x < xEnd + lastX; x++)
    //     {
    //         for (int y = yEnd; y < height + yEnd; y++)
    //         {

    //             yield return new Vector2(x, y);
    //         }
    //     }
    //     doneLooping = true;
    //     yield return null;
    // }
}
