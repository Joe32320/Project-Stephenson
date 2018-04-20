using UnityEngine;
using System.Collections;

public class ProvinceCentreWeight{

	private int q;
	private int r;
	private int s;
	private float weight;
	private ProvinceCentre centre;

	public ProvinceCentreWeight(int nQ, int nR, int nS, ProvinceCentre nCentre, float[] pushes){

		q=nQ;
		r=nR;
		s=nS;

		centre = nCentre;

		weight =  Mathf.Abs(pushes[0]-q)+Mathf.Abs(pushes[1]-r)+Mathf.Abs(pushes[2]-s);

	}

	public float getWeight(){
		return weight;
	}

	public int getQ(){
		return q;
	}

	public int getR(){
		return r;
	}

	public int getS(){
		return s;
	}


}
