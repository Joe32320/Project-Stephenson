using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Country {

	private List<Province> provinces;
	private Color overlayColor;

	public Country(List<Province> nProvinces, Color nOverLayColor){
		provinces = nProvinces;
		overlayColor = nOverLayColor;
	}



}
