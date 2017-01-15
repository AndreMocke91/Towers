using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {
   
    public bool debug = false;    

	// GameObjects
	public GameObject hostileSpawner;
    public GameObject chunkHolder;    
    public GameObject player;
	public GameObject biome;
	public GameObject chunkPrefab;
	public BiomePrefabs biomePrefabs;

	//map variables
	public int chunkDiameter;
	public string seed = "seed";
	[Range(20,500)]
	public int mapChunkRadius = 20;
	public int arrDimensions;
	//map quadrants
	public int[,] mapQuadrantOne; //-x +y
	public int[,] mapQuadrantTwo; //+x +y
	public int[,] mapQuadrantThree; //+x -y
	public int[,] mapQuadrantFour; //-x -y

	// privates
	bool playerIsSpawned = false;
	System.Random randomGen = new System.Random();
	int[,] chunkMapGrid;
	WaveSpawner waveSpawner;
	System.Random seedGen;


    void Start ()
    {
        if (debug)
            Debug.Log("Starting map generation...");

		waveSpawner = hostileSpawner.GetComponent<WaveSpawner> ();
		chunkHolder.transform.position = new Vector3(-chunkDiameter / 2, 0, -chunkDiameter / 2);
		biomePrefabs = biome.GetComponent<BiomePrefabs> ();
		seedGen = new System.Random ();

		if (biomePrefabs != null) {
			GenerateMap ();
			GenerateChunk (0, 0);
			chunkHolder.GetComponent<ChunkHolder> ().UpdateChunksAroundPlayer (2);
		}
    }

	public void GenerateMap()
	{
		GetSeed ();

		arrDimensions = chunkDiameter * mapChunkRadius;
		mapQuadrantOne = GenerateQuadrant (new int[arrDimensions, arrDimensions], arrDimensions);
		mapQuadrantTwo = GenerateQuadrant (new int[arrDimensions, arrDimensions], arrDimensions);
		mapQuadrantThree = GenerateQuadrant (new int[arrDimensions, arrDimensions], arrDimensions);
		mapQuadrantFour = GenerateQuadrant (new int[arrDimensions, arrDimensions], arrDimensions);
	}

	public int[,] GenerateQuadrant(int [,] quad, int arrDimensions)
	{			
		for(int x = 0; x < arrDimensions; x++)
			for(int z = 0; z < arrDimensions; z++)
			{				
				if(seedGen.Next(0,100) >= biomePrefabs.fillPercentage)
				{
					quad[x, z] = 1;
				}
				else
				{
					quad[x, z] = 0;
				}
			}

		for (int i = 0; i < 5; i++) {
			SmoothMap (arrDimensions, quad);
		}

		return quad;
	}
    	
	public void GenerateChunk(int xStartCoordinate, int zStartCoordinate)
    {          
		if (xStartCoordinate < 0 && zStartCoordinate >= 0) {
			Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 1");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantOne);
		} else if (xStartCoordinate >= 0 && zStartCoordinate >= 0) {
			Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 2");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantTwo);
		} else if (xStartCoordinate >= 0 && zStartCoordinate < 0) {
			Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 3");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantThree);
		} else {
			Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 4");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantFour);
		}


    }
	    
	void SmoothMap(int arrDimensions, int[,] quad)
    {
		for (int x = 0; x < arrDimensions; x++) {
			for (int z = 0; z < arrDimensions; z++) {
				int neighbourWallTiles = GetNeighbouringWallCount (x, z, quad);

				if (neighbourWallTiles > 4)
					quad [x, z] = 1;
				else if (neighbourWallTiles < 4)
					quad [x, z] = 0;

			}
		}
    }

	private int GetNeighbouringWallCount(int xCoor, int zCoor, int[,] quad)
	{
		int wallCount = 0;
		for (int neighbourX = xCoor - 1; neighbourX <= xCoor + 1; neighbourX++)
		{
			for (int neighbourZ = zCoor - 1; neighbourZ <= zCoor + 1; neighbourZ++)
			{
				if (neighbourX >= 0 && neighbourX < arrDimensions && neighbourZ >= 0 && neighbourZ < arrDimensions)
				{
					if (neighbourX != xCoor || neighbourZ != zCoor)
					{
						wallCount += quad[neighbourX, neighbourZ];
					}
				}
				else
				{
					//wallCount++;
				}
			}
		}

		return wallCount;
	}

	void RenderMapPreFabs(int xStartCoordinate, int zStartCoordinate, int[,] quad)
    {
		GameObject newChunk = Instantiate (chunkPrefab, new Vector3 (xStartCoordinate, 0, zStartCoordinate), Quaternion.identity) as GameObject;
		newChunk.transform.parent = chunkHolder.transform;
		newChunk.GetComponent<Chunk> ().origin = new Vector2 (xStartCoordinate, zStartCoordinate);

		for (int x = 0; x < chunkDiameter; x++)
			for (int z = 0; z < chunkDiameter; z++)
            {
				Vector3 pos = new Vector3(x + xStartCoordinate, 0, z + zStartCoordinate);
                Quaternion rot = Quaternion.Euler(90, 0, 0);

				if (quad[Math.Abs(xStartCoordinate) + x, Math.Abs(zStartCoordinate) + z] == 1)
                {
					GameObject tile = Instantiate(biomePrefabs.inpassableTiles[randomGen.Next(0, biomePrefabs.inpassableTiles.Count)], pos, rot ) as GameObject;
					tile.transform.parent = newChunk.transform;
					newChunk.GetComponent<Chunk> ().tiles.Add (tile);
                }
                else
                {
					GameObject tile = Instantiate(biomePrefabs.passableTiles[randomGen.Next(0,biomePrefabs.passableTiles.Count)], pos, rot) as GameObject;
					tile.transform.parent = newChunk.transform;
					newChunk.GetComponent<Chunk> ().tiles.Add (tile);

					if (x > (chunkDiameter / 2) && z > chunkDiameter / 2 && !playerIsSpawned)
                        SpawnPlayer(x, z);

					if (randomGen.Next(0, 100) < biomePrefabs.foilagePercentage)
                        AddFoilageObject(tile);
					else if (randomGen.Next(0, 500) < biomePrefabs.neutralMobHomesPercentage)
                        AddNeutralMobHome(tile);

                }
             
            }

		chunkHolder.GetComponent<ChunkHolder> ().chunks.Add (new Vector2 (xStartCoordinate, zStartCoordinate), newChunk);
		newChunk.name = xStartCoordinate.ToString () + "," + zStartCoordinate.ToString ();
    }

    void AddFoilageObject(GameObject tile)
    {
        Vector3 pos = tile.transform.position;
		Quaternion rot = Quaternion.Euler(0, randomGen.Next(0,180), 0);

        GameObject treePrefab;

		treePrefab = biomePrefabs.foilagePrefabs [randomGen.Next (0, biomePrefabs.foilagePrefabs.Count)];
			
        GameObject tree = Instantiate(treePrefab, pos, rot) as GameObject;
        tree.transform.parent = tile.transform;
    }

    void AddNeutralMobHome(GameObject tile)
    {
        Vector3 pos = tile.transform.position;
        Quaternion rot = Quaternion.Euler(270, 0, 0);

		GameObject wh = Instantiate(biomePrefabs.neutralMobHomePrefabs[randomGen.Next(0,biomePrefabs.neutralMobHomePrefabs.Count)], pos, rot) as GameObject;
        wh.transform.parent = tile.transform;
		waveSpawner.neutralMobSpawners.Add (wh.GetComponent<NeutralMobSpawner> ());
    }

    void SpawnPlayer(int x, int z)
    {
        player.transform.position = new Vector3(x, (player.GetComponent<BoxCollider>().size.y/2), z);
        playerIsSpawned = true;
    }   

	void GetSeed()
	{
		if(seed == "seed")
		{
			seedGen = new System.Random(randomGen.Next(0,100).GetHashCode());
		}
		else
		{
			seedGen = new System.Random(seed.GetHashCode());
		}

		if(debug)
			Debug.Log("Using seed : " + seedGen.ToString());
	}
    
}
