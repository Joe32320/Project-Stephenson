using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum VertOrientation{
	Up,
	Down,
}


public class Vertex{
	
	private VertOrientation orientation;
	private Vector3 position;
	private Vector3 secondPosition;

	private Edge upOrDownEdge;
	private Edge eastWardEdge;
	private Edge westWardEdge;

	private BoardGraphics graphics;
	private int vertNum;

	private float height;
	private bool isRiver;

	public Vertex(Vector3 nPosition, VertOrientation nOrientation, BoardGraphics nGraphics, int num,bool isInWestHalf, float mapX){

		position = nPosition;
		if(isInWestHalf){
			secondPosition = new Vector3(position.x+mapX,position.y,position.z);
		}
		else{
			secondPosition = new Vector3(position.x-mapX,position.y,position.z);
		}
		orientation = nOrientation;
		graphics = nGraphics;
		vertNum = num;
		isRiver = false;
		height = -500f;

	}

	public int getVertNum(){
		return this.vertNum;
	}

	public Vector3 getPosition(){
		return position;
	}

	public VertOrientation getOrientation(){
		return orientation;
	}

	public float getHeight(){
		return height;
	}

	public bool setHeight(){

		if(height > -100f){
			return false;
		}
		else{

			List<Edge> edgeList = new List<Edge>();

			edgeList.Add(upOrDownEdge);
			edgeList.Add(westWardEdge);
			edgeList.Add(eastWardEdge);

			float average = 0.0f;
			float count = 0.0f;

			for(int i = 0; i < edgeList.Count; i++){

				Edge edge = edgeList[i];

				if(edge != null){
					List<Tile> tileList = edge.getTiles();
					
					for(int j = 0; j < tileList.Count; j++){
						average = average + tileList[j].getHeight();
						count = count + 1.0f;
					}
				}

			}

			height = (average/count);



			return true;
		}


	}


	//0 Up/Down, 1 East, 2 West
	public void addEdge(int pos, Edge edge){

		if(pos == 0){
			upOrDownEdge = edge;
		}
		else if(pos == 1){
			eastWardEdge = edge;
		}
		else if(pos == 2){
			westWardEdge = edge;
		}
		else{

		}
	}

	public void setRiverJunction(){

		if(upOrDownEdge==null||eastWardEdge==null||westWardEdge==null){}
		else{
			BiomeTransistion uBT = upOrDownEdge.getBiomeTransistion();
			BiomeTransistion eBT = eastWardEdge.getBiomeTransistion();
			BiomeTransistion wBT = westWardEdge.getBiomeTransistion();

			bool uCheck = (uBT == BiomeTransistion.River);
			bool eCheck = (eBT == BiomeTransistion.River);
			bool wCheck = (wBT == BiomeTransistion.River);

			bool uCoast = (uBT == BiomeTransistion.Coast);
			bool eCoast = (eBT == BiomeTransistion.Coast);
			bool wCoast = (wBT == BiomeTransistion.Coast);

			int coastType = 3;

			if(uCoast||eCoast||wCoast){
				coastType = 2;
			}


			if(uCheck&&eCheck&&wCheck){
				graphics.buildRiverVertex(0,this.orientation,true,true,true, this.vertNum, position,secondPosition);
			}
			else if(uCheck&&eCheck&&!wCheck){
				graphics.buildRiverVertex(1,this.orientation,true,true,false,this.vertNum,position,secondPosition);
			}
			else if(uCheck&&!eCheck&&wCheck){
				graphics.buildRiverVertex(1,this.orientation,true,false,true,this.vertNum,position,secondPosition);
			}
			else if(!uCheck&&eCheck&&wCheck){
				graphics.buildRiverVertex(1,this.orientation,false,true,true,this.vertNum,position,secondPosition);
			}
			else if(uCheck){
				graphics.buildRiverVertex(coastType,this.orientation,true,false,false,this.vertNum,position,secondPosition);
			}
			else if(eCheck){
				graphics.buildRiverVertex(coastType,this.orientation,false,true,false,this.vertNum,position,secondPosition);
			}
			else if(wCheck){
				graphics.buildRiverVertex(coastType,this.orientation,false,false,true,this.vertNum,position,secondPosition);
			}
			else{
				graphics.buildRiverVertex(4,this.orientation,false,false,false,vertNum,position,secondPosition);
			}


		}


	}

	//0 is up edge, 1 is east edge, 2 is west edge
	public Edge getEdge(int no){

		if(no == 0){
			return upOrDownEdge;
		}
		else if(no == 1){
			return eastWardEdge;
		}
		else if(no == 2){
			return westWardEdge;
		}
		else{
			return null;
		}


	}

	public void swapInEdges(Vertex otherVertex){

		if(upOrDownEdge == null){
			upOrDownEdge = otherVertex.getEdge(0);
		}
		else if(eastWardEdge == null){
			eastWardEdge = otherVertex.getEdge(1);
		}
		else if(westWardEdge == null){
			westWardEdge = otherVertex.getEdge(2);
		}

	}

	public void setIsVertexRiver(bool condition ){
		isRiver = condition;
	}

	public bool isVertexRiver(){
		return isRiver;
	}

	public bool isCoast(){
		bool check = false;

		int noLandEdges = 0;

		if(upOrDownEdge.isLand ()){
			noLandEdges = noLandEdges + 1;
		}
		if(eastWardEdge.isLand ()){
			noLandEdges = noLandEdges + 1;
		}
		if(westWardEdge.isLand ()){
			noLandEdges = noLandEdges + 1;
		}

		if(noLandEdges == 1){
			check = true;
		}
		return check;
	}


}
