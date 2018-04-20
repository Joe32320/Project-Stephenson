using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float speed;

	public float scrollSpeed;

	public float maxZoom;
	public float minZoom;

	float cameraDistance = 50f;
	private float maxX;
	private float maxY;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		float h = speed * Input.GetAxis ("Horizontal");
		float v = speed * Input.GetAxis("Vertical");



		//float x = 1f * Input.GetAxis("Mouse ScrollWheel");

		transform.Translate(h,v,0);

		if(transform.position.x > maxX){
			transform.position = new Vector3(0f,transform.position.y,-10f);
		}
		if(transform.position.x < 0){
			transform.position = new Vector3(maxX,transform.position.y,-10f);
		}
		if(transform.position.y > maxY){
			transform.position = new Vector3(transform.position.x,maxY,-10f);
		}
		if(transform.position.y  < 0){
			transform.position = new Vector3(transform.position.x,0,-10f);
		}

		cameraDistance += -Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;

		cameraDistance = Mathf.Clamp(cameraDistance, minZoom, maxZoom);

		Camera.main.orthographicSize = cameraDistance;
	}

	public void addRange(Vector3 vec){
		maxX = vec.x;
		maxY = vec.y;
	}

}
