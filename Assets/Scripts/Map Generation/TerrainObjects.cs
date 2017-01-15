using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TerrainObjects  {

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
		public List<GameObject> foilagePrefabs;
	}

	[System.Serializable]
	public class NeutralMobFactory
	{
		public GameObject[] neutralMobHomePrefabs;
	}
}
