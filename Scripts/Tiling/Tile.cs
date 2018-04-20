using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public enum Biome{
	Grassland,
	Ocean,
	Desert,
	Ice,
	None
}

public enum BiomeFeatures{
	Forest,
	Hill,
	Mountain,
	Swamp,
	Clear,
	None
}



public class Tile {

	private float xPosition;
	private float yPosition;

	private Vector3 position;
	private Vector3 phantomPosition;

	private int qCoord;
	private int rCoord;
	private int sCoord;

	private int iCoord;
	private int jCoord;

	private Province province;

	//Ordered clockwise from N, so : N = 0, NE = 1, SE = 2, S = 3, SW = 4, NW = 5.
	private Vertex[] vertexList;

	//Ordered clockwise from NE, so : NE = 0 (Q=+1, R = -1, S = 0), E = 1 (Q=+1, R = 0, S = -1), SE = 2 (Q=0, R = +1, S = -1), 
	//SW = 3(Q=-1, R = +1, S = 0), W = 4 (Q=-1, R = 0, S = +1), NW = 5(Q=0, R = -1, S = +1).
	private Edge[] edgeList;

	private int tileNum;
	private BoardGraphics graphics;
	private Biome biome;
	private BiomeFeatures feature;
	private float height;

	private Dictionary<Resource,float> priceList;
	private Dictionary<Resource,float> stockList;
	
	public Tile(int q, int r, int s, float xPos, float yPos, int i, int j, int tileNo, BoardGraphics nGraphics, bool inWestHalf, float mapX){

		position = new Vector3(xPos,yPos,0);

		if(inWestHalf){
			phantomPosition = new Vector3(xPos + mapX,yPos,0);
		}
		else{
			phantomPosition = new Vector3(xPos - mapX, yPos,0);
		}


		graphics = nGraphics;
		qCoord = q;
		rCoord = r;
		sCoord = s;

		if(q+r+s != 0){
			Debug.Log ("False");
		}

		iCoord = i;
		jCoord = j;

		tileNum = tileNo;

		vertexList = new Vertex[6];
		edgeList = new Edge[6];

		biome = Biome.Ocean;
		feature = BiomeFeatures.None;

		graphics.buildTile(position,phantomPosition, Biome.Ocean, tileNum);

		province = null;

	}

	public void addVertex(int pos, Vertex nVertex){
		vertexList[pos] = nVertex;
	}

	public void addEdge(int pos, Edge nEdge){
		edgeList[pos] = nEdge;
	}

	public Edge getEdge(int pos){
		return edgeList[pos];
	}

	public Vector3 getPosition(){
		return this.position;
	}

	public Vector3 getPhantomPosition(){
		return this.phantomPosition;
	}

	public int getQ(){
		return qCoord;
	}

	public int getR(){
		return rCoord;
	}

	public int getS(){
		return sCoord;
	}

	public int getI(){
		return iCoord;
	}

	public int getJ(){
		return jCoord;
	}

	public Vertex getVertex(int pos){
		return vertexList[pos];
	}

	public void changeBiome(Biome nBiome){

		if(nBiome == Biome.None){

		}
		else{
			this.biome = nBiome;
			graphics.buildTile(position,phantomPosition, nBiome, tileNum);

			for(int i = 0; i < 6; i++){

				edgeList[i].changeTransistionType();

			}


		}


	}

	public void changeBiomeFeature(BiomeFeatures nFeature){

		if(nFeature == BiomeFeatures.None||biome == Biome.Ocean){

		}
		else{
			this.feature = nFeature;
			graphics.buildFeatures(position,phantomPosition, nFeature, tileNum);
		}

	}

	public Biome getBiome(){
		return this.biome;
	}

	public Vector3 returnNearestEdgePositiontoPixel(float x, float y){

		Edge nearestEdge = edgeList[0];

		float lastEdgeDist = nearestEdge.getAbsoluteDistancefrompixel(x,y);


		for(int i =0; i < edgeList.Length; i++){

			if(edgeList[i].getAbsoluteDistancefrompixel(x,y) < lastEdgeDist){
				nearestEdge = edgeList[i];
				lastEdgeDist = edgeList[i].getAbsoluteDistancefrompixel(x,y);
			}

		}
		return nearestEdge.getPosition();
	}

	public Edge returnNearestEdgetoPixel(float x, float y){
		
		Edge nearestEdge = edgeList[0];
		
		float lastEdgeDist = nearestEdge.getAbsoluteDistancefrompixel(x,y);
		
		
		for(int i =0; i < edgeList.Length; i++){
			
			if(edgeList[i].getAbsoluteDistancefrompixel(x,y) < lastEdgeDist){
				nearestEdge = edgeList[i];
				lastEdgeDist = edgeList[i].getAbsoluteDistancefrompixel(x,y);
			}
			
		}
		return nearestEdge;
	}

	public void addPriceList(Dictionary<Resource,float> nPriceList){
		this.priceList = nPriceList;
	}

	public void addStockList(Dictionary<Resource,float> nStockList){
		this.stockList = nStockList;
	}

	public float getPrice(Resource res){
		return this.priceList[res];
	}

	public float getStockSize(Resource res){
		return this.stockList[res];
	}

	public void setHeight(float nHeight){
		height = nHeight;
	}

	public float getHeight(){
		return height;
	}

	public int getDistanceToTile(Tile otherTile){
		return Math.Max (Math.Max(Math.Abs(this.qCoord-otherTile.qCoord),Math.Abs(this.rCoord-otherTile.rCoord)),Math.Abs(this.sCoord-otherTile.sCoord));
	}

	public int[] getRelativeQRSCoords(Tile otherTile){ 
		int[] QRS = new int[3];

		QRS[0] = this.qCoord - otherTile.qCoord;
		QRS[1] = this.rCoord - otherTile.rCoord;
		QRS[2] = this.sCoord - otherTile.sCoord;

		//Debug.Log (QRS[0] + "," + QRS[1] + "," + QRS[2]);

		return QRS;


	}

	public void setOverlay(float r, float g, float b){
		graphics.buildOverlay(position,phantomPosition,tileNum, r,g,b);
	}

	public Province getProvince(){
		return this.province;
	}

	public void setProvince(Province nProvince){
		province = nProvince;
	}

	public void swapEdges(int pos, Edge edgeToAdd){
		edgeList[pos] = edgeToAdd;
	}

    public BiomeFeatures getBiomeFeature()
    {
        return feature;
    }

	public void addCity(City c){
        graphics.buildCity(position, phantomPosition, tileNum);
	}
}
