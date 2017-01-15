using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiomePrefabs : MonoBehaviour {
		
		public List<GameObject> passableTiles;
		public List<GameObject> inpassableTiles;

		public List<GameObject> foilagePrefabs;
		public List<GameObject> obstaclePrefabs;
		public List<GameObject> neutralMobHomePrefabs;

		[Range(0,100)]
		public float fillPercentage;
		[Range(0, 100)]
		public float foilagePercentage;
		[Range(0, 100)]
		public float neutralMobHomesPercentage;

}
