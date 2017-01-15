using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapGenerator : MonoBehaviour {
   
    public bool debug = false;    

	// GameObjects
	public GameObject hostileSpawner;
    public GameObject chunkHolder;    
    public GameObject player;
	public GameObject biome;
	public GameObject chunkPrefab;
	public BiomePrefabs biomePrefabs;
	public List<BiomePrefabs> biomeList;
	[Range (0,10000)]
	public int scale = 6;

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

	//map quadrants
	public int[,] biomeMapQuadrantOne; //-x +y
	public int[,] biomeMapQuadrantTwo; //+x +y
	public int[,] biomeMapQuadrantThree; //+x -y
	public int[,] biomeMapQuadrantFour; //-x -y

	// privates
	bool playerIsSpawned = false;
	System.Random randomGen = new System.Random();
	int[,] chunkMapGrid;
	WaveSpawner waveSpawner;
	System.Random seedGen;
	float biomeNoiseInterval;
	float[] biomeNoiseTopRange;
	int perlinSeed;

    void Start ()
    {
        if (debug)
            Debug.Log("Starting map generation...");

		waveSpawner = hostileSpawner.GetComponent<WaveSpawner> ();
		chunkHolder.transform.position = new Vector3(-chunkDiameter / 2, 0, -chunkDiameter / 2);
		seedGen = new System.Random ();

		biomeNoiseInterval = 1 / biomeList.Count;
		biomeNoiseTopRange = new float[biomeList.Count];

		for (int i = 0; i < biomeList.Count; i++) {
			biomeNoiseTopRange [i] = biomeNoiseInterval + biomeNoiseInterval * i;
		}

		biomePrefabs = biomeList [seedGen.Next (0, biomeList.Count)];

		if (biomePrefabs != null) {
			GenerateMap ();
			perlinSeed = seedGen.Next (arrDimensions);
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
		if (xStartCoordinate < 0 && zStartCoordinate >= 0 
			&& mapQuadrantOne.GetLength (0) > Math.Abs (xStartCoordinate) + chunkDiameter
			&& mapQuadrantOne.GetLength (1) > Math.Abs (zStartCoordinate) + chunkDiameter) {
			//Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 1");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantOne);
		} else if (xStartCoordinate >= 0 && zStartCoordinate >= 0
			&& mapQuadrantTwo.GetLength (0) > Math.Abs (xStartCoordinate) + chunkDiameter
			&& mapQuadrantTwo.GetLength (1) > Math.Abs (zStartCoordinate) + chunkDiameter) {
			//Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 2");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantTwo );
		} else if (xStartCoordinate >= 0 && zStartCoordinate < 0 
			&& mapQuadrantThree.GetLength (0) > Math.Abs (xStartCoordinate) + chunkDiameter
			&& mapQuadrantThree.GetLength (1) > Math.Abs (zStartCoordinate) + chunkDiameter) {
			//Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 3");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantThree);
		} else if(xStartCoordinate - chunkDiameter > -arrDimensions 
			&& mapQuadrantFour.GetLength (0) > Math.Abs (xStartCoordinate) + chunkDiameter
			&& mapQuadrantFour.GetLength (1) > Math.Abs (zStartCoordinate) + chunkDiameter) {
			//Debug.Log ("Generating chunk at : " + xStartCoordinate + "," +zStartCoordinate + " inside quad 4");
			RenderMapPreFabs (xStartCoordinate, zStartCoordinate, mapQuadrantFour);
		} else 	{
			Debug.Log("Nearing edge of map...");
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

	int GetNeighbouringWallCount(int xCoor, int zCoor, int[,] quad)
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
				if (quad.GetLength (0) > Math.Abs (xStartCoordinate) + x
				    && quad.GetLength (1) > Math.Abs (zStartCoordinate) + z) {

					Vector3 pos = new Vector3 (x + xStartCoordinate, 0, z + zStartCoordinate);
					Quaternion rot = Quaternion.Euler (90, 0, 0);

					float perlinNoiseValue = (Mathf.PerlinNoise ((pos.x + perlinSeed) / scale, (pos.z + perlinSeed) / scale));
					SelectBiome (perlinNoiseValue);


					if (quad [Math.Abs (xStartCoordinate) + x, Math.Abs (zStartCoordinate) + z] == 1) {
						GameObject tile = Instantiate (biomePrefabs.inpassableTiles [randomGen.Next (0, biomePrefabs.inpassableTiles.Count)], pos, rot) as GameObject;
						tile.transform.parent = newChunk.transform;
						newChunk.GetComponent<Chunk> ().tiles.Add (tile);
					} else {
						GameObject tile = Instantiate (biomePrefabs.passableTiles [randomGen.Next (0, biomePrefabs.passableTiles.Count)], pos, rot) as GameObject;
						tile.transform.parent = newChunk.transform;
						newChunk.GetComponent<Chunk> ().tiles.Add (tile);

						if (x > (chunkDiameter / 2) && z > chunkDiameter / 2 && !playerIsSpawned)
							SpawnPlayer (x, z);

						if (randomGen.Next (0, 100) < biomePrefabs.foilagePercentage)
							AddFoilageObject (tile);
						else if (randomGen.Next (0, 500) < biomePrefabs.neutralMobHomesPercentage)
							AddNeutralMobHome (tile);

					}
				} else {
					Debug.Log ("Fuck");
				}
             
            }

		chunkHolder.GetComponent<ChunkHolder> ().chunks.Add (new Vector2 (xStartCoordinate, zStartCoordinate), newChunk);
		newChunk.name = xStartCoordinate.ToString () + "," + zStartCoordinate.ToString ();
    }

	void SelectBiome(float perlinNoise)
	{		
		for (int i = 0; i < biomeList.Count; i++) {
			if (perlinNoise < biomeNoiseTopRange [i]) {
				biomePrefabs = biomeList [i];
				return;
			}
		}

		if (perlinNoise > 0.5)
			biomePrefabs = biomeList [0];
		else
			biomePrefabs = biomeList [1];
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
