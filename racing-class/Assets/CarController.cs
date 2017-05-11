using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	[Range(50f, 1000f)]
	public float DefaultEngineForce = 100f;

	public float RollingResistance = 12.8f;

	public float BrakingForce = 200f;

	private bool _accelerating = false;

	private bool _braking = false;

	private Rigidbody2D _rigidBody;

	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Detecting input key
		_accelerating = false;
		_braking = false;
		if(Input.GetKey(KeyCode.W))
		{
			_accelerating = true;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			_braking = true;
		}
	}

	void FixedUpdate() 
	{
		Vector2 totalForces = CalculateForces();
		Vector2 accelartion = totalForces / _rigidBody.mass;
			
		var velocity = _rigidBody.velocity + Time.fixedDeltaTime * accelartion;	
		_rigidBody.velocity = velocity;
	}

	private Vector2 GetCarDirection() 
	{
		return new Vector2(0.0f, 1.0f);
	}

	private float GetEngineForce()
	{
		if(_accelerating)
		{
			return DefaultEngineForce;
		}

		return 0;
	}

	private float GetBrakingForce()
	{
		if(_braking)
		{
			return BrakingForce;
		}

		return 0;
	}

	private Vector2 CalculateForces() 
	{
		Vector2 longForce = new Vector2(0.0f, 0.0f);

		var carDirection = GetCarDirection();

		Vector2 tractionForce = carDirection * GetEngineForce();
		Vector2 currentVelocity = _rigidBody.velocity;
		Vector2 dragForce = -_rigidBody.drag * currentVelocity *  currentVelocity.magnitude;
		Vector2 rollingResistance = -RollingResistance * currentVelocity;
		Vector2 brakingForce = -GetBrakingForce() * carDirection;
		
		longForce = tractionForce + dragForce + rollingResistance + brakingForce;

		return longForce;
	}
}
