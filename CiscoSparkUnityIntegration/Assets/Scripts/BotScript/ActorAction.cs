using UnityEngine;
using System.Collections;

public class ActorAction : MonoBehaviour {

    private bool move = false;
    //[]
    private Vector3 destination;
    public float smoothTime = 0.5F;
	// Use this for initialization
	void Start () {
        move = false;
        destination = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if(move)
            transform.position = Vector3.Lerp(transform.position,
                                    destination,
                                        smoothTime * Time.deltaTime);
        if (transform.position == destination)
        {
            move = false;
            Debug.Log("End ");
        }

	}
    public void Move() {
        move = true;
    }
    public void GoTo(Vector3 dest)
    {
        destination = dest;
    }
}
