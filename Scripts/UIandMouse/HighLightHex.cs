using UnityEngine;
using System.Collections;

public class HighLightHex : MonoBehaviour {


	TileManager tM;

	public Biome biomeSelected;
	private BiomeTransistion biomeTransistionSelected;
	private BiomeFeatures biomeFeatureSelected;



	// Use this for initialization
	void Start () {
	
		biomeSelected = Biome.None;
		biomeTransistionSelected = BiomeTransistion.None;

	}
	
	// Update is called once per frame
	void Update () {
	
		mouseOverX();
		mouseDown();
	}

	public void addTM(TileManager nTM){
		tM = nTM;
	}

	void mouseOverX(){
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		Collider collider = GameObject.Find("BoardObject").GetComponent<Collider>();


		if(biomeTransistionSelected == BiomeTransistion.None){
			if(collider.Raycast(ray, out hitInfo, Mathf.Infinity)){
				
				
				gameObject.transform.position = tM.getTilePositionfromPixel(hitInfo.point.x,hitInfo.point.y);
				
				
			}
		}
		else {
			if(collider.Raycast(ray, out hitInfo, Mathf.Infinity)){

				gameObject.transform.position = tM.getEdgePositionfromPixel(hitInfo.point.x,hitInfo.point.y);

			}
		}


	}

	void mouseDown(){

		if(biomeSelected != Biome.None){
			if(Input.GetMouseButton(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				Collider collider = GameObject.Find("BoardObject").GetComponent<Collider>();
				
				if(collider.Raycast(ray, out hitInfo, Mathf.Infinity)){
					
					tM.changeTileBiome(hitInfo.point.x, hitInfo.point.y,biomeSelected);
					
				}
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////
		else if(biomeTransistionSelected != BiomeTransistion.None){

			if(Input.GetMouseButton(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				Collider collider = GameObject.Find("BoardObject").GetComponent<Collider>();
				
				if(collider.Raycast(ray, out hitInfo, Mathf.Infinity)){
					
					tM.changeEdgeType(hitInfo.point.x,hitInfo.point.y,BiomeTransistion.River);
					
				}
			}
			else if(Input.GetMouseButton(1)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				Collider collider = GameObject.Find("BoardObject").GetComponent<Collider>();
				
				if(collider.Raycast(ray, out hitInfo, Mathf.Infinity)){
					
					tM.changeEdgeType(hitInfo.point.x,hitInfo.point.y,BiomeTransistion.None);
					
				}
			}

		}
		else if(biomeFeatureSelected != BiomeFeatures.None){
			if(Input.GetMouseButton(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				Collider collider = GameObject.Find("BoardObject").GetComponent<Collider>();
				if(collider.Raycast(ray, out hitInfo, Mathf.Infinity)){
					tM.changeTileFeature(hitInfo.point.x, hitInfo.point.y,biomeFeatureSelected);
				}


			}
		}
		//////////////////////////////////////////////////////////////////////////////////////////



	}


	public void setOnClickAction(Biome biome){
		biomeSelected = biome;
		biomeTransistionSelected = BiomeTransistion.None;
		biomeFeatureSelected = BiomeFeatures.None;
	}

	public void setOnClickActionRiver(){


		biomeSelected = Biome.None;
		biomeFeatureSelected = BiomeFeatures.None;
		if(biomeTransistionSelected == BiomeTransistion.None){
			biomeTransistionSelected = BiomeTransistion.River;
		}
		else{
			biomeTransistionSelected = BiomeTransistion.None;
		}

	}

	public void setOnClickActionTerrain(BiomeFeatures feature){

		biomeSelected = Biome.None;
		biomeTransistionSelected = BiomeTransistion.None;
		biomeFeatureSelected = feature;
	}

}
