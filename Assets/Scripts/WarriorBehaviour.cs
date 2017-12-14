using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBehaviour : PlayerBehaviour {

	private float specialValue = 10f;

	void Start () {
		stateAttack = 2;
	}

	public float SpecialValue{
		get{
			return specialValue;
		}
		set{
			specialValue = value;
		}
	}
	// Use this for initialization

	public override void SpecialCommand (List<GameObject> enemies){
	}
			
	public override string Name{
		get{ return "Guerreiro";}
		set{}
	}

	public override void FinishSpecial (){
	}
		
}
