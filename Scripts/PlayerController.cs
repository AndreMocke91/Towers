using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Camera mainCamera;
    [Range(0,20)]
    public float speed = 5f;

    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Vector3 vectorToTarget = objectHit.position - transform.position;
                vectorToTarget.Normalize();                   

                rigidBody.MovePosition(transform.position + vectorToTarget * Time.deltaTime * speed);
            }
        }
    }
}
