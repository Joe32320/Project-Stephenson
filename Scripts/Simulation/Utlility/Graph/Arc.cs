using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Graphs{
	public class Arc{

		private List<Node> nodes;
		private int weight;

		public Arc(Node firstNode, Node secondNode, int nWeight){
			nodes = new List<Node>();

			nodes.Add(firstNode);
			nodes.Add(secondNode);
			weight = nWeight;
		}

		public int getWeight(){
			return weight;
		}

		public Node getNeighbourNode(Node node){
			if(nodes[0].Equals(node)){
				return nodes[1];
			}
			else{
				return nodes[0];
			}
		}
	}
}
