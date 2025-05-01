using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public Animator anim;
	
	[Space(10)]
	[Header("Health Properties")]
	public int Health = 250;
	public int MaxHealth = 250;
	
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
	
	public void Hurt(int damage)
	{
		Health -= damage;
	}
	
	
}
