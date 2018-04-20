using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class TileManager {

	private int tileI;
	private int tileJ;

	private float tileSize;
	private float tileWidth;
	private float tileHeight;

	private float mapX;
	private float mapY;

	// Access a Tile using R value for the first Dictionary, then the Q value for the inner Dictionary to retrieved tile
	private Dictionary<int, Dictionary<int,Tile>> tileDictQRS; 
	
	// Access a Tile using j value for the first Dictionary, then the i value for the inner Dictionary to retrieved tile
	private Dictionary<int, Dictionary<int,Tile>> tileDictIJ;


	//DO NOT SORT!
	private List<Tile> tileList;
	private List<Vertex> vertexList;
	private List<Edge> edgeList;



	public TileManager(int ntileI, int ntileJ, float ntileSize, BoardGraphics graphics, TempLoader temp){

		tileI = ntileI;
		tileJ = ntileJ;
		tileSize = ntileSize;

		graphics.setup(tileI*tileJ);

		tileHeight = tileSize * 2;
		tileWidth = Mathf.Sqrt(3)/2 * tileHeight;

		mapX = tileWidth*(float)tileI;

		tileList = new List<Tile>();
		vertexList = new List<Vertex>();
		edgeList = new List<Edge>();

		tileDictQRS = new Dictionary<int, Dictionary<int, Tile>>();
		tileDictIJ = new Dictionary<int, Dictionary<int, Tile>>();

		Dictionary<int,Tile> rowListQRS; //Q=X, R=Z, S=Y in the old cube coordinates
		Dictionary<int,Tile> rowListIJ;

		int tileCount = 0;

		for(int j = 0; j < tileJ; j++){

			rowListIJ = new Dictionary<int, Tile>();
			rowListQRS = new Dictionary<int, Tile>();

			tileDictQRS.Add(-j,rowListQRS);
			tileDictIJ.Add(j,rowListIJ);

			int offset = Mathf.CeilToInt((float)j/2);

			for(int i = 0; i<tileI; i++){

				float x_Offset;
				
				if( j%2 == 1){
					x_Offset = tileWidth;
				}
				else{
					x_Offset = tileWidth / 2;
				}
				
				int q = i+offset;
				int s =  -(i+offset) + j;
				int r = -j;

				bool inWestHalf;

				if(i<=tileI/2){
					inWestHalf = true;
				}
				else{
					inWestHalf = false;
				}

				Tile tile = new Tile(q,r,s,tileWidth*i + x_Offset ,(tileSize) + (tileHeight*0.75f*j),i,j,tileCount,graphics,inWestHalf,mapX);

				//Debug.Log (i+":"+j);

				tile.addStockList(temp.setUpStockList());
				tile.addPriceList(temp.setUpStockList());

				rowListIJ.Add(i, tile);
				rowListQRS.Add(i+offset, tile);

				tileList.Add(tile);
				addVerticesToTile(tile, graphics);
				addEdgesToTile(tile, graphics);

				tileCount = tileCount + 1;
			}
		}

		this.addEdgesToVertices();
		graphics.setUpEdgeNum(edgeList.Count);
		graphics.setUpVertexNum(vertexList.Count);
		graphics.buildCollider(tileList[tileList.Count - 1]);

		this.wrapAroundMap();

		}

	private void addVerticesToTile(Tile tile, BoardGraphics graphics){

		float xPos = tile.getPosition().x;
		float yPos = tile.getPosition().y;

		int q = tile.getQ();
		int r = tile.getR();
		int s = tile.getS();

		bool c = false;

		if(tile.getI() < tileI/2){
			c = true;
		}


		//Vertex 0
		if(containsTile(q,r-1,s+1)){tile.addVertex(0,getTileQRS(q,r-1,s+1).getVertex(2));}
		else if(containsTile(q+1,r-1,s)){tile.addVertex(0,getTileQRS(q+1,r-1,s).getVertex(4));}
		else{Vertex vert = new Vertex(new Vector3(xPos,yPos + tileSize,0),VertOrientation.Up,graphics,vertexList.Count,c,mapX); tile.addVertex(0,vert); vertexList.Add(vert);}

		//Vertex 1
		if(containsTile(q+1,r-1,s)){tile.addVertex(1,getTileQRS(q+1,r-1,s).getVertex(3));}
		else if(containsTile(q+1,r,s-1)){tile.addVertex(1,getTileQRS(q+1,r,s-1).getVertex(5));}
		else{Vertex vert = new Vertex(new Vector3(xPos + (tileWidth/2),yPos+(tileSize/2),0),VertOrientation.Down,graphics,vertexList.Count,c,mapX);tile.addVertex(1,vert); vertexList.Add(vert);}

		//Vertex 2
		if(containsTile(q+1,r,s-1)){tile.addVertex(2,getTileQRS(q+1,r,s-1).getVertex(4));}
		else if(containsTile(q,r+1,s-1)){tile.addVertex(2,getTileQRS(q,r+1,s-1).getVertex(0));}
		else{Vertex vert = new Vertex(new Vector3(xPos + (tileWidth/2), yPos - (tileSize/2),0),VertOrientation.Up,graphics,vertexList.Count,c,mapX);tile.addVertex(2,vert); vertexList.Add(vert);}

		//Vertex 3
		if(containsTile(q,r+1,s-1)){tile.addVertex(3,getTileQRS(q,r+1,s-1).getVertex(5));}
		else if(containsTile(q-1,r+1,s)){tile.addVertex(3,getTileQRS(q-1,r+1,s).getVertex(1));}
		else{Vertex vert = new Vertex(new Vector3(xPos,yPos - tileSize,0),VertOrientation.Down,graphics,vertexList.Count,c,mapX);tile.addVertex(3,vert); vertexList.Add(vert);}

		//Vertex 4
		if(containsTile(q-1,r+1,s)){tile.addVertex(4,getTileQRS(q-1,r+1,s).getVertex(0));}
		else if(containsTile(q-1,r,s+1)){tile.addVertex(4,getTileQRS(q-1,r,s+1).getVertex(2));}
		else{Vertex vert = new Vertex(new Vector3(xPos - (tileWidth/2),yPos - (tileSize/2),0),VertOrientation.Up,graphics,vertexList.Count,c,mapX);tile.addVertex(4,vert); vertexList.Add(vert);}

		//Vertex 5
		if(containsTile(q-1,r,s+1)){tile.addVertex(5,getTileQRS(q-1,r,s+1).getVertex(1));}
		else if(containsTile(q,r-1,s+1)){tile.addVertex(5,getTileQRS(q,r-1,s+1).getVertex(3));}
		else{Vertex vert = new Vertex(new Vector3(xPos - (tileWidth/2),yPos+(tileSize/2),0),VertOrientation.Down,graphics,vertexList.Count,c,mapX);tile.addVertex(5,vert); vertexList.Add(vert);}

	}

	private void addEdgesToTile(Tile tile, BoardGraphics graphics){

		int q = tile.getQ();
		int r = tile.getR();
		int s = tile.getS();

		bool c = false;

		if(tile.getI() < tileI/2){
			c = true;
		}

		//Edge 0
		if(containsTile(q+1,r-1,s)){tile.addEdge(0,getTileQRS(q+1,r-1,s).getEdge(3)); getTileQRS(q+1,r-1,s).getEdge(3).addTile(tile);}
		else{Edge edge = new Edge(tile.getVertex(0),tile.getVertex(1),EdgeOrientation.NWSE,edgeList.Count,graphics,c,mapX); tile.addEdge(0,edge); edgeList.Add(edge); edge.addTile(tile);}

		//Edge 1
		if(containsTile(q+1,r,s-1)){tile.addEdge(1,getTileQRS(q+1,r,s-1).getEdge(4)); getTileQRS(q+1,r,s-1).getEdge(4).addTile(tile);}
		else{Edge edge = new Edge(tile.getVertex(1),tile.getVertex(2),EdgeOrientation.NS,edgeList.Count,graphics,c,mapX); tile.addEdge(1,edge); edgeList.Add(edge); edge.addTile(tile);}

		//Edge 2
		if(containsTile(q,r+1,s-1)){tile.addEdge(2,getTileQRS(q,r+1,s-1).getEdge(5)); getTileQRS(q,r+1,s-1).getEdge(5).addTile(tile);}
		else{Edge edge = new Edge(tile.getVertex(2),tile.getVertex(3),EdgeOrientation.NESW,edgeList.Count,graphics,c,mapX); tile.addEdge(2,edge); edgeList.Add(edge); edge.addTile(tile);}

		//Edge 3
		if(containsTile(q-1,r+1,s)){tile.addEdge(3,getTileQRS(q-1,r+1,s).getEdge(0)); getTileQRS(q-1,r+1,s).getEdge(0).addTile(tile);}
		else{Edge edge = new Edge(tile.getVertex(4),tile.getVertex(3),EdgeOrientation.NWSE,edgeList.Count,graphics,c,mapX); tile.addEdge(3,edge); edgeList.Add(edge); edge.addTile(tile);}

		//Edge 4
		if(containsTile(q-1,r,s+1)){tile.addEdge(4,getTileQRS(q-1,r,s+1).getEdge(1)); getTileQRS(q-1,r,s+1).getEdge(1).addTile(tile);}
		else{Edge edge = new Edge(tile.getVertex(5),tile.getVertex(4),EdgeOrientation.NS,edgeList.Count,graphics,c,mapX); tile.addEdge(4,edge); edgeList.Add(edge); edge.addTile(tile);}

		//Edge 5
		if(containsTile(q,r-1,s+1)){tile.addEdge(5,getTileQRS(q,r-1,s+1).getEdge(2)); getTileQRS(q,r-1,s+1).getEdge(2).addTile(tile);}
		else{Edge edge = new Edge(tile.getVertex(0),tile.getVertex(5),EdgeOrientation.NESW,edgeList.Count,graphics,c,mapX); tile.addEdge(5,edge); edgeList.Add(edge); edge.addTile(tile);}
	}

	private void addEdgesToVertices(){
		for(int i = 0; i < edgeList.Count; i++){

			Edge edge = edgeList[i];
			if(edge.getEdgeOrientation() == EdgeOrientation.NS){
				edge.getNorthVert().addEdge(0,edge);
				edge.getSouthVert().addEdge(0,edge);
			}
			else if(edge.getEdgeOrientation() == EdgeOrientation.NESW){
				edge.getNorthVert().addEdge(2,edge); //2
				edge.getSouthVert().addEdge(1,edge); //1
			}
			else if(edge.getEdgeOrientation()==EdgeOrientation.NWSE){
				edge.getNorthVert().addEdge(1,edge); //1
				edge.getSouthVert().addEdge(2,edge); //2
			}
		}
	}



	public bool containsTile(int q, int r, int s){
		bool check = false;

		if(tileDictQRS.ContainsKey(r)){
			if(tileDictQRS[r].ContainsKey(q)){
				check = true;
			}
		}
		return check;
	}

	public bool containsTile(int i, int j){
		bool check = false;

		if(tileDictIJ.ContainsKey(j)){
			if(tileDictIJ[j].ContainsKey(i)){
				check = true;
			}
		}
		return check;
	}

	public int getTileI(){
		return this.tileI;
	}

	public int getTileJ(){
		return this.tileJ;
	}

	public float getTileSize(){
		return this.tileSize;
	}

	public Tile getTileIJ(int i, int j){
		return tileDictIJ[j][i];
	}

	public Tile getTileQRS(int q, int r, int s){

		if(tileDictQRS.ContainsKey(r)){
			if(tileDictQRS[r].ContainsKey(q)){
				return tileDictQRS[r][q];
			}
			else{
				return null;
			}
		}
		else{
			return null;
		}


	}

	public List<Tile> getTileList(){
		List<Tile> listToReturn = new List<Tile>(tileList);
		return listToReturn;
	}

	public List<Edge> getEdgeList(){
		List<Edge> listToReturn = new List<Edge>(edgeList);
		return listToReturn;
	}

	public List<Vertex> getVertexList(){
		List<Vertex> listToReturn = new List<Vertex>(vertexList);
		return listToReturn;
	}

	public void changeTileBiome(float x, float y, Biome nBiome){

		Tile tile = this.pixelToHex(x,y);
		if(tile != null){
			tile.changeBiome(nBiome);
		}
	}

	public void changeTileFeature(float x, float y, BiomeFeatures feature){
		Tile tile = this.pixelToHex(x,y);
		if(tile != null){
			tile.changeBiomeFeature(feature);
		}
	}

	public void changeEdgeType(float x, float y, BiomeTransistion bT){
		Edge edge = this.getEdgefromPixel(x,y);

		if(edge != null&&bT==BiomeTransistion.River){
			edge.addRiver();
		}
		else if(edge!=null&&bT==BiomeTransistion.None){
			edge.removeRiver();
		}

	}

	private Edge getEdgefromPixel(float x, float y){
		Tile tile = this.pixelToHex(x,y);
		if(tile == null){
			return null;
		}
		else{
			return tile.returnNearestEdgetoPixel(x,y);
		}
	}


	public Vector3 getEdgePositionfromPixel(float x, float y){
		Tile tile = this.pixelToHex(x,y);

		if(tile == null){
			return new Vector3(0,0,0);
		}
		else{
			return tile.returnNearestEdgePositiontoPixel(x,y);
		}
	}


	public Vector3 getTilePositionfromPixel(float x, float y){

		Vector3 tile = this.pixelToTilePosition(x,y);
		if(tile == null){
			return new Vector3(0,0,0);
		}
		else{
			return tile;
		}
	}

	private Tile pixelToHex(float x, float y){
		
		float q = (((x-(tileWidth/2))*Mathf.Sqrt(3)/3)+((y-tileSize)/3))/tileSize;
		float r = -(y-tileSize)*2/3/tileSize;
		float s = 0f-q-r;
		
		//Debug.Log ("Q: " + q + ", R: " + r + ", S: " + s);

		KeyValuePair<Tile,Vector3> pos = roundToNearestHex(q,r,s);
		return pos.Key;
	}

	private Vector3 pixelToTilePosition(float x, float y){
		float q = (((x-(tileWidth/2))*Mathf.Sqrt(3)/3)+((y-tileSize)/3))/tileSize;
		float r = -(y-tileSize)*2/3/tileSize;
		float s = 0f-q-r;

		//Debug.Log ("Q: " + q + ", R: " + r + ", S: " + s);

		KeyValuePair<Tile,Vector3> pos = roundToNearestHex(q,r,s);
		return pos.Value;
	}

	private KeyValuePair<Tile,Vector3> roundToNearestHex(float q, float r, float s){
		
		float rq = Mathf.Round(q);
		float rr = Mathf.Round(r);
		float rs = Mathf.Round(s);
		
		float qD = Mathf.Abs(rq - q);
		float rD = Mathf.Abs(rr - r);
		float sD = Mathf.Abs(rs - s);
		
		if(qD>rD&&qD>sD){
			rq = -rr-rs;
		}
		else if(rD>qD&&rD>sD){
			rr = -rq-rs;
		}
		else{
			rs = -rq-rr;
		}

		int j = -Mathf.RoundToInt(rr);
		int offset = Mathf.CeilToInt((float) j/2);
		int i = Mathf.RoundToInt(rq)-offset;

		bool isOffMainMap = false;

		if(i<0||i>=tileI){
			isOffMainMap = true;
		}

		i = i%tileI;
		j= j%tileJ;

		if(i < 0){
			i = tileI+i;
		}

		//Debug.Log("i: " + i + " j: " + j);
		if(containsTile(i,j)){
			if(isOffMainMap){
				return new KeyValuePair<Tile,Vector3>(this.getTileIJ(i,j),this.getTileIJ(i,j).getPhantomPosition());
			}
			else{
				return new KeyValuePair<Tile,Vector3>(this.getTileIJ(i,j),this.getTileIJ(i,j).getPosition());
			}

		}
		else{
			return new KeyValuePair<Tile, Vector3>(null,new Vector3(0,0,0));
		}

	}

	public List<Tile> pathToTile(Tile fromTile, Tile toTile){
		return null;
	}

	private void wrapAroundMap(){

		for(int j = 0; j < tileJ; j++){

			Tile tile = this.getTileIJ(tileI-1,j);

			for(int e = 0; e < 3; e++){
				Edge edge = tile.getEdge(e);

				Vertex v1 = edge.getNorthVert();
				Vertex v2 = edge.getSouthVert();

				

				if(edge.getOtherTile(tile)==null){

					if(e==0&&this.containsTile(0,j+1)&&edge.getTiles().Count<2){
						Edge newEdge = this.getTileIJ(0,j+1).getEdge(3);
						newEdge.addTile(tile);
						tile.swapEdges(0,newEdge);
						
						tile.addVertex(0,newEdge.getNorthVert());
						tile.addVertex(1,newEdge.getSouthVert());

						newEdge.getNorthVert().swapInEdges(v1);
						newEdge.getSouthVert().swapInEdges(v2);
					}
					else if(e==1&&this.containsTile(0,j)){
						Edge newEdge = this.getTileIJ(0,j).getEdge(4);
						newEdge.addTile(tile);
						tile.swapEdges(1,newEdge);
						tile.addVertex(1,newEdge.getNorthVert());
						tile.addVertex(2,newEdge.getSouthVert());

						newEdge.getNorthVert().swapInEdges(v1);
						newEdge.getSouthVert().swapInEdges(v2);
					}
					else if(e==2&&this.containsTile(0,j-1)&&edge.getTiles().Count<2){
						Edge newEdge = this.getTileIJ(0,j-1).getEdge(5);
						newEdge.addTile(tile);
						tile.swapEdges(2,newEdge);
						tile.addVertex(2,newEdge.getNorthVert());
						tile.addVertex(3,newEdge.getSouthVert());

						newEdge.getNorthVert().swapInEdges(v1);
						newEdge.getSouthVert().swapInEdges(v2);
					}
					else{
						//Debug.Log("Error on tile: "+tile.getI()+":"+tile.getJ());
					}
				}
			}


		}
	}


}
