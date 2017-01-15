using UnityEngine;
using System.Collections;

[System.Serializable]
public class TraversableNode{

	public int g,h;
	public float f;
	public Vector3 pos;
	public TraversableNode previousNode;

	public TraversableNode(Vector3 _pos, Vector3 origin, Vector3 target, TraversableNode _previousNode)
	{
		g = Utils.GetASharpDistance (_pos, origin);
		h = Utils.GetASharpDistance (_pos, target);
		previousNode = _previousNode;
		pos = new Vector3 (_pos.x, 0, _pos.z);
		f = (_pos - target).sqrMagnitude;
	}


}
