using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

    public int width, height;

    [Range(0,100)]
    public int fillPercentage;
    [Range(0, 100)]
    public int treePercentage;
    [Range(0, 100)]
    public int neutralMobHomesPercentage;

    public string seed = "seed";
    private int[,] map;
    public bool debug = false;

    public Tiles tiles;
    public Foilage foilage;
    public NeutralMobFactory neutralMobFactory;

    public GameObject tileHolder;    
    public GameObject player;

    private bool playerIsSpawned = false;

    System.Random randomGen = new System.Random();

    void Start ()
    {
        if (debug)
            Debug.Log("Starting map generation...");

        tileHolder.transform.position = new Vector3(-width / 2, 0, -height / 2);

        GenerateMap();

        if (debug)
            Debug.Log("Starting smoothing iterations...");

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        RenderMapPreFabs();

    }
    	
	void GenerateMap()
    {        
        
        System.Random seedGen;

        map = new int[width, height];

        if(seed == "seed")
        {
            seedGen = new System.Random(randomGen.Next(0,100));
        }
        else
        {
            seedGen = new System.Random(seed.GetHashCode());
        }

        if(debug)
        Debug.Log("Using seed : " + seedGen.ToString());

        for(int x = 0; x < width; x++)
            for(int z = 0; z < height; z++)
            {
                if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                    map[x, z] = 1;
                else
                if(seedGen.Next(0,100) >= fillPercentage)
                {
                    map[x, z] = 1;
                }
                else
                {
                    map[x, z] = 0;
                }
            }
    }

    private int GetNeighbouringWallCount(int xCoor, int zCoor)
    {
        int wallCount = 0;
        for (int neighbourX = xCoor - 1; neighbourX <= xCoor + 1; neighbourX++)
        {
            for (int neighbourZ = zCoor - 1; neighbourZ <= zCoor + 1; neighbourZ++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourZ >= 0 && neighbourZ < height)
                {
                    if (neighbourX != xCoor || neighbourZ != zCoor)
                    {
                        wallCount += map[neighbourX, neighbourZ];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                int neighbourWallTiles = GetNeighbouringWallCount(x, z);

                if (neighbourWallTiles > 4)
                    map[x, z] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, z] = 0;

            }
        }
    }

    void RenderMapPreFabs()
    {
        for (int x = 0; x < width; x++)
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(x, 0, z);
                Quaternion rot = Quaternion.Euler(90, 0, 0);

                if (map[x, z] == 1)
                {
                    GameObject tile = Instantiate(tiles.waterTile, pos, rot) as GameObject;
                    tile.transform.parent = tileHolder.transform;                    
                }
                else
                {
                    GameObject tile = Instantiate(tiles.grassTile, pos, rot) as GameObject;
                    tile.transform.parent = tileHolder.transform;

                    if (x > (width / 2) && z > height / 2 && !playerIsSpawned)
                        SpawnPlayer(x, z);

                    if (randomGen.Next(0, 100) < treePercentage)
                        AddTree(tile);
                    else if (randomGen.Next(0, 500) < neutralMobHomesPercentage)
                        AddNeutralMobHome(tile);

                }
             
            }
    }

    void AddTree(GameObject tile)
    {
        Vector3 pos = tile.transform.position;
        Quaternion rot = Quaternion.Euler(0, 0, 0);

        GameObject treePrefab;
        if (randomGen.Next(0, 100) > 50)
        {
            pos = pos + new Vector3(0, 0, -1.1f);
            treePrefab = foilage.tree1;           
        }
        else
            treePrefab = foilage.tree2;

        GameObject tree = Instantiate(treePrefab, pos, rot) as GameObject;
        tree.transform.parent = tile.transform;
    }

    void AddNeutralMobHome(GameObject tile)
    {
        Vector3 pos = tile.transform.position;
        Quaternion rot = Quaternion.Euler(270, 0, 0);

		int neutralHomeIndex = Random.Range (0, neutralMobFactory.neutralHomes.Length);
		Debug.Log ("Selecting neutral mob index : " + neutralHomeIndex);

		GameObject wh = Instantiate(neutralMobFactory.neutralHomes[neutralHomeIndex], pos, rot) as GameObject;
        wh.transform.parent = tile.transform;
    }

    void SpawnPlayer(int x, int z)
    {
        player.transform.position = new Vector3(x, (player.GetComponent<BoxCollider>().size.y/2), z);
        playerIsSpawned = true;
    }
    
    [System.Serializable]
    public class Tiles
    {
        public GameObject dirtTile;
        public GameObject waterTile;
        public GameObject grassTile;
    }

    [System.Serializable]
    public class Foilage
    {
        public GameObject tree1;
        public GameObject tree2;
    }

    [System.Serializable]
    public class NeutralMobFactory
    {
        public GameObject[] neutralHomes;
    }
}
