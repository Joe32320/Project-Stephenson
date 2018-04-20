using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;


public enum BiomeTransistion{
	Coast,
	River,
	None
}

public enum EdgeOrientation{
	NWSE,
	NS,
	NESW
}

public class Edge{

	//If tileList has 2 entires, first entry is the westernmost Tile
	private List<Tile> tileList;

	private Vertex northVertex;
	private Vertex southVertex;
	private	EdgeOrientation orientation;
	private Vector3 centre;
	private Vector3 secondCentre;

	private int edgeNumber;

	private BoardGraphics graphics;

	private BiomeTransistion biomeTransisition;
	
	public Edge(Vertex nNorthVertex, Vertex nSouthVertex, EdgeOrientation nOrientation, int num, BoardGraphics nGraphics, bool isInWestHalf, float mapX){

		graphics = nGraphics;
		tileList = new List<Tile>();

		northVertex = nNorthVertex;
		southVertex = nSouthVertex;

		float northX = northVertex.getPosition().x;
		float northY = northVertex.getPosition().y;

		float southX = southVertex.getPosition().x;
		float southY = southVertex.getPosition().y;

		orientation = nOrientation;
		centre = new Vector3((northX/2f)+(southX/2f),(northY/2f)+(southY/2f),0f);
		if(isInWestHalf){
			secondCentre = new Vector3(centre.x + mapX, centre.y, 0);
		}
		else{
			secondCentre = new Vector3(centre.x - mapX, centre.y, 0);
		}

		edgeNumber = num;

		biomeTransisition = BiomeTransistion.None;

	}

	public Vertex getNorthVert(){
		return northVertex;
	}

	public Vertex getSouthVert(){
		return southVertex;
	}

	public EdgeOrientation getEdgeOrientation(){
		return orientation;
	}
	
	public int getEdgeNum(){
		return edgeNumber;
	}

	public Vector3 getPosition(){
		return this.centre;
	}

	public void swapTile(Tile tileToDelete, Tile tileToAdd){
		
	}

	public void addTile(Tile tile){

		if(tileList.Count == 0){
			tileList.Add(tile);
		}
		else{
			Tile otherTile = tileList[0];

			if(otherTile.getPosition().x < tile.getPosition().x){
				tileList.Add(tile);
			}
			else{
				tileList.Insert(0,tile);
			}
		}
	}

	public List<Tile> getTiles(){
		List<Tile> listToReturn = new List<Tile>(tileList);
		return listToReturn;
	}



	public Tile getOtherTile(Tile tile){

		if(tileList.Count < 2){
			return null;
		}
		else if(tile.Equals(tileList[0])){
			return tileList[1];
		}
		else {
			return tileList[0];
		}
	}

	public Vertex getOtherVertex(Vertex vertex){
		if(vertex.Equals (northVertex)){
			return southVertex;
		}
		else if(vertex.Equals(southVertex)){
			return northVertex;
		}
		else{
			return null;
		}
	}

	public BiomeTransistion getBiomeTransistion(){
		return this.biomeTransisition;
	}

	public void changeTransistionType(){


		if(tileList.Count < 2){

		}
		else{
			Tile tile1 = tileList[0];
			Tile tile2 = tileList[1];

			if(((tile1.getBiome()!= Biome.Ocean)&&(tile2.getBiome()==Biome.Ocean))||((tile1.getBiome()== Biome.Ocean)&&(tile2.getBiome()!=Biome.Ocean))){
				graphics.changeEdgePieces(edgeNumber,BiomeTransistion.Coast,this.centre,this.secondCentre,this.orientation);
				this.biomeTransisition = BiomeTransistion.Coast;
			}
			else{
				graphics.changeEdgePieces(edgeNumber,BiomeTransistion.None,this.centre,this.secondCentre, this.orientation);
				this.biomeTransisition = BiomeTransistion.None;
			}
		}

	}

	public void addRiver(){

		bool check =false;

		if(tileList.Count < 2){

		}
		else{
			if(tileList[0].getBiome()==Biome.Ocean||tileList[1].getBiome()==Biome.Ocean){
				check = true;
			}
		}


		if(this.biomeTransisition == BiomeTransistion.River||check){

		}
		else{
			if(tileList.Count == 2){
				graphics.buildRiverEdge(this.edgeNumber,this.getPosition(),this.secondCentre, this.orientation);
				this.biomeTransisition = BiomeTransistion.River;
				this.northVertex.setRiverJunction();
				this.southVertex.setRiverJunction();
			}
		}


	}

	public void removeRiver(){

		if(this.biomeTransisition == BiomeTransistion.River){
			graphics.deleteRiver(edgeNumber);
			this.biomeTransisition = BiomeTransistion.None;
			this.northVertex.setRiverJunction();
			this.southVertex.setRiverJunction();
		}



	}

	public bool isLand(){
		if(tileList.Count < 2){
			return false;
		}
		else if(tileList[0].getBiome() !=Biome.Ocean && tileList[1].getBiome()!=Biome.Ocean){
			return true;
		}
		else {
			return false;
		}
	}

	public float getAbsoluteDistancefrompixel(float Xp, float Yp){

		float Xe = centre.x;
		float Ye = centre.y;

		float x = (float) Math.Pow((Xp-Xe),2);
		float y = (float) Math.Pow((Yp-Ye),2);

		float dist = (float) Math.Abs(Math.Sqrt((x+y)));

		return dist;
	}



}
