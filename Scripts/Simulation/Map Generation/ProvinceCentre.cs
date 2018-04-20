using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProvinceCentre {

	static int compareWeight(ProvinceCentreWeight a, ProvinceCentreWeight b){
		return -a.getWeight().CompareTo(b.getWeight());
	}

	private Tile tile;
	private Tile previousTile;
	private float qPush;
	private float rPush;
	private float sPush;
	private List<ProvinceCentreWeight> weights;
	
	public ProvinceCentre(Tile nTile){
		tile = nTile;
		previousTile = null;

		qPush = 0f;
		rPush = 0f;
		sPush = 0f;
		weights = new List<ProvinceCentreWeight>();

	}

	public Tile getTile(){
		return tile;
	}

	public void moveTile(TileManager tM, List<Tile> currentCentreTiles, out List<Tile> newCentreTiles){

		float[] pushes = new float[3];
		pushes[0] = qPush;
		pushes[1] = rPush;
		pushes[2] = sPush;

		newCentreTiles = new List<Tile>();


		ProvinceCentreWeight weight = new ProvinceCentreWeight(1,-1,0,this,pushes);
		weights.Add (weight);

		weight = new ProvinceCentreWeight(1,0,-1,this,pushes);
		weights.Add (weight);

		weight = new ProvinceCentreWeight(0,1,-1,this,pushes);
		weights.Add (weight);

		weight = new ProvinceCentreWeight(-1,1,0,this,pushes);
		weights.Add (weight);

		weight = new ProvinceCentreWeight(-1,0,1,this,pushes);
		weights.Add (weight);

		weight = new ProvinceCentreWeight(0,-1,1,this,pushes);
		weights.Add (weight);

		weights.Sort(compareWeight);

		//Debug.Log(weights[0].getWeight()+": Q: "+weights[0].getQ()+": R: " + weights[0].getR()+": S: " + weights[0].getS ());
		//Debug.Log(weights[1].getWeight()+": Q: "+weights[1].getQ()+": R: " + weights[1].getR()+": S: " + weights[1].getS ());
		//Debug.Log(weights[2].getWeight()+": Q: "+weights[2].getQ()+": R: " + weights[2].getR()+": S: " + weights[2].getS ());
		//Debug.Log(weights[3].getWeight()+": Q: "+weights[3].getQ()+": R: " + weights[3].getR()+": S: " + weights[3].getS ());
		//Debug.Log(weights[4].getWeight()+": Q: "+weights[4].getQ()+": R: " + weights[4].getR()+": S: " + weights[4].getS ());
		//Debug.Log(weights[5].getWeight()+": Q: "+weights[5].getQ()+": R: " + weights[5].getR()+": S: " + weights[5].getS ());
		for(int i = 0; i < 6; i++){

			weight = weights[i];

			int tileQ = this.getTile().getQ();
			int tileR = this.getTile().getR();
			int tileS = this.getTile().getS();

			int weightQ = weight.getQ();
			int weightR = weight.getR();
			int weightS = weight.getS();

			Tile nextPossibleTile = tM.getTileQRS(tileQ+weightQ,tileR+weightR,tileS+weightS);

			if(nextPossibleTile.getBiome()!=Biome.Ocean&&!currentCentreTiles.Contains(nextPossibleTile)){
				previousTile = tile;
				tile = tM.getTileQRS(tileQ+weightQ,tileR+weightR,tileS+weightS);


				currentCentreTiles.Remove (tile);
				currentCentreTiles.Add (nextPossibleTile);
				newCentreTiles = currentCentreTiles;
				break;
				


			}
		}

		weights = new List<ProvinceCentreWeight>();
		resetPushes();
	}

	public void setPushes(int[] QRS, int distance){

		float distancef = (float) distance;

		qPush = (Mathf.Pow((float) QRS[0],1f)/((float) (Mathf.Pow(distancef,1f)))*(-10f)) + qPush;
		rPush = (Mathf.Pow((float) QRS[1],1f)/((float) (Mathf.Pow(distancef,1f)))*(-10f)) + rPush;
		sPush = (Mathf.Pow((float) QRS[2],1f)/((float) (Mathf.Pow(distancef,1f)))*(-10f)) + sPush;

		//Debug.Log (qPush + "," + rPush + "," + sPush);

	}

	public float[] getPushes(){
		float[] QRS = new float[3];

		QRS[0] = qPush;
		QRS[1] = rPush;
		QRS[2] = sPush;

		return QRS;
	}

	public void resetPushes(){
		qPush = 0f;
		rPush = 0f;
		sPush = 0f;
	}

	public void setTile(Tile nTile){
		tile = nTile;
	}


}
