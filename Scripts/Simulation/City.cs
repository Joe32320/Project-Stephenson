using UnityEngine;
using System.Collections;

public class City{

	private string name;
	private Tile tile;
	private Province province;

	public City(Tile t, Province p){

        tile = t;
        province = p;

        t.addCity(this);
        t.changeBiomeFeature(BiomeFeatures.Clear);

	}


}
