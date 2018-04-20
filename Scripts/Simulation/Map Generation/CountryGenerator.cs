using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CountryGenerator {

	HashSet<Province> provinceList;
	Dictionary<int,Province> provinceNodeDic;
	Dictionary<Province, Dictionary<Province,int>> provinceBorderLengths;
	Graphs.Graph<Province> graph;
	List<KeyValuePair<List<Province>,bool>> provinceSpilts;
	int sizeTwoCountries;
	int sizeOneCountries;

	public CountryGenerator(HashSet<Province> nProvinceList, int eights, int fours){

		this.provinceList = nProvinceList;
		sizeTwoCountries = eights;
		sizeOneCountries = fours;

	}

	public void generate(){

		findBorderLengths();
		setUpGraph();
		findFourSizedCuts();

	}

	private void findBorderLengths(){

		provinceBorderLengths = new Dictionary<Province, Dictionary<Province, int>>();
		foreach(Province province in provinceList){
			
			Dictionary<Province, int> borderLengths = province.findBorderLengths();
			provinceBorderLengths.Add(province,borderLengths);

		}

	}

	private void setUpGraph(){
		
		graph = new Graphs.Graph<Province>();
		provinceNodeDic = new Dictionary<int, Province>();

		//Create Nodes
		foreach(Province province in provinceList){
			
			provinceNodeDic.Add(graph.addIntNode(province),province);

		}

		//Create Edges

		List<Province> visited = new List<Province>();

		foreach(Province province in provinceList){


			Dictionary<Province,int> borderLengths = provinceBorderLengths[province];

			foreach(Province nPro in borderLengths.Keys){

				if(!visited.Contains(nPro)){
					graph.addIntEdge(province,nPro,borderLengths[nPro]);
					//Debug.Log("Edge created");
				}

			}
			visited.Add(province);
		}

	}

	private void findFourSizedCuts(){

		provinceSpilts = new List<KeyValuePair<List<Province>, bool>>();

		KeyValuePair<HashSet<Province>,bool> fullList = new KeyValuePair<HashSet<Province>, bool>(provinceList,true);

		HashSet<HashSet<int>> nodesList = graph.findKCut(fullList.Key.Count/2,fullList.Key,true);
        //Debug.Log("passed");

        foreach (HashSet<int> nodeList in nodesList){
			
			Color color = new Color(UnityEngine.Random.value,UnityEngine.Random.value,UnityEngine.Random.value);
			foreach(int node in nodeList){
				Province province = provinceNodeDic[node];
				//province.setColour(color);
				//province.overlayColour();

			}
		}

	}

	private void createCountries(){

		if(provinceList.Count == 8 && sizeTwoCountries == 1){
			Country country = new Country(new List<Province>(provinceList), new Color(UnityEngine.Random.value,UnityEngine.Random.value,UnityEngine.Random.value));
		}
		else{

			int eightsCount = 0;
			int tempProvincesNum = provinceList.Count;

			while(tempProvincesNum != 8){
				tempProvincesNum = tempProvincesNum / 2;
				eightsCount = eightsCount + 1;
			}






		}



	}






}
