using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProvinceGenerator{

	private HashSet<Tile> landTiles;
	private HashSet<Tile> inlandTiles;
	private HashSet<Tile> coastalTiles;
	private int numProvinces;
	private int numTiles;
	private List<ProvinceCentre> centres;

	private Dictionary<Tile,int> distanceToOcean;


	public ProvinceGenerator(List<Tile> nLandTiles,int numberProvinces){
		landTiles = new HashSet<Tile>(nLandTiles);
		searchForInlandAndCoastalTiles(); //intialises coastalTiles and inlandTiles
		numProvinces = numberProvinces;
		numTiles = landTiles.Count;
		centres = new List<ProvinceCentre>();

	}

	/*public List<Province> generate(){
		pickIntialTiles();
		List<Province> provinceList = drawProvinces();

		for(int i = 0; i < provinceList.Count; i++){
			provinceList[i].overlayColour();
			provinceList[i].findBorderingProvinces();
			//Debug.Log ("Province Size: " + provinceList[i].getTiles().Count);
		}
		iterate(0);
		//setAllTileDistances();

		Debug.Log("Passed");

		return new List<Province>(provinceList);



	}*/

	public HashSet<Province> generateNew(){
		
		int count = 0;
		int provinceNum = numProvinces;

		while(provinceNum != 1){
			provinceNum = provinceNum/2;
			count = count + 1;


		}
		HashSet<HashSet<Tile>> tilesList = createSplit(landTiles, coastalTiles);

		for(int j = 0; j < count - 1; j++){
			HashSet<HashSet<Tile>> tempTilesList = new HashSet<HashSet<Tile>>();

			foreach(HashSet<Tile> tiles in tilesList){
				HashSet<Tile> tilesToSortFrom = findEdgeTilesInSet(tiles);
				HashSet<HashSet<Tile>> tempTempList = createSplit(tiles,tilesToSortFrom);
				foreach(HashSet<Tile> temp in tempTempList){
					tempTilesList.Add(temp);
				}
			}

			tilesList = new HashSet<HashSet<Tile>>(tempTilesList);
		}

		HashSet<Province> provinceSet = new HashSet<Province>();

		foreach(HashSet<Tile> tileSet in tilesList){
			Color color = new Color(UnityEngine.Random.value,UnityEngine.Random.value,UnityEngine.Random.value);
			Province province = new Province(color,tileSet);
			province.overlayColour();
			provinceSet.Add(province);
		}

		foreach(Province province in provinceSet){
			province.findBorderingProvinces();
		}

        foreach(Province province in provinceSet)
        {
            placeCities(province);
        }



		return provinceSet;
	}

	public HashSet<HashSet<Tile>> createSplit(HashSet<Tile> tilesToSplit, HashSet<Tile> tilesToSortFrom){

		Graphs.Graph<Tile> graph = new Graphs.Graph<Tile>();

		Dictionary<int,Tile> tileDic = new Dictionary<int, Tile>();


		//create nodes
		foreach(Tile tile in tilesToSplit){
			int node = graph.addIntNode(tile);
			tileDic.Add(node,tile);

		}

		//create Edges
		HashSet<Tile> visited = new HashSet<Tile>();

		foreach(Tile tile in tilesToSplit){
			for(int j = 0; j < 6; j++){
				Tile neighbour = tile.getEdge(j).getOtherTile(tile);

				if(tileDic.ContainsValue(neighbour)&&!visited.Contains(neighbour)){
					graph.addIntEdge(tile,neighbour,1);
				}
			}

			visited.Add(tile);
		}

		bool minCut = true;

		if(tilesToSplit.Count > 650){
			minCut = false;
		}

		HashSet<HashSet<int>> nodes = graph.findKCut(tilesToSplit.Count/2,tilesToSortFrom, minCut);

		HashSet<HashSet<Tile>> tilesList = new HashSet<HashSet<Tile>>();

		foreach(HashSet<int> node in nodes){

			HashSet<Tile> tiles = new HashSet<Tile>();

			Color color = new Color(UnityEngine.Random.value,UnityEngine.Random.value,UnityEngine.Random.value);
			foreach(int tileNo in node){
				Tile tile = tileDic[tileNo];
				tiles.Add(tile);
				tile.setOverlay(color.r,color.g,color.b);
			}

			tilesList.Add(tiles);
		}

		return tilesList;

	}

	private HashSet<Tile> findEdgeTilesInSet(HashSet<Tile> tilesToSort){

		HashSet<Tile> edgeTiles = new HashSet<Tile>();

		foreach(Tile tile in tilesToSort){
			int count = 0;
			bool check = false;
			for(int i = 0; i < 6; i++){

				if(tilesToSort.Contains(tile.getEdge(i).getOtherTile(tile))){
					count = count + 1;
					if(edgeTiles.Contains(tile.getEdge(i).getOtherTile(tile))){
						check = true;
					}
				}

			}
			if(count < 6&&!check){
				edgeTiles.Add(tile);
			}
		}


		return edgeTiles;
	}




	/*private void findDistancesToOcean(){

		distanceToOcean = new Dictionary<Tile, int>();
		List<Tile> sortedTiles = new List<Tile>(coastalTiles);
		List<Tile> unSortedTiles = new List<Tile>(inlandTiles);


		for(int i = 0; i < landTiles.Count; i++){
			distanceToOcean.Add(landTiles[i],1000);
			if(sortedTiles.Contains(landTiles[i])){
				distanceToOcean[landTiles[i]] = 1;
			}
		}

		bool check = true;

		while(check){

			List<Tile> newSortedList = new List<Tile>(sortedTiles);


			for(int i = 0; i < sortedTiles.Count; i++){
				Tile tile = sortedTiles[i];
				for(int j = 0; j < 6; j++){
					Tile neighbour = tile.getEdge(j).getOtherTile(tile);

					if(!sortedTiles.Contains(neighbour)&&distanceToOcean.ContainsKey(neighbour)&&distanceToOcean[neighbour]>distanceToOcean[tile]+1){
						distanceToOcean[neighbour] = distanceToOcean[tile] + 1;
						if(!newSortedList.Contains(neighbour)){
							newSortedList.Add(neighbour);
							unSortedTiles.Remove(neighbour);
						}
					}
				}
			}

			sortedTiles = newSortedList;
			if(unSortedTiles.Count == 0){
				check = false;
			}

		}




	}*/

	private void searchForInlandAndCoastalTiles(){
		inlandTiles = new HashSet<Tile>();
		coastalTiles = new HashSet<Tile>();

		foreach(Tile tile in landTiles){

			bool check1 = true;

			for(int j = 0; j < 6; j++){
				Tile neighbour = tile.getEdge(j).getOtherTile(tile);

				if(neighbour== null || neighbour.getBiome()==Biome.Ocean){
					check1 = false;
				}


			}

			if(check1){
				inlandTiles.Add (tile);
			}
			else{
				coastalTiles.Add(tile);
			}

		}

		//findDistancesToOcean();
	}

    public void placeCities(Province province)
    {
        List<Tile> tileList = province.getTiles();
        Tile maxTile = tileList[0];
        int maxScore = 0;

        for(int i = 0; i < tileList.Count; i++)
        {
            Tile tile = tileList[i];

            bool isTileCoast = false;
            bool isTileRiverside = false;
            bool isTileBordered = false;
            bool isTileFlat = false;

            if(tile.getBiomeFeature() == BiomeFeatures.Clear || tile.getBiomeFeature() == BiomeFeatures.None)
            {
                isTileFlat = true;
            }

            for(int j = 0; j < 6; j++)
            {
                Edge edge = tile.getEdge(j);
                if (edge.getBiomeTransistion() == BiomeTransistion.River)
                {
                    
                    isTileRiverside = true;
                }
                Tile nextTile = edge.getOtherTile(tile);
                if(nextTile.getBiome() == Biome.Ocean)
                {
                    isTileCoast = true;
                }

                if(nextTile.getProvince() != null && !nextTile.getProvince().Equals(tile.getProvince()))
                {
                    isTileBordered = true;
                }

            }

            int score = 1000;

            if (isTileBordered)
            {
                score = score / 100;
            }

            if (isTileCoast)
            {
                score = score * 2;
            }

            if (isTileRiverside)
            {
                score = score * 2;
                //Debug.Log("River" + score);
            }

            if (isTileFlat)
            {
                score = score * 2;
            }

            if(score > maxScore)
            {
                maxTile = tile;
                maxScore = score;
            }

        }

        province.addCity(maxTile);
        Debug.Log(maxScore);


    }


	/*
	public Tile pickFirstTile(){

		Tile pickedTile = inlandTiles[0];
		int pickedNum =  Mathf.Abs(pickedTile.getQ()) + Mathf.Abs(pickedTile.getR()) + Mathf.Abs(pickedTile.getS()); 

		for(int i = 0; i < inlandTiles.Count; i++){

			Tile candidateTile = inlandTiles[i];
			int candidateNum = Mathf.Abs (candidateTile.getQ())+Mathf.Abs (candidateTile.getR ())+Mathf.Abs(candidateTile.getS ());

			if(candidateNum>pickedNum){
				pickedTile = candidateTile;
				pickedNum = candidateNum;
			}
		}

		return pickedTile;
	}

	private void pickIntialTiles(){

		Tile firstTile = pickFirstTile();
		centres.Add(new ProvinceCentre(firstTile));
		List<Tile> centreTiles = new List<Tile>();
		centreTiles.Add(firstTile);


		Dictionary<Tile,List<int>> distancesToCentres = new Dictionary<Tile, List<int>>();

		for(int i = 0; i < inlandTiles.Count; i++){
			List<int> list = new List<int>();
			distancesToCentres.Add(inlandTiles[i],list);
		}

		for(int i = 0; i < numProvinces - 1; i++){
			
			for(int j = 0; j < inlandTiles.Count; j++){
				
				int distance = searchTilesDistances(inlandTiles[j],centres[i].getTile());
				List<int> list = distancesToCentres[inlandTiles[j]];
				list.Add(distance);
				list.Sort();
			}

			Tile maxTile = inlandTiles[0];

			for(int j = 0; j < inlandTiles.Count; j++){

				for(int k = 0; k < distancesToCentres[inlandTiles[j]].Count; k++){

					if(distancesToCentres[inlandTiles[j]][k] > distancesToCentres[maxTile][k]){
						maxTile = inlandTiles[j];
						break;
					}
					else if(distancesToCentres[inlandTiles[j]][k] < distancesToCentres[maxTile][k]){
						break;
					}
				}
			}
			centres.Add (new ProvinceCentre(maxTile));
		}
	}

	private void iterate(int count){

		for(int i = 0; i < centres.Count; i++){
			ProvinceCentre centre = centres[i];
			centre.getTile().setOverlay(1,0,0);
			if(i==0){
				centre.getTile().setOverlay(0,1,0);
			}
			if(i == centres.Count - 1){
				centre.getTile().setOverlay(0,0,1);
			}
		}

	}

	private List<Province> drawProvinces(){

		HashSet<Tile> unvisitedTiles = new HashSet<Tile>(landTiles);

		List<Queue<Tile>> queueList = new List<Queue<Tile>>();
		List<Province> provinceList = new List<Province>();

		for(int i = 0; i < numProvinces; i++ ){
			Queue<Tile> queue = new Queue<Tile>();
			queue.Enqueue(centres[i].getTile());
			unvisitedTiles.Remove(centres[i].getTile());
			queueList.Add (queue);

			Province province = new Province(new Color(Random.Range (0f,1f),Random.Range (0f,1f),Random.Range (0f,1f)));
			provinceList.Add (province);
			province.addTile(centres[i].getTile());

		}

		while(unvisitedTiles.Count > 0){

			queueList.Reverse();
			provinceList.Reverse();

			for(int j = 0; j < queueList.Count; j++){

				Queue<Tile> queue = queueList[j];
				int queueCount = queue.Count;

				for(int k = 0; k < queueCount; k++){
					//Debug.Log (queueCount);
					if(queue.Count>0){

						Tile tile = queue.Dequeue();
						
						for(int l = 0; l < 6; l++){
							
							Tile neighbour = tile.getEdge(l).getOtherTile(tile);
							
							if(unvisitedTiles.Contains(neighbour)){
								queue.Enqueue(neighbour);
								unvisitedTiles.Remove(neighbour);
								Province province = provinceList[j];
								province.addTile(neighbour);
							}	
						}
					}
				}
			}
		}
		return provinceList;
	}

	private int searchTilesDistances(Tile startTile, Tile endTile){

		Dictionary<Tile,int> distanceList = new Dictionary<Tile, int>();

		Queue<Tile> queue = new Queue<Tile>();
		queue.Enqueue(startTile);

		Tile tile = startTile;
		int count = 0;

		distanceList.Add(startTile,count);

		while(queue.Count > 0){
			tile = queue.Dequeue();
			count = distanceList[tile];

			if(tile.Equals(endTile)){
				break;
			}

			for(int i = 0; i < 6; i++){

				Tile neighbour = tile.getEdge(i).getOtherTile(tile);
				if(neighbour!=null && neighbour.getBiome()!=Biome.Ocean){

					if(!distanceList.ContainsKey(neighbour)){
						distanceList.Add(neighbour,count+1);
						queue.Enqueue(neighbour);

						//Delete if breaking, just a shortcut
						if(neighbour.Equals(endTile)){
							return count+1;
						}
					}
				}
				/*else if(neighbour!=null && neighbour.getBiome()==Biome.Ocean){
					return count + 100;
				}
			}
		}
		return count;
	}*/
}
