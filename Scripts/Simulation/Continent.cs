using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Continent {

	private List<Tile> tiles;
	private HashSet<Province> provinces;
	private TileManager tM;

	public Continent(List<Tile> tileList){
		tiles = tileList;
	}

	public void generateProvinces(int count){
		ProvinceGenerator gen = new ProvinceGenerator(tiles,count);
		provinces = gen.generateNew();
	}

	public void generateCountries(int eights, int fours){

		CountryGenerator gen = new CountryGenerator(provinces, eights, fours);
		gen.generate();
	}

	public List<Province> getProvinces(){
		return new List<Province>(provinces);
	}

	public int getProvinceNum(){
		return provinces.Count;
	}
}
