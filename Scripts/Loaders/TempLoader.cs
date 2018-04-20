using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class TempLoader{


	private List<Resource> resourceList;

	public TempLoader(){

		resourceList = new List<Resource>();

		Resource res;

		res = new Resource("Coal",1f);
		resourceList.Add(res);

		res = new Resource("Iron",1f);
		resourceList.Add(res);

		res = new Resource("Steel",1f);
		resourceList.Add(res);

		res = new Resource("Machinery",1f);
		resourceList.Add(res);




	}



	public Dictionary<Resource,float> setUpPriceList(){

		Dictionary<Resource,float> priceList = new Dictionary<Resource, float>();

		for(int i = 0; i < this.resourceList.Count; i++){
			priceList.Add(this.resourceList[i],0f);
		}

		return priceList;


	}

	public Dictionary<Resource,float> setUpStockList(){

		Dictionary<Resource,float> stockList = new Dictionary<Resource, float>();
		
		for(int i = 0; i < this.resourceList.Count; i++){
			stockList.Add(this.resourceList[i],0f);
		}
		
		return stockList;

	}



}
