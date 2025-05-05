using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

	private bool _keepHighHealth = true;
	private bool _keepHighHealthOld = true;
	
	[Space(10)]
	[Header("Health Properties")]
	public int Health = 250;
	public int MaxHealth = 250;
	
	void Start () 
	{
		anim = GetComponent<Animator> ();
		_healthBar = GameObject.Find("Health Bar Mask").GetComponent<RectMask2D> ();
		HealthBarPercent = Health / MaxHealth;
		if (HealthBarPercent > 0.5f)
		{
			_keepHighHealth = true;
			_keepHighHealthOld = true;
		}
		else
		{
			_keepHighHealth = false;
			_keepHighHealthOld = false;
		}
	}

	void Update()
	{
		HealthBarPercent = ((1.0f * Health) / (1.0f * MaxHealth));
		if (HealthBarPercent > 0.5f)
		{
			_keepHighHealth = true;
		}
		else
		{
			_keepHighHealth = false;
		}

		if (_keepHighHealth != _keepHighHealthOld)
		{
			_keepHighHealthOld = _keepHighHealth;
			AkSoundEngine.SetState("KeepHealth", _keepHighHealth ? "KeepHighHealth" : "KeepLowHealth");
		}

		if (Health <= 0)
		{
			Destroy(this.gameObject);
		}
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

	private void KeepHighHealth()
	{
		_keepHighHealth = true;
	}

	private void KeepLowHealth()
	{
		_keepHighHealth = false;
	}
}
