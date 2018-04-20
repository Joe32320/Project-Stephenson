using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Province {


	private HashSet<Tile> tiles;
	private Color colour;
	private HashSet<Province> neighbouringProvinces;
    private City city;

	public Province(Color nColour, HashSet<Tile> nTiles){
		colour = nColour;
		tiles = nTiles;

		foreach(Tile tile in tiles){
			tile.setProvince(this);
		}

		neighbouringProvinces = new HashSet<Province>();
	}

    public void addCity(Tile t)
    {
        city = new City(t, this);

    }

	public int provinceSize(){
		return tiles.Count;
	}

	public List<Tile> getTiles(){
		return new List<Tile>(tiles);
	}

	public void addTile(Tile tile){
		tiles.Add(tile);
		tile.setProvince(this);
	}

	public void removeTile(Tile tile){
		tiles.Remove(tile);
		tile.setProvince(null);
	}

	public void setColour(Color nColour){

		//float r = (colour.r + nColour.r*3f)/4f;
		//float b = (colour.b + nColour.b*3f)/4f;
		//float g = (colour.g + nColour.g*3f)/4f;


		colour = nColour;//new Color(r,g,b);



	}

	public void overlayColour(){
		foreach(Tile tile in tiles){
			tile.setOverlay(colour.r,colour.g,colour.b);
		}
	}

	public HashSet<Province> findBorderingProvinces(){

		neighbouringProvinces = new HashSet<Province>();

		foreach(Tile tile in tiles){

			for(int j = 0; j < 6; j++){

				Tile neighbour = tile.getEdge(j).getOtherTile(tile);

				if(neighbour.getProvince()!=null){


					if(!neighbour.getProvince().Equals(this)){

						if(!neighbouringProvinces.Contains(neighbour.getProvince())){

							neighbouringProvinces.Add (neighbour.getProvince());
						}
					}
				}
			}
		}

		return new HashSet<Province>(neighbouringProvinces);
	}

	public Dictionary<Province,int> findBorderLengths(){

		Dictionary<Province,int> borderLengths = new Dictionary<Province, int>();

		foreach(Tile tile in tiles){
			for(int j = 0; j < 6; j++){

				if(tile.getEdge(j).getOtherTile(tile).getProvince()!=null&&tile.getEdge(j).getOtherTile(tile).getProvince()!=this){

					Province province = tile.getEdge(j).getOtherTile(tile).getProvince();

					if(borderLengths.ContainsKey(province)){
						borderLengths[province] = borderLengths[province] + 1;
					}
					else{
						borderLengths.Add(province,1);
					}
				}
			}
		}
		return borderLengths;
	}

	//Returns tiles bordering otherProvince that are in this province
	public List<Tile> findBorderingTiles(Province otherProvince){

		List<Tile> neighbourTiles = new List<Tile>();

		foreach(Tile tile in tiles){
			for(int j = 0; j < 6; j++){

				Tile neighbour = tile.getEdge(j).getOtherTile(tile);

				if(neighbour.getProvince() !=null){

					if(neighbour.getProvince().Equals(otherProvince)&&!neighbourTiles.Contains(tile)){
						neighbourTiles.Add(tile);
						break;
					}
				}
			}
		}

		return neighbourTiles;
	}


}
