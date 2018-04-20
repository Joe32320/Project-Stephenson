using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Graphs{public class Node{

		private List<Arc> arcs;
		private int colour; //For problems that are related to graph "colouring" i.e four colour map theorm.

		public Node(){
			arcs = new List<Arc>();
		}

		public void addArc(Arc arc){
			arcs.Add(arc);
		}

		public List<Arc> getArcs(){
			return new List<Arc>(arcs);
		}
	}
}
