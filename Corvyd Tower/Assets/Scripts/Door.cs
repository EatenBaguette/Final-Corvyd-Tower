using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {
	public Animator anim;
	
	private RectMask2D _healthBar;
	
	private int _healthBarMin = -113;
	private int _healthBarMax = -329;

	private float _healthBarPercent;
	private float HealthBarPercent
	{
		get => _healthBarPercent;
		set
		{
			_healthBarPercent = value;
			var pad = _healthBar.padding;
			pad.z = (-1 * _healthBarPercent * Mathf.Abs(_healthBarMax - _healthBarMin) + _healthBarMin);
			_healthBar.padding = pad;
		}
	}
	
	[Space(10)]
	[Header("Health Properties")]
	public int Health = 250;
	public int MaxHealth = 250;
	
	void Start () 
	{
		anim = GetComponent<Animator> ();
		_healthBar = GameObject.Find("Health Bar Mask").GetComponent<RectMask2D> ();
		HealthBarPercent = Health / MaxHealth;
	}

	void Update()
	{
		HealthBarPercent = ((1.0f * Health) / (1.0f * MaxHealth));
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

	public void ResetHealth()
	{
		Health = MaxHealth;
	}
}
