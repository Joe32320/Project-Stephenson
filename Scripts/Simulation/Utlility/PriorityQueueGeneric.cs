using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class PriorityQueueGeneric<T>{
	
	List<KeyValuePair<T,float>> data;

	public PriorityQueueGeneric(){
		data = new List<KeyValuePair<T,float>>();
	}

	public void push(KeyValuePair<T, float> value){


		if(data.Count == 0){
			data.Add(value);
		}
		else{
			for(int i = 0; i < data.Count; i++){
				if(value.Value > data[i].Value){				
					data.Insert(i,value);
					break;
				}
				else if(i + 1 == count()){
					data.Add(value);
					break;
				}
			}
		}


		//Debug.Log ("After Push count:" + data.Count);
	}

	public KeyValuePair<T,float> popTop() {

		if(isEmpty()){
			return new KeyValuePair<T, float>(default(T), 0);
		}
		else{
			//Debug.Log ("Logged");
			KeyValuePair<T,float> returnValue = data[0];
			data.RemoveAt(0);
			//Debug.Log ("After pop Count:" + data.Count);
			return returnValue;
		}
	}

	public KeyValuePair<T,float> popBottom(){
		if(isEmpty()){
			return new KeyValuePair<T, float>(default(T), 0);
		}
		else{
			KeyValuePair<T, float> returnValue = data[count() - 1];
			data.RemoveAt(count() - 1);
			return returnValue;
		}
	}

	public int count(){
		return data.Count;
	}

	public bool isEmpty(){
		if(data.Count == 0){
			return true;
		}
		else{
			return false;
		}
	}

}
