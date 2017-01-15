using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public static class Utils {

	public static void RotateModel(GameObject subject, GameObject target, float speed)
	{	
		Vector3 targetDirection = target.transform.position - subject.transform.position;
		Vector3 newDirection = Vector3.RotateTowards (subject.transform.forward, targetDirection, speed, 0.0f);
		//Debug.DrawRay (subject.transform.position, newDirection, Color.red, 15.0f);
		newDirection.Set (newDirection.x/2, 0, newDirection.z/2);
		subject.transform.rotation = Quaternion.LookRotation (newDirection);
	}

	public static void RotatePopUpMenu(GameObject subject, GameObject target, float speed)
	{	
		Vector3 targetDirection = target.transform.position - subject.transform.position;
		Vector3 newDirection = Vector3.RotateTowards (subject.transform.forward, targetDirection, speed, 0.0f);
		//Debug.DrawRay (subject.transform.position, newDirection, Color.red, 15.0f);
		newDirection.Set (-newDirection.x/2, -newDirection.y/2, -newDirection.z/2);
		subject.transform.rotation = Quaternion.LookRotation (newDirection);
	}

	public static void RotateModel(GameObject subject, Vector3 target, float speed)
	{	
		Vector3 targetDirection = target - subject.transform.position;
		Vector3 newDirection = Vector3.RotateTowards (subject.transform.forward, targetDirection, speed, 0.0f);
		//Debug.DrawRay (subject.transform.position, newDirection, Color.red, 15.0f);
		newDirection.Set (newDirection.x/2, 0, newDirection.z/2);
		subject.transform.rotation = Quaternion.LookRotation (newDirection);
	}

	public static GameObject GetTileFromTransformPosition(Vector3 _position)
	{
		GameObject underFootTile = 
			Physics.OverlapSphere (new Vector3 (Mathf.Floor(_position.x), 0, Mathf.Floor(_position.z)), 0.1f /*Radius*/)
				.Where (x => x.gameObject.tag == "BuildableTile")
				.Select (c => c.gameObject)
				.FirstOrDefault ();

		return underFootTile;
	}

	public static int GetASharpDistance(Vector3 origin, Vector3 target)
	{
		//Debug.Log ("Getting distance from : " + origin.ToString () + " to " + target.ToString());
		Vector3 distanceVector = origin - target;
		return Mathf.Abs ((int)Mathf.Round (distanceVector.x)) + Mathf.Abs ((int)Mathf.Round (distanceVector.z));
	}

	public static List<TraversableNode> GetASharpPath(Vector3 origin, Vector3 target, Material pathMaterial)
	{
		Debug.Log ("Finding A* path from : " + origin.ToString () + " to " + target.ToString ());
		target = new Vector3 (Mathf.Floor(target.x), 0, Mathf.Floor(target.z));
		origin = new Vector3 (Mathf.Floor(origin.x), 0, Mathf.Floor(origin.z));
		List<TraversableNode> openNodes = new List<TraversableNode> ();
		List<TraversableNode> closedNodes = new List<TraversableNode> ();
		TraversableNode originNode = new TraversableNode (origin, origin, target, null);
		TraversableNode currentNode;
		bool pathFound = false;

		openNodes.Add (originNode);

		do {
			
			currentNode = openNodes.OrderBy (x => x.f).FirstOrDefault ();
			//Debug.Log("Current node coordinates " + currentNode.pos.ToString());
			closedNodes.Add (currentNode);
			openNodes.Remove (currentNode);

			if(closedNodes.Where(x => x.pos == target).FirstOrDefault() != null)
			{
				pathFound = true;
				//Debug.Log(closedNodes.Where(x => x.pos == target).First().pos.ToString());
				break;
			}

			List<TraversableNode> tempNodes = new List<TraversableNode>();

			tempNodes.Add( new TraversableNode(new Vector3(currentNode.pos.x, 0, currentNode.pos.z +1),origin,target,currentNode));
			tempNodes.Add( new TraversableNode(new Vector3(currentNode.pos.x, 0, currentNode.pos.z -1),origin,target,currentNode));
			tempNodes.Add( new TraversableNode(new Vector3(currentNode.pos.x-1, 0, currentNode.pos.z),origin,target,currentNode));
			tempNodes.Add( new TraversableNode(new Vector3(currentNode.pos.x+1, 0, currentNode.pos.z),origin,target,currentNode));

			foreach(TraversableNode tv in tempNodes)
			{
				GameObject tile;
				if(tile = GetTileFromTransformPosition(tv.pos)){						
					if(tile.transform.childCount == 0){
													
						TraversableNode newNode = new TraversableNode(tile.transform.position,origin,target,currentNode);

						//Debug.Log("Nodes in openlist : " + openNodes.Count);
						//Debug.Log("Checking tile at position : " + tile.transform.position.ToString());
						//Debug.Log("newNode position = " + newNode.pos.ToString());

						if(openNodes.Where(x => x.pos == newNode.pos).FirstOrDefault() == null){
							if(closedNodes.Where(x => x.pos == newNode.pos).FirstOrDefault() == null){
								//Debug.Log("Adding node : " + newNode.pos.ToString() + " with f value of " + newNode.f.ToString());
								openNodes.Add(newNode);

							}else{
								//Debug.Log("Found in closed List");
							}
						}else{
							//Debug.Log("Found in open List");
						}
					}
				}
			}

			//Debug.Log("..................................");

		} while(openNodes.Count > 0);

		List<TraversableNode> path = new List<TraversableNode> ();

		while (currentNode.previousNode != null) {
			path.Add (currentNode);
			currentNode = currentNode.previousNode;
			GameObject tile = GetTileFromTransformPosition (currentNode.pos);
			tile.GetComponent<Renderer> ().material = pathMaterial;

		}

		if (path.Count > 0)
		if (pathFound)
			return path;
		else
			Debug.Log ("Go berserk");
		else {
			Debug.Log ("Go berserk");
		}

		return null;

	}

}
