using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public Animator anim;
	
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter (Collider other) 
	{
		if (other.CompareTag("Player"))
		{
			anim.SetBool ("DoorOpen", true);
			anim.SetBool ("DoorClose", false);
		}

	}

	void OnTriggerExit (Collider other) 
	{
		if (other.CompareTag("Player"))
		{
			anim.SetBool ("DoorOpen", false);
			anim.SetBool ("DoorClose", true);
		}
		

	}
	
}
