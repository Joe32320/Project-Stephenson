using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphs{

	public class Graph<T>{

		private Dictionary<T,int> tToNodeDic;
		private HashSet<int> nodeIntList;
		private Dictionary<int,int> arcWeightList;
		private Dictionary<int,HashSet<int>> nodeArcList;
		private Dictionary<int, HashSet<int>> arcNodeList;

		private int maxColour; //For graph colouring problems, represents the total number of colours used, so if only 1 colour used, maxColour == 1, i.e like List.Count. 


		public Graph(){
			
			tToNodeDic = new Dictionary<T, int>();
			nodeIntList = new HashSet<int>();
			arcWeightList = new Dictionary<int, int>();
			nodeArcList = new Dictionary<int, HashSet<int>>();
			arcNodeList = new Dictionary<int,HashSet<int>>();

		}

		public int addIntNode(T toNode){
			int node = nodeIntList.Count;
			tToNodeDic.Add(toNode,node);
			nodeIntList.Add(node);
			HashSet<int> arcList = new HashSet<int>();
			nodeArcList.Add(node,arcList);
			return node;
		}

		public void addIntEdge(T first, T second, int weight){
			if(tToNodeDic.ContainsKey(first)&&tToNodeDic.ContainsKey(second)){
				int arc = arcWeightList.Count;
				arcWeightList.Add(arc,weight);
				HashSet<int> nodesList = new HashSet<int>();
				nodesList.Add(tToNodeDic[first]);
				nodesList.Add(tToNodeDic[second]);
				arcNodeList.Add(arc,nodesList);

				nodeArcList[tToNodeDic[first]].Add(arc);
				nodeArcList[tToNodeDic[second]].Add(arc);

			}
			else{
				Debug.Log("Tried to create edge between node(s) that didn't exist");
			}
		}

		private int getNeighbourNode(int arc, int node){
			List<int> nodes = arcNodeList[arc].ToList();

			int neighbour = -1;

			for(int i = 0; i < nodes.Count; i++){
				if(nodes[i] != node){
					neighbour = nodes[i];
				}
			}

			if(neighbour==-1||nodes.Count > 2){
				throw new System.ArgumentException("Error");


			}
			return neighbour;
		}

		public HashSet<HashSet<int>> findKCut(int cutSize,HashSet<T> startingNodes, bool findMinCut){

			Dictionary<int, KeyValuePair<HashSet<int>,int>> cuts = new Dictionary<int, KeyValuePair<HashSet<int>, int>>();

			foreach(T startingNode in startingNodes){

				int startNode = tToNodeDic[startingNode];
				HashSet<int> visitedNodes = new HashSet<int>();
				visitedNodes.Add(startNode);

				HashSet<int> possibleNodes = new HashSet<int>();
				HashSet<int> startArcList = nodeArcList[startNode];
				int cut = 0;

				foreach(int arcNum in startArcList){
					int neighNode = getNeighbourNode(arcNum,startNode);
					possibleNodes.Add(neighNode);
					cut = cut + arcWeightList[arcNum];
				}

				int finalCut = 0;

				for(int j = 0; j < cutSize - 1; j++){

					int minimumNode = -1;
					int minimumCut = int.MaxValue;

					foreach(int possibleNode in possibleNodes){
						int tempCut = cut;
						HashSet<int> possibleNodeArcs = nodeArcList[possibleNode];

						foreach(int arcNum in possibleNodeArcs){
							int neighNode = getNeighbourNode(arcNum,possibleNode);

							if(!visitedNodes.Contains(neighNode)){
								tempCut = tempCut + arcWeightList[arcNum];
							}
							else{
								tempCut = tempCut - arcWeightList[arcNum];
							}

						}

						if(tempCut < minimumCut){
							minimumCut = tempCut;
							minimumNode = possibleNode;
						}

					}

					visitedNodes.Add(minimumNode);
					possibleNodes.Remove(minimumNode);

					HashSet<int> minimumNodeArcs = nodeArcList[minimumNode];

					foreach(int arcNum in minimumNodeArcs){
						int neighNode = getNeighbourNode(arcNum,minimumNode);


						if(!visitedNodes.Contains(neighNode)){
							possibleNodes.Add(neighNode);
							cut = cut + arcWeightList[arcNum];
						}
						else{
							cut = cut - arcWeightList[arcNum];
						}
					}

					finalCut = cut;
				}

				if(!findMinCut){
					if(areNodesConnected(visitedNodes)){

						KeyValuePair<HashSet<int>,int> pair = new KeyValuePair<HashSet<int>, int>(visitedNodes,finalCut);
						cuts.Add(tToNodeDic[startingNode],pair);

						HashSet<int> firstList = pair.Key;
						HashSet<int> secondList = new HashSet<int>();

						foreach(int node in nodeIntList){
							if(!firstList.Contains(node)){
								secondList.Add(node);
							}
						}

						HashSet<HashSet<int>> nodesList = new HashSet<HashSet<int>>();
						nodesList.Add(firstList);
						nodesList.Add(secondList);
						return nodesList;

					}
				}
				else{
					KeyValuePair<HashSet<int>,int> pair = new KeyValuePair<HashSet<int>, int>(visitedNodes,finalCut);
					cuts.Add(tToNodeDic[startingNode],pair);
				}




			}

			//return null;

			if(cuts.Count==0){
				return null;
			}
			else{
				HashSet<int> nodes = null;
				int cut = int.MaxValue;

				foreach(KeyValuePair<HashSet<int>,int> value in cuts.Values){

					if(cut > value.Value){
						if(areNodesConnected(value.Key)){
							nodes = value.Key;
							cut = value.Value;
						}
					}

				}

				HashSet<int> firstList = nodes;
				HashSet<int> secondList = new HashSet<int>();

				foreach(int node in nodeIntList){
					if(!firstList.Contains(node)){
						secondList.Add(node);
					}
				}

				HashSet<HashSet<int>> nodesList = new HashSet<HashSet<int>>();
				nodesList.Add(firstList);
				nodesList.Add(secondList);
				return nodesList;
			}




		}


		private bool areNodesConnected(HashSet<int> cutA){
			
			bool doTwoConnectedGraphsExist = false;

			List<int> cutB = new List<int>(cutA.Count+10);

			foreach(int node in nodeIntList){
				if(!cutA.Contains(node)){
					cutB.Add(node);
				}
			}

			bool check = true;
			int startNode = cutB[0];
			HashSet<int> visitedNodes = new HashSet<int>();
			visitedNodes.Add(startNode);

			Queue<int> queue = new Queue<int>();
			queue.Enqueue(startNode);

			while(queue.Count > 0){

				int node = queue.Dequeue();

				foreach(int arc in nodeArcList[node]){
					int neigh = getNeighbourNode(arc,node);
					if(!cutA.Contains(neigh)&&!visitedNodes.Contains(neigh)){
						queue.Enqueue(neigh);
						visitedNodes.Add(neigh);
					}
				}
			}

			if(visitedNodes.Count == cutB.Count){
				doTwoConnectedGraphsExist = true;
			}

			return doTwoConnectedGraphsExist;
		}



	}

}

