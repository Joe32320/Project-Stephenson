using UnityEngine;
using System.Collections;

public class Resource{

	private string name;
	private float weight;

	public Resource(string nName, float nWeight){
		name = nName;
		weight = nWeight;
	}

	public string getName(){
		return name;
	}

	public float getWeight(){
		return weight;
	}

}
