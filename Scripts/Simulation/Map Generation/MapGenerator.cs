using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MapGenerator{


	private List<Tile> tileList;
	private Dictionary<Tile,float> heightMap;
	private float landPercentage;
	private int noLandTiles;
	private int noHillTiles;
	private int noMountainTiles;
	private int noRivers;
	private int minI;
	private int minJ;
	private int maxI;
	private int maxJ;

	private List<Tile> landTilesList;
	private List<Tile> coastalTileList;

	private float heightPercentage;

	public MapGenerator(List<Tile> nTileList, int nMinI, int nMinJ, int nMaxI, int nMaxJ){

		tileList = nTileList; 

		landTilesList = new List<Tile>(576);

		minI = nMinI;
		minJ = nMinJ;
		maxI = nMaxI;
		maxJ = nMaxJ;

		landPercentage = 0.384f;
		heightPercentage = 0.2f;


		heightMap = new Dictionary<Tile, float>(576);

		noLandTiles = Mathf.CeilToInt(landPercentage*(float)tileList.Count);
		noHillTiles = Mathf.CeilToInt(heightPercentage * (float) noLandTiles);
		noMountainTiles = Mathf.CeilToInt(heightPercentage * 0.333333f * (float) noLandTiles);

		noRivers = Mathf.CeilToInt((float)noLandTiles/50f);

		//Debug.Log (noLandTiles + ":" + noHillTiles + ":" + noMountainTiles);
	}


	public static List<List<int>> spiltWorld(){

		Dictionary<int, bool> occupied = new Dictionary<int, bool>(16);
		List<List<int>> worldTileList = new List<List<int>>(16);

		for(int i = 0; i < 16; i++){
			occupied.Add(i,false);

		}

		//Pick first 4 tile section
		bool check = true;
		int firstFour = 0;

		while(check){
			firstFour = UnityEngine.Random.Range(0,16);

			if(firstFour <= 10 && firstFour%4!=3){
				check = false;
			}
		}

		//test
		//firstFour = 0;

		List<int> firstFourList = new List<int>(4);

		firstFourList.Add (firstFour);
		firstFourList.Add(firstFour+1);
		firstFourList.Add(firstFour+4);
		firstFourList.Add (firstFour+5);

		worldTileList.Add (firstFourList);

		occupied[firstFour] = true;
		occupied[firstFour+1] = true;
		occupied[firstFour+4] = true;
		occupied[firstFour+5] = true;

		//Pick Second 4 tile section if possible
		check = true;
		int secondFour = 0;

		if(firstFour != 5){

			while(check){
				secondFour = UnityEngine.Random.Range(0,16);
				
				if(secondFour <= 10 && secondFour%4!=3){
					if(!occupied[secondFour]&&!occupied[secondFour+1]&&!occupied[secondFour+4]&&!occupied[secondFour+5]){
						check = false;
					}
				}
			}

			List<int> secondFourList = new List<int>(4);

			secondFourList.Add (secondFour);
			secondFourList.Add (secondFour+1);
			secondFourList.Add (secondFour+4);
			secondFourList.Add (secondFour+5);

			worldTileList.Add (secondFourList);

			occupied[secondFour] = true;
			occupied[secondFour+1] = true;
			occupied[secondFour+4] = true;
			occupied[secondFour+5] = true;

		}

		//Pick 2 tile sections, any that can't be assigned are 1 tile sections
		check = true;
		int ran = UnityEngine.Random.Range(0,100);
		int num1tile = 2;

		if(ran < 25){
			num1tile = 0;
		}
		if(ran>90){
			num1tile = 4;
		}

		//Debug.Log ("Num 1 tiles: " + num1tile);

		while(check){

			int worldTileNum = UnityEngine.Random.Range(0,16);

			if(!occupied[worldTileNum]){

				bool viableNeighbour = false;
				List<int> numList = new List<int>(4);

				for(int i =0; i < 4; i++){
					numList.Add (i);
				}

				for(int i = 0; i < 4; i++){

					int random = UnityEngine.Random.Range(0,numList.Count);
					int direction = numList[random];
					numList.RemoveAt(random);

					switch(direction){
					case 0:
						//lookSouth
						if(worldTileNum - 4 >= 0 && !occupied[worldTileNum - 4]){
							List<int> newTileList = new List<int>(2);
							newTileList.Add (worldTileNum - 4);
							newTileList.Add(worldTileNum);
							worldTileList.Add(newTileList);
							
							occupied[worldTileNum - 4] = true;
							occupied[worldTileNum] = true;
							viableNeighbour = true;
						}
						break;
					case 1:
						//lookNorth
						if(worldTileNum + 4 < 16 && !occupied[worldTileNum+4]){
							List<int> newTileList = new List<int>(2);
							newTileList.Add (worldTileNum);
							newTileList.Add(worldTileNum + 4);
							worldTileList.Add(newTileList);
							
							occupied[worldTileNum] = true;
							occupied[worldTileNum + 4] = true;
							viableNeighbour = true;
						}
						break;
					case 2:
						//lookEast
						if(worldTileNum + 1 < 16 && (worldTileNum +1)%4!=0&&!occupied[worldTileNum+1]){
							List<int> newTileList = new List<int>(2);
							newTileList.Add (worldTileNum);
							newTileList.Add(worldTileNum + 1);
							worldTileList.Add(newTileList);
							
							occupied[worldTileNum] = true;
							occupied[worldTileNum + 1] = true;
							viableNeighbour = true;
						}
						break;
					case 3:
						//lookWest
						if(worldTileNum - 1 >= 0 && (worldTileNum - 1)%4!=3&&!occupied[worldTileNum-1]){
							List<int> newTileList = new List<int>(2);
							newTileList.Add(worldTileNum - 1);
							newTileList.Add (worldTileNum);
							worldTileList.Add(newTileList);
							
							occupied[worldTileNum - 1] = true;
							occupied[worldTileNum] = true;
							viableNeighbour = true;
						}
						break;

					}

					if(viableNeighbour){
						break;
					}
				}

				if(!viableNeighbour){
					List<int> newTileList = new List<int>(1);
					newTileList.Add (worldTileNum);
					worldTileList.Add(newTileList);

					occupied[worldTileNum] = true;
				}

			}

			bool areAllTilesSorted = true;
			int count = 0; 

			for(int i = 0; i < 16; i++){
				if(!occupied[i]){
					areAllTilesSorted = false;
					count = count + 1;
				}
			}

			if(count<=num1tile){
				for(int i = 0; i < 16; i++){
					if(!occupied[i]){
						List<int> list = new List<int>(16);
						list.Add (i);
						worldTileList.Add (list);
						occupied[i] = true;
					}
				}
				check = false;
				break;
			}

			if(areAllTilesSorted){
				check = false;
				break;
			}
		}

		/*for(int i = 0; i < worldTileList.Count; i++){
			List<int> list = worldTileList[i];
			string s = "";
			for(int j = 0; j < list.Count; j++){
				s = s + list[j] + ",";
			}
			//Debug.Log (s);
		}*/

		return worldTileList;

	}




	public Continent generateContinent(int numTiles, int tileI, int tileJ, float tileSize, int numProvinces, float minX, float minY){

		bool check = true;
		while(check){
			reset();
			check = !buildMap(tileI, tileJ, tileSize, minX, minY);

			if(check == false){
				buildRiver();
				//this.multiplicativeNoise(0.25f);
				buildHillsandMountains();

			}
		}

		Continent continent = new Continent(landTilesList);

		return continent;

	}

	public bool buildMap(int tileI, int tileJ, float tileSize, float minX, float minY){

		for(int i = 0; i < tileList.Count; i++){
			heightMap.Add (tileList[i],0f);
		}

		this.buildSineWave(tileI, tileJ, tileSize, minX, minY);

		this.multiplicativeNoise(5f);
		/*this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);*/
		this.smooth(1);
		this.multiplicativeNoise(5f);
		/*this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);
		this.multiplicativeNoise(0.5f);*/
		this.smooth(1);
		this.discourageEdges();
		this.buildLand();

		return true;
	}

	public void reset(){

		for(int i = 0; i < tileList.Count; i++){
			tileList[i].changeBiome(Biome.Ocean);
			tileList[i].changeBiomeFeature(BiomeFeatures.Clear);
		}

		heightMap = new Dictionary<Tile, float>();
	}

	public void buildSineWave(int tileI, int tileJ, float tileSize, float minX, float minY){

		double xMax = tileI*Math.Sqrt(3)*tileSize + (tileSize*Math.Sqrt(3)/2);
		double yMax = tileJ*tileSize*1.5 + (tileSize/2);



		for(int i = 0; i < tileList.Count; i++){
			Tile tile = tileList[i];
			double tileX = (double) tile.getPosition().x - minX;// + (tileSize*Math.Sqrt(3)/2);
			double tileY = (double) tile.getPosition().y - minY;// + (tileSize/2);

			if(tileX < 0){
				tileX = 0;
			}
			if(tileY < 0){
				tileY = 0;
			}
			heightMap[tile] = (float)Math.Sin(tileX/xMax*Math.PI)*(float) Math.Sin (tileY/yMax*Math.PI)*100f;
		}

	}

	private bool isTileInSet(Tile tile){
		bool check = false;

		if(tile.getI()>= minI && tile.getI() <= maxI && tile.getJ() >= minJ && tile.getJ() <= maxJ){
			check = true;
		}

		return check;

	}

	public void discourageEdges(){
		for(int i = 0; i < tileList.Count; i++){
			
			bool check = false;
			
			for(int j = 0; j < 6; j++){

				Tile neighbour = tileList[i].getEdge(j).getOtherTile(tileList[i]);

				if( neighbour == null||!isTileInSet(neighbour)){
					check = true;
				}
			}
			
			if(check){
				heightMap[tileList[i]] = 0f;
			}
		}
	}

	public void addNoise(float strength){
		for(int i =0; i < tileList.Count; i++){
			heightMap[tileList[i]] = heightMap[tileList[i]] + UnityEngine.Random.value*strength;
		}
	}

	public void multiplicativeNoise(float level){
		//1 is noisy, 0 is no noise
		for(int i = 0; i < tileList.Count; i++){
			heightMap[tileList[i]] = heightMap[tileList[i]]*(level*UnityEngine.Random.value+(1-level));
		}
	}

	public void difference(){

		Dictionary<Tile, float> roughHeightMap = new Dictionary<Tile, float>();

		for(int i = 0; i < tileList.Count; i++){
			float currentTileHeight = heightMap[tileList[i]];
			float numNeighbours = 1f;

			float difference = 0f;

			for(int j =0 ; j < 6; j++){
				if(tileList[i].getEdge(j).getOtherTile(tileList[i])!=null&&isTileInSet(tileList[i].getEdge(j).getOtherTile(tileList[i]))){
					Tile otherTile = tileList[i].getEdge(j).getOtherTile(tileList[i]);
					difference = Math.Abs(currentTileHeight - heightMap[otherTile]) + difference;
					numNeighbours = numNeighbours + 1;
				}
			}

			difference = difference/numNeighbours;
			roughHeightMap.Add(tileList[i], difference+ currentTileHeight);
		}
		heightMap = roughHeightMap;

	}

	public void smooth(int count){

		for(int k = 0; k < count; k++){
			Dictionary<Tile, float> smoothedHeightMap = new Dictionary<Tile, float>(576);
			for(int i = 0; i < tileList.Count; i++){
				float numNeighbours = 1f;
				float average = heightMap[tileList[i]];
				
				for(int j = 0; j < 6; j++){
					if(tileList[i].getEdge(j).getOtherTile(tileList[i])!=null&&isTileInSet(tileList[i].getEdge(j).getOtherTile(tileList[i]))){
						Tile otherTile = tileList[i].getEdge(j).getOtherTile(tileList[i]);
						average = average + (heightMap[otherTile]*10 + heightMap[tileList[i]])/11f;
						numNeighbours = numNeighbours + 1f;
					}
				}
				
				average = average/numNeighbours;
				
				smoothedHeightMap.Add(tileList[i],average);
				
			}
			heightMap = smoothedHeightMap;	 
		}
	}

	public void buildLand(){

		PriorityQueue queue = new PriorityQueue();

		HashSet<Tile> checkedTiles = new HashSet<Tile>();


		Tile highestTile = tileList[0];
		float highestHeight = heightMap[highestTile];

		//Finds highest tile to begin from
		for(int i = 0; i < tileList.Count; i++){

			if(heightMap[tileList[i]] > highestHeight){
				highestTile = tileList[i];
				highestHeight = heightMap[tileList[i]];
			}

		}

		//Puts highest tile into queue
		queue.push(new KeyValuePair<Tile,float>(highestTile,highestHeight));



		for(int i = 0; i < noLandTiles; i++){

			//Pops the first item in the queue
			Tile tile = queue.popTop().Key;

			//If tile hasn't been visited, marks it as so
			if(!checkedTiles.Contains(tile)){
				checkedTiles.Add (tile);
			}

			//Marks as it as land
			tile.changeBiome(Biome.Grassland);

			//Checks all possible neighbours
			for(int j = 0; j < 6; j++){
				//Checks neighbours aren't null or already visited or in another world tile block
				if(tile.getEdge(j).getOtherTile(tile)==null||checkedTiles.Contains(tile.getEdge(j).getOtherTile(tile))||!isTileInSet(tile.getEdge(j).getOtherTile(tile))){}
				else{

					Tile otherTile = tile.getEdge (j).getOtherTile(tile);

					int count = 0;
					int check = 0;

					Biome checkBiome = Biome.Ocean;

					//Checks neighbour of neighbours and counts number of land tiles adjacent to it
					for(int k = 0; k < 6; k++){

						Tile otherOtherTile = otherTile.getEdge(k).getOtherTile(otherTile);

						if(otherOtherTile != null&&otherOtherTile.getBiome()!=Biome.Ocean&&isTileInSet(otherOtherTile)){
							count = count + 1;
						}
					}
					//If landtiles adjacent to neighbour greater than 1, then add to the queue, except on the first iteration where there is only 1 land tile
					if(count >= 2||i<1){
						queue.push(new KeyValuePair<Tile, float>(tile.getEdge(j).getOtherTile(tile),heightMap[tile.getEdge(j).getOtherTile(tile)]));
						checkedTiles.Add(tile.getEdge(j).getOtherTile(tile));
					}
				}
			}
		}

		//Finds all bodies of water, including surrounding ocean
		List<List<Tile>> bodiesofwater = this.findWaterBodies();
		//Fills all inland seas with swamp for now
		this.fillInLakes(bodiesofwater);

	}

	public void findLandTiles(){


		for(int i = 0; i < tileList.Count; i++){

			if(tileList[i].getBiome()!=Biome.Ocean){
				landTilesList.Add (tileList[i]);
			}

		}

	}

	public void buildHillsandMountains(){

		Dictionary<Tile, float> difference = new Dictionary<Tile, float>(576);

		for(int i =0; i < tileList.Count; i++){

			Tile tile = tileList[i];

			if(tile.getBiome()!= Biome.Ocean){

				float average = 0f;
				float count = 0f;

				for(int j = 0; j < 6; j++){

					Tile neighbour = tile.getEdge (j).getOtherTile(tile); 

					if(neighbour != null&&isTileInSet(neighbour)){
						count = count + 1f;
						float heightDifference = Mathf.Abs(heightMap[tile] - heightMap[neighbour]);
						average = average + heightDifference;
					}
				}

				float averageDifference = average/count;
				difference.Add(tile,averageDifference);
			}
		}

		findLandTiles();

		List<Tile> sortList = new List<Tile>(576);
		
		sortList.Add (landTilesList[0]);
		
		for(int i = 1; i < landTilesList.Count; i++){
			float currentTileHeight = difference[landTilesList[i]];
			
			for(int j = 0; j < sortList.Count; j++){
				
				float listTileHeight = difference[sortList[j]];
				
				if(currentTileHeight > listTileHeight){
					sortList.Insert(j,landTilesList[i]);
					break;
				}
				else if(j + 1 == sortList.Count){
					sortList.Add(landTilesList[i]);
					break;
				}
			}
		}


		for(int i = 0; i < noHillTiles; i++){
			sortList[i].changeBiomeFeature(BiomeFeatures.Hill);
		}
		
		for(int i = 0; i < noMountainTiles; i++){
			sortList[i].changeBiomeFeature(BiomeFeatures.Mountain);
		}


	}


	public bool isMapGood(){

		bool check = true;

		int landTiles = 0;

		for(int i = 0; i < tileList.Count; i++){

			Tile tile = tileList[i];
			int oceanTiles = 0;


			for(int j = 0; j < 6; j++){

				Tile neighbour = tile.getEdge(j).getOtherTile(tile);

				if(neighbour == null && tile.getBiome()!=Biome.Ocean){
					check = false;
				}
				else if (neighbour != null && neighbour.getBiome()== Biome.Ocean && tile.getBiome()!=Biome.Ocean){
					oceanTiles = oceanTiles + 1;
				}
			}

			if(oceanTiles == 6){
				check = false;
			}

			if(tile.getBiome() != Biome.Ocean){
				landTiles = landTiles + 1;
			}

		}

		if(landTiles !=noLandTiles){
			check = false;
		}



		return check;
	}

	public List<Tile> findCoastalTiles(){
		List<Tile> coastTiles = new List<Tile>(576);
		
		for(int i = 0; i < tileList.Count; i++){
			
			Tile tile = tileList[i];
			
			bool isCoast = false;
			
			if(tile.getBiome() != Biome.Ocean){
				for(int j = 0; j < 6; j++){
					
					Tile adjacentTile = tile.getEdge(j).getOtherTile(tile);
					
					if(adjacentTile != null && adjacentTile.getBiome() == Biome.Ocean){
						isCoast = true;
					}
				}

				if(isCoast){
					coastTiles.Add (tile);
				}
			}
		}

		coastalTileList = coastTiles;
		return coastTiles;

	}

	public void fillInLakes(List<List<Tile>> bodiesOfWater){

		List<Tile> biggestBodyOfWater = bodiesOfWater[0];
		int indexOfBiggestBodyOfWater = 0;


		for (int i = 1; i < bodiesOfWater.Count; i++) {

			if(bodiesOfWater[i].Count > biggestBodyOfWater.Count){
				indexOfBiggestBodyOfWater = i;
			}
		}

		bodiesOfWater.RemoveAt(indexOfBiggestBodyOfWater);

		int inlandWaterTiles = 0;

		for (int i = 0; i < bodiesOfWater.Count; i++) {


			List<Tile> bodyOfWater = bodiesOfWater[i]; 

			for (int j = 0; j < bodyOfWater.Count; j++) {

				Tile tile = bodyOfWater[j];

				if(tile.getJ()<12||tile.getJ()>=48){
					tile.changeBiome(Biome.Ice);
				}
				else if(tile.getJ()>=24&&tile.getJ()<36){
					tile.changeBiome(Biome.Desert);
				}
				else{
					tile.changeBiome(Biome.Grassland);
					tile.changeBiomeFeature(BiomeFeatures.Swamp);
				}


				inlandWaterTiles = inlandWaterTiles + 1;
			}
		}
	}

	public List<List<Tile>> findWaterBodies(){

		HashSet<Tile> visited = new HashSet<Tile>();
		List<List<Tile>> bodiesOfWater = new List<List<Tile>>(8);

		for(int i = 0; i < tileList.Count; i ++){

			Tile sourceTile = tileList[i];
			if(sourceTile.getBiome()==Biome.Ocean&&!visited.Contains(sourceTile)){
				
				Queue<Tile> queue = new Queue<Tile>();
				queue.Enqueue(sourceTile);
				
				List<Tile> bodyOfWater = new List<Tile>(576);
				
				while(queue.Count > 0){
					
					Tile tile = queue.Dequeue();
					
					if(!visited.Contains(tile)){
						visited.Add(tile);
						bodyOfWater.Add (tile);
					}

					for(int j =0; j < 6; j++){
						
						Tile nextTile = tile.getEdge(j).getOtherTile(tile);
						
						if(nextTile != null && isTileInSet(nextTile)){
							if(nextTile.getBiome()==Biome.Ocean && !visited.Contains(nextTile)){
								queue.Enqueue(nextTile);
								visited.Add (nextTile);
								bodyOfWater.Add (nextTile);
							}
						}
					}
				}
				
				bodiesOfWater.Add(bodyOfWater);				
			}
		}

		return bodiesOfWater;

	}

	public void buildRiver(){

		for(int i = 0; i < tileList.Count; i++){
			tileList[i].setHeight(heightMap[tileList[i]]);
		}

		for(int i = 0; i < tileList.Count; i++){
			for(int j = 0; j < 6; j++){
				tileList[i].getVertex(j).setHeight();
			}
		}
	

		List<Tile> coastalTiles = findCoastalTiles();

		for (int i = 0; i < noRivers; i++) {

			float rand = UnityEngine.Random.Range(0,coastalTiles.Count);
			int randIndex = Mathf.FloorToInt(rand);
			//Debug.Log(rand + ":" + randIndex);
			Tile tile = coastalTiles[randIndex];

			Vertex startingVertex = null;

			for(int j = 0; j < 6; j++){

				if(tile.getVertex(j).isCoast()){
					startingVertex = tile.getVertex(j);


					break;
				}

			}

			if(startingVertex != null){

				List<Edge> riverEdges = new List<Edge>(576);
				startingVertex.setIsVertexRiver(true);
				Vertex currentVertex = startingVertex;

				for(int j = 0; j < 50; j++){

					List<Edge> potentialEdges = new List<Edge>(3);

					for(int k = 0; k < 3; k++){

						Edge edge = currentVertex.getEdge(k);

						if(edge != null){
							if(edge.isLand()&&!edge.getOtherVertex(currentVertex).isCoast()&&!edge.getOtherVertex(currentVertex).isVertexRiver()){
								potentialEdges.Add(edge);
							}
						}
					}

					if(potentialEdges.Count > 0){

						Edge edge = potentialEdges[0];
						int count = 0;

						for(int k = 0; k < potentialEdges.Count; k++){
							if(currentVertex.getHeight() < potentialEdges[k].getOtherVertex(currentVertex).getHeight()){
								edge = potentialEdges[k];
								count = count + 1;

							}
						}

						if(count > 0){
							riverEdges.Add (edge);
							currentVertex = edge.getOtherVertex(currentVertex);
							currentVertex.setIsVertexRiver(true);

						}
					}
				}

				for(int j = 0; j < riverEdges.Count; j++){
					riverEdges[j].addRiver();
				}
			}
		}
	}

    public void buildForests()
    {

    }

    public void addResources()
    {

    }


}
