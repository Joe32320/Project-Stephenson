using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class BoardGraphics : MonoBehaviour {

	public GameObject overLayPrefab;

	//0=Grass, 1=Water, 2=Desert, 3=Ice
	public GameObject[] tilePrefab;
	private GameObject grassPiece;
	private GameObject waterPiece;
	private GameObject desertPiece;
	private GameObject icePiece;


	//0=Coast
	public GameObject[] transistionPrefab;
	private GameObject coastPiece;
	private GameObject riverPiece;
	private GameObject riverJunction;
	private GameObject riverThrough;
	private GameObject riverEnd;
	private GameObject riverStart;


	public GameObject[] featurePrefab;
	private GameObject forestPiece;
	private GameObject hillPiece;
	private GameObject mountainPiece;
	private GameObject swampPiece;

	public GameObject[] resourcePrefab;


	private GameObject[] tilePieces;
	private GameObject[] edgePieces;
	private GameObject[] vertPieces;
	private GameObject[] featurePieces;
	private GameObject[] overLayPieces;
	private GameObject[] resourcePieces;

    public GameObject[] mapDetailsPrefab;

    private GameObject[] cityPieces;


    void Awake(){

		waterPiece = tilePrefab[0];
		grassPiece = tilePrefab[1];
		desertPiece = tilePrefab[2];
		icePiece = tilePrefab[3];

		coastPiece = transistionPrefab[0];
		riverPiece = transistionPrefab[1];
		riverJunction = transistionPrefab[2];
		riverThrough = transistionPrefab[3];
		riverEnd = transistionPrefab[4];
		riverStart = transistionPrefab[5];

		forestPiece = featurePrefab[0];
		hillPiece = featurePrefab[1];
		mountainPiece = featurePrefab[2];
		swampPiece = featurePrefab[3];
	


	}


	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setup(int numTiles){
		this.tilePieces = new GameObject[numTiles*2];
		this.featurePieces = new GameObject[numTiles*2];
		this.overLayPieces = new GameObject[numTiles*2];
        this.cityPieces = new GameObject[numTiles * 2];
        this.resourcePieces = new GameObject[numTiles * 2];
	}

	public void setUpEdgeNum(int numEdge){
		this.edgePieces = new GameObject[numEdge*2];
	}

	public void setUpVertexNum(int numVert){
		this.vertPieces = new GameObject[numVert*2];
	}

	//public void set


	public void buildTile(Vector3 position,Vector3 secondPosition, Biome biome, int tileNo){

		GameObject newTilePiece = grassPiece;
		newTilePiece.isStatic = true;

		switch (biome){
		case Biome.Grassland:
			newTilePiece = grassPiece;
			break;
		case Biome.Ocean:
			newTilePiece = waterPiece;
			break;
		case Biome.Desert:
			newTilePiece = desertPiece;
			break;
		case Biome.Ice:
			newTilePiece = icePiece;
			break;

		}
		if(tilePieces[tileNo] == null){
			
			GameObject test = GameObject.Instantiate(newTilePiece,position,Quaternion.identity) as GameObject;
			test.transform.parent = gameObject.transform;
			tilePieces[tileNo] = test;
			test.isStatic = true;

			GameObject test1 = GameObject.Instantiate(newTilePiece,secondPosition,Quaternion.identity) as GameObject;
			test1.transform.parent = gameObject.transform;
			tilePieces[tileNo+(tilePieces.Length/2)] = test1;
			test1.isStatic = true;


		}
		else{
			Destroy(tilePieces[tileNo]);
			Destroy(tilePieces[tileNo+(tilePieces.Length/2)]);

			GameObject test = GameObject.Instantiate(newTilePiece,position,Quaternion.identity) as GameObject;
			test.transform.parent = gameObject.transform;
			tilePieces[tileNo] = test;
			test.isStatic = true;

			GameObject test1 = GameObject.Instantiate(newTilePiece,secondPosition,Quaternion.identity) as GameObject;
			test1.transform.parent = gameObject.transform;
			tilePieces[tileNo+(tilePieces.Length/2)] = test1;
			test1.isStatic = true;
		}
	}

	public void buildFeatures(Vector3 position,Vector3 secondPosition, BiomeFeatures nFeature, int tileNo){

		GameObject newTilePiece = forestPiece;

		if(nFeature == BiomeFeatures.Clear || nFeature == BiomeFeatures.None){
			if(featurePieces[tileNo] !=null){
				Destroy(featurePieces[tileNo]);
			}

		}
		else{
			switch(nFeature){
			case BiomeFeatures.Forest:
				newTilePiece = forestPiece;
				break;
			case BiomeFeatures.Hill:
				newTilePiece = hillPiece;
				break;
			case BiomeFeatures.Mountain:
				newTilePiece = mountainPiece;
				break;
			case BiomeFeatures.Swamp:
				newTilePiece = swampPiece;
				break;
			}

			if(featurePieces[tileNo] == null){
				GameObject test = GameObject.Instantiate(newTilePiece,position,Quaternion.identity) as GameObject;
				test.transform.parent = gameObject.transform;
				featurePieces[tileNo] = test;
				test.isStatic = true;

				GameObject test1 = GameObject.Instantiate(newTilePiece,secondPosition,Quaternion.identity) as GameObject;
				test1.transform.parent = gameObject.transform;
				featurePieces[tileNo+(tilePieces.Length/2)] = test1;
				test1.isStatic = true;
			}
			else{
				Destroy(featurePieces[tileNo]);
				Destroy(featurePieces[tileNo+(tilePieces.Length/2)]);

				GameObject test = GameObject.Instantiate(newTilePiece,position,Quaternion.identity) as GameObject;
				test.transform.parent = gameObject.transform;
				featurePieces[tileNo] = test;
				test.isStatic = true;

				GameObject test1 = GameObject.Instantiate(newTilePiece,secondPosition,Quaternion.identity) as GameObject;
				test1.transform.parent = gameObject.transform;
				featurePieces[tileNo+(tilePieces.Length/2)] = test1;
				test1.isStatic = true;
				//Color color = new Color(1,0,0);
				//test.GetComponent<SpriteRenderer>().color = color;
			}


		}
	}

	public void changeEdgePieces(int edgeNum, BiomeTransistion transistion, Vector3 position, Vector3 secondPosition, EdgeOrientation orientate){

		int angle = 0;
		Vector3 z = new Vector3(0,0,1);

		if(orientate == EdgeOrientation.NS){
			angle = 0;
		}
		else if(orientate == EdgeOrientation.NWSE){
			angle = 60;
		}
		else{
			angle = -60;
		}


		if(transistion.Equals(BiomeTransistion.Coast)){
			GameObject newEdgepiece = coastPiece;

			if(edgePieces[edgeNum] == null){
				GameObject test = GameObject.Instantiate(newEdgepiece,position,Quaternion.AngleAxis(angle,z)) as GameObject;
				edgePieces[edgeNum] = test;
				test.transform.parent = gameObject.transform;
				test.isStatic = true;

				GameObject test1 = GameObject.Instantiate(newEdgepiece,secondPosition,Quaternion.AngleAxis(angle,z)) as GameObject;
				edgePieces[edgeNum+(edgePieces.Length/2)] = test1;
				test1.transform.parent = gameObject.transform;
				test1.isStatic = true;
			}
			else{
				Destroy(edgePieces[edgeNum]);
				GameObject test = GameObject.Instantiate(newEdgepiece,position,Quaternion.AngleAxis(angle,z)) as GameObject;
				edgePieces[edgeNum] = test;
				test.transform.parent = gameObject.transform;
				test.isStatic = true;

				Destroy(edgePieces[edgeNum+(edgePieces.Length/2)]);
				GameObject test1 = GameObject.Instantiate(newEdgepiece,secondPosition,Quaternion.AngleAxis(angle,z)) as GameObject;
				edgePieces[edgeNum+(edgePieces.Length/2)] = test1;
				test1.transform.parent = gameObject.transform;
				test1.isStatic = true;
			}

		}

		if(transistion.Equals(BiomeTransistion.None)){

			if(edgePieces[edgeNum]!= null){
				Destroy(edgePieces[edgeNum]);
				Destroy(edgePieces[edgeNum+(edgePieces.Length/2)]);
			}

		}
	}

	public void buildRiverEdge(int edgeNum, Vector3 position,Vector3 secondPosition, EdgeOrientation orientate){

		int angle = 0;
		Vector3 z = new Vector3(0,0,1);
		
		if(orientate == EdgeOrientation.NS){
			angle = 0;
		}
		else if(orientate == EdgeOrientation.NWSE){
			angle = 60;
		}
		else{
			angle = -60;
		}



		GameObject newPiece = riverPiece;
		if(edgePieces[edgeNum]==null){
			GameObject test = GameObject.Instantiate(newPiece,position,Quaternion.AngleAxis(angle,z)) as GameObject;
			edgePieces[edgeNum] = test;
			test.transform.parent = gameObject.transform;
			test.isStatic = true;

			GameObject test1 = GameObject.Instantiate(newPiece,secondPosition,Quaternion.AngleAxis(angle,z)) as GameObject;
			edgePieces[edgeNum+(edgePieces.Length/2)] = test1;
			test1.transform.parent = gameObject.transform;
			test1.isStatic = true;
		}
		else{
			Destroy(edgePieces[edgeNum]);
			GameObject test = GameObject.Instantiate(newPiece,position,Quaternion.AngleAxis(angle,z)) as GameObject;
			edgePieces[edgeNum] = test;
			test.transform.parent = gameObject.transform;
			test.isStatic = true;

			Destroy(edgePieces[edgeNum+(edgePieces.Length/2)]);
			GameObject test1 = GameObject.Instantiate(newPiece,secondPosition,Quaternion.AngleAxis(angle,z)) as GameObject;
			edgePieces[edgeNum+(edgePieces.Length/2)] = test1;
			test1.transform.parent = gameObject.transform;
			test1.isStatic = true;
		}
	}

	// type=0 is a junction, type=1 is a through, type=2 is a delta, type=3 is a spring, type=4 Delete
	public void buildRiverVertex(int junction, VertOrientation orientate, bool up, bool east, bool west, int vertNum, Vector3 position,Vector3 secondPosition){

		int angle = 0;
		GameObject piece = riverEnd;

		if(junction == 0){
			if(orientate == VertOrientation.Up){
				angle = 180;
			}
			piece = riverJunction;
		}
		else if(junction == 1){

			if(up&&west){
				if(orientate==VertOrientation.Down){
					angle = 0;
				}
				else{
					angle = 300;
				}
			}
			else if(up&&east){
				if(orientate == VertOrientation.Down){
					angle = 120;
				}
				else{
					angle = 180;
				}
			}
			else if(east&&west){
				if(orientate==VertOrientation.Down){
					angle = 240;
				}
				else{
					angle = 60;
				}
			}

			piece = riverThrough;



		}
		else if(junction == 2||junction == 3){
			if(up){
				if(orientate==VertOrientation.Down){
					angle = 0;
				}
				else{
					angle = 180;
				}
			}
			else if(east){
				if(orientate == VertOrientation.Down){
					angle = 120;
				}
				else{
					angle = 60;
				}
			}
			else{
				if(orientate==VertOrientation.Down){
					angle = 240;
				}
				else{
					angle = 300;
				}
			}

			if(junction == 2){
				piece = riverEnd;
			}
			else{
				piece = riverStart;
			}
		}

		if(vertPieces[vertNum] != null){
			Destroy(vertPieces[vertNum]);
			Destroy(vertPieces[vertNum+(vertPieces.Length/2)]);
		}
		GameObject test = GameObject.Instantiate(piece, position,Quaternion.AngleAxis(angle,new Vector3(0,0,1))) as GameObject;
		vertPieces[vertNum] = test;
		test.transform.parent = gameObject.transform;
		test.isStatic = true;

		GameObject test1 = GameObject.Instantiate(piece, secondPosition,Quaternion.AngleAxis(angle,new Vector3(0,0,1))) as GameObject;
		vertPieces[vertNum+(vertPieces.Length/2)] = test1;
		test1.transform.parent = gameObject.transform;
		test1.isStatic = true;

		if(junction == 4){
			if(vertPieces[vertNum]!=null){
				Destroy(vertPieces[vertNum]);
				Destroy(vertPieces[vertNum+(vertPieces.Length/2)]);
			}
		}

	}

	public void deleteRiver(int edgeNum){

		if(edgePieces[edgeNum]!=null){
			Destroy(edgePieces[edgeNum]);
			Destroy(edgePieces[edgeNum+(edgePieces.Length/2)]);
		}


	}


	public void buildCollider(Tile tile){


		Vector3[] vertices = new Vector3[4];
		int[] triangles = new int[6];

		vertices[0] = new Vector3(tile.getPosition().x/-2f,0,0);
		vertices[1] = new Vector3(tile.getPosition().x/-2f,tile.getPosition().y+1.5f,0);
		vertices[2] = new Vector3(tile.getPosition().x*1.5f+1.5f,tile.getPosition().y+1.5f,0);
		vertices[3] = new Vector3(tile.getPosition().x*1.5f+1.5f,0,0);

		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 3;

		triangles[3] = 1;
		triangles[4] = 2;
		triangles[5] = 3;

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;


		gameObject.AddComponent<MeshCollider>();

		MeshCollider mc = GetComponent<MeshCollider>();

		mc.sharedMesh = mesh;


	}

	public void buildOverlay(Vector3 position,Vector3 secondPosition,int tileNo,float r, float g, float b){

		GameObject newTilePiece = overLayPrefab;
		newTilePiece.GetComponent<SpriteRenderer>().color = new Color(r,g,b);

		if(overLayPieces[tileNo] == null){
			GameObject test = GameObject.Instantiate(newTilePiece,position,Quaternion.identity) as GameObject;
			test.transform.parent = gameObject.transform;
			overLayPieces[tileNo] = test;
			test.isStatic = true;

			GameObject test1 = GameObject.Instantiate(newTilePiece,secondPosition,Quaternion.identity) as GameObject;
			test1.transform.parent = gameObject.transform;
			overLayPieces[tileNo+(overLayPieces.Length/2)] = test1;
			test1.isStatic = true;

		}
		else{
			Destroy(overLayPieces[tileNo]);
			GameObject test = GameObject.Instantiate(newTilePiece,position,Quaternion.identity) as GameObject;
			test.transform.parent = gameObject.transform;
			overLayPieces[tileNo] = test;
			test.isStatic = true;

			Destroy(overLayPieces[tileNo+(overLayPieces.Length/2)]);
			GameObject test1 = GameObject.Instantiate(newTilePiece,secondPosition,Quaternion.identity) as GameObject;
			test1.transform.parent = gameObject.transform;
			overLayPieces[tileNo+(overLayPieces.Length/2)] = test1;
			test1.isStatic = true;

		}




	}

    public void buildCity(Vector3 position, Vector3 secondPosition, int tileNo)
    {
        GameObject newCityPiece = mapDetailsPrefab[0];
        if (cityPieces[tileNo] == null)
        {
            GameObject test = GameObject.Instantiate(newCityPiece, position, Quaternion.identity) as GameObject;
            test.transform.parent = gameObject.transform;
            cityPieces[tileNo] = test;
            test.isStatic = true;

            GameObject test1 = GameObject.Instantiate(newCityPiece, secondPosition, Quaternion.identity) as GameObject;
            test1.transform.parent = gameObject.transform;
            cityPieces[tileNo + (cityPieces.Length / 2)] = test1;
            test1.isStatic = true;
        }
        else
        {
            Destroy(cityPieces[tileNo]);
            GameObject test = GameObject.Instantiate(newCityPiece, position, Quaternion.identity) as GameObject;
            test.transform.parent = gameObject.transform;
            cityPieces[tileNo] = test;
            test.isStatic = true;

            Destroy(cityPieces[tileNo + (cityPieces.Length / 2)]);
            GameObject test1 = GameObject.Instantiate(newCityPiece, secondPosition, Quaternion.identity) as GameObject;
            test1.transform.parent = gameObject.transform;
            cityPieces[tileNo + (cityPieces.Length / 2)] = test1;
            test1.isStatic = true;
        }

    }

	public void destroyOverLay(int tileNo){



	}


}
