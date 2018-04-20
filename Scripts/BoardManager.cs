using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class BoardManager : MonoBehaviour {


	public int tileI;
	public int tileJ;

	public float tileSize;

	private BoardGraphics graphics;
	private TileManager tM;

	private List<Continent> continents;
	private List<Country> countries;



	// Use this for initialization
	void Awake(){

		float time = Time.realtimeSinceStartup;

		this.generateWorld();
		GameObject.Find ("Highlight Hex").GetComponent<HighLightHex>().addTM(tM);
		GameObject.Find("Main Camera").GetComponent<CameraMovement>().addRange(tM.getTileList()[tM.getTileList().Count-1].getPosition());

		Debug.Log(Time.realtimeSinceStartup - time);
	}

	private void generateWorld(){
		
		TempLoader temp = new TempLoader();
		graphics = GameObject.Find("BoardObject").GetComponent<BoardGraphics>();
		TileManager tileManager = new TileManager(tileI, tileJ, tileSize, graphics, temp);
		tM = tileManager;
		continents = new List<Continent>();



		generateContinents(tM.getTileList());
		generateCountries();


		
	}

	private void generateContinents(List<Tile> tileList){

		int worldTileI = tileI/4;
		int worldTileJ = tileJ/4;

		List<List<int>> worldTileList = MapGenerator.spiltWorld();


		for(int j = 0; j < worldTileList.Count; j++){
			List<int> firstWorldTile = worldTileList[j];

			int minI = 100;
			int minJ = 100;
			int maxI = 0;
			int maxJ = 0;

			for(int i = 0; i < firstWorldTile.Count; i++){

				int tileBlockI = firstWorldTile[i]%4;
				int tileBlockJ = firstWorldTile[i]/4;

				if(tileBlockI < minI){
					minI = tileBlockI;
				}
				if(tileBlockJ < minJ){
					minJ = tileBlockJ;
				}
				if(tileBlockI > maxI){
					maxI = tileBlockI;
				}
				if(tileBlockJ > maxJ){
					maxJ = tileBlockJ;
				}
			}

			minI = minI*worldTileI;
			minJ = minJ*worldTileJ;
			maxI = (maxI*worldTileI) + (worldTileI - 1);
			maxJ = (maxJ*worldTileJ) + (worldTileJ - 1);

			//Debug.Log ("I: " + minI + "-" + maxI + " J: " + minJ + "-" + maxJ);

			List<Tile> tileBlockList = new List<Tile>();

			for(int i = 0 ; i < tileList.Count; i++){

				Tile tile = tileList[i];

				if(tile.getI()>= minI && tile.getI() <= maxI && tile.getJ() >= minJ && tile.getJ() <= maxJ){
					tileBlockList.Add(tileList[i]);
				}
			}

			MapGenerator gen = new MapGenerator(tileBlockList,minI,minJ,maxI,maxJ);

			Continent continent = gen.generateContinent(tileList.Count, maxI - minI,maxJ - minJ, tileSize,firstWorldTile.Count*8,tM.getTileIJ(minI,minJ).getPosition().x, tM.getTileIJ(minI,minJ).getPosition().y);

			float time = Time.realtimeSinceStartup;
			continent.generateProvinces(firstWorldTile.Count*8);
			Debug.Log("Something: " + (Time.realtimeSinceStartup - time));
			continents.Add(continent);

		}

		Debug.Log("Number of Continents: " + continents.Count);



	}

	private void generateCountries(){

		List<int> list = new List<int>(24);

		for(int i = 0; i < 24; i++){
			list.Add(i);
		}

		for(int i = 0; i < continents.Count;i++){

			Continent continent = continents[i];
			int totalcontinentSize = continent.getProvinceNum()/4;
			int continentUnassigned = continent.getProvinceNum()/4;

			List<int> countryIDs = new List<int>();

			bool check = true;
			while(check){

				if(continentUnassigned == 1){
					countryIDs.Add(list[list.Count-1]);
					list.RemoveAt(list.Count-1);
					check = false;
				}
				else if(continentUnassigned == 0){
					check = false;
				}
				else{

					int rand = UnityEngine.Random.Range(0,list.Count);
					int countryID = list[rand];

					if(countryID < 8){
						continentUnassigned = continentUnassigned - 2;
						countryIDs.Add(countryID);
						list.RemoveAt(rand);
					}
					else{
						continentUnassigned = continentUnassigned - 1;
						countryIDs.Add(countryID);
						list.RemoveAt(rand);
					}
				}
			}

			String info = "";

			int eights = 0;
			int fours = 0;

			for(int j = 0; j < countryIDs.Count; j++){

				if(countryIDs[j] < 8){
					eights = eights + 1;
				}
				else{
					fours = fours + 1;
				}

				info = info + countryIDs[j] + ",";

			}

			continent.generateCountries(eights, fours);

			Debug.Log(info);
		}



	}



	void Start () {







	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
