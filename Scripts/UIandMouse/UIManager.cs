using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	private bool biomeButtonsShowing;
	private bool featureButtonsShowing;

	private GameObject grassland;
	private GameObject ocean;
	private GameObject ice;
	private GameObject desert;

	private GameObject plains;
	private GameObject forest;
	private GameObject hill;
	private GameObject mountain;

	private HighLightHex mouseControl;

	void Start(){

		mouseControl = GameObject.Find("Highlight Hex").GetComponent<HighLightHex>();

		grassland = GameObject.Find("Grassland Button");		
		desert = GameObject.Find("Desert Button");
		ocean = GameObject.Find("Ocean Button");
		ice = GameObject.Find("Ice Button");

		plains = GameObject.Find("Plain Button");
		forest = GameObject.Find ("Forest Button");
		hill = GameObject.Find ("Hill Button");
		mountain = GameObject.Find("Mountain Button");

		biomeButtonsShowing = true;
		pressedBiome();
		featureButtonsShowing = true;
		pressedTerrain();





	}


	public void pressedBiome(){

		if(biomeButtonsShowing==false){

			grassland.SetActive(true);
			desert.SetActive(true);
			ocean.SetActive(true);
			ice.SetActive(true);
			biomeButtonsShowing = true;
			mouseControl.setOnClickAction(Biome.None);



		}
		else{
			grassland.SetActive(false);
			desert.SetActive(false);
			ocean.SetActive(false);
			ice.SetActive(false);

			mouseControl.setOnClickAction(Biome.None);

			biomeButtonsShowing = false;

		}

		if(featureButtonsShowing == true){
			pressedTerrain();
		}



	}


	public void pressedGrassLand(){
		mouseControl.setOnClickAction(Biome.Grassland);
	}

	public void pressedDesert(){
		mouseControl.setOnClickAction(Biome.Desert);
	}

	public void pressedIce(){
		mouseControl.setOnClickAction(Biome.Ice);
	}

	public void pressedOcean(){
		mouseControl.setOnClickAction(Biome.Ocean);
	}

	public void pressedRiver(){

		if(biomeButtonsShowing){
			pressedBiome();
		}
		mouseControl.setOnClickActionRiver();
	}

	public void pressedTerrain(){

		if(featureButtonsShowing==false){
			
			plains.SetActive(true);
			hill.SetActive(true);
			forest.SetActive(true);
			mountain.SetActive(true);
			featureButtonsShowing = true;
			mouseControl.setOnClickActionTerrain(BiomeFeatures.None);
			
		}
		else{
			plains.SetActive(false);
			hill.SetActive(false);
			forest.SetActive(false);
			mountain.SetActive(false);
			
			featureButtonsShowing = false;
			mouseControl.setOnClickActionTerrain(BiomeFeatures.None);
			
		}

		if(biomeButtonsShowing){
			pressedBiome();
		}


	}

	public void pressedPlain(){
		mouseControl.setOnClickActionTerrain(BiomeFeatures.Clear);
		Debug.Log("pressed");

	}

	public void pressedHill(){
		mouseControl.setOnClickActionTerrain(BiomeFeatures.Hill);
		Debug.Log("pressed");
	}

	public void pressedForest(){
		mouseControl.setOnClickActionTerrain(BiomeFeatures.Forest);
		Debug.Log("pressed");
	}

	public void pressedMountain(){
		mouseControl.setOnClickActionTerrain(BiomeFeatures.Mountain);
		Debug.Log("pressed");
	}






}
