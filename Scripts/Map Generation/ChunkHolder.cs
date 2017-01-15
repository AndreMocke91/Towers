using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class ChunkHolder : MonoBehaviour {

	public Dictionary<Vector2,GameObject> chunks;
	public GameObject player;
	public Vector2 underFootChunkOrigin;
	public MapGenerator mapGenerator;
	public int chunkGenRadius = 2;

	public Vector2 oldUnderFootChunkOrigin;
	int chunkDiameter;

	void Start()
	{
		chunks = new Dictionary<Vector2, GameObject> ();
		GetUnderFootChunkOrigin ();
		oldUnderFootChunkOrigin = underFootChunkOrigin;
		chunkDiameter = mapGenerator.chunkDiameter;

	}

	void FixedUpdate()
	{
		GetUnderFootChunkOrigin ();
		if (oldUnderFootChunkOrigin != underFootChunkOrigin) {
			if(mapGenerator.biomePrefabs != null)
				UpdateChunksAroundPlayer (chunkGenRadius);
		}
	}

	void GetUnderFootChunkOrigin()
	{
		int floorX = (int)Math.Floor((player.transform.position.x ) / chunkDiameter) * chunkDiameter;
		int floorZ = (int)Math.Floor((player.transform.position.z ) / chunkDiameter) * chunkDiameter;

		underFootChunkOrigin.x = floorX;
		underFootChunkOrigin.y = floorZ;
	}

	public void UpdateChunksAroundPlayer(int radius)
	{
		for (int x = -chunkDiameter * radius + (int)Math.Round(underFootChunkOrigin.x);
			x < chunkDiameter * radius + (int)Math.Round(underFootChunkOrigin.x);
			x = x + chunkDiameter) {
			for (int z = -chunkDiameter * radius + (int)Math.Round(underFootChunkOrigin.y);
				z < chunkDiameter * radius + (int)Math.Round(underFootChunkOrigin.y);
					z = z + chunkDiameter) {

				if (chunks.ContainsKey (new Vector2 (x, z))) {
					chunks [new Vector2 (x, z)].gameObject.SetActive (true);
				} else {
					mapGenerator.GenerateChunk (x, z);
				}

				// here comes the unloading of chunks
				Dictionary<Vector2,GameObject> inRangeChunks = InRangeChunks(chunkGenRadius);
				foreach(var kvp in chunks.Where(p=> !inRangeChunks.ContainsKey(p.Key)).ToDictionary (p => p.Key, p => p.Value))
				{
					kvp.Value.gameObject.SetActive (false);
				}

				}
		}
		oldUnderFootChunkOrigin = underFootChunkOrigin;
	}

	Dictionary<Vector2,GameObject> InRangeChunks(int chunkLoadingRange)
	{
		Dictionary<Vector2,GameObject> inRangeChunks = chunks
			.Where (x => x.Key.x < underFootChunkOrigin.x + chunkDiameter * chunkLoadingRange && //smaller than max x
		     x.Key.x > underFootChunkOrigin.x - chunkDiameter * chunkLoadingRange) //bigger than min x
			.Where (x => x.Key.y < underFootChunkOrigin.y + chunkDiameter * chunkLoadingRange && //smaller than max y
		     x.Key.y > underFootChunkOrigin.y - chunkDiameter * chunkLoadingRange) //bigger than min y
			.ToDictionary (p => p.Key, p => p.Value);
		return inRangeChunks;
	}




}
