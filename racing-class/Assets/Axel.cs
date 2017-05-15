using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axle : MonoBehaviour {

	[Range(0f, 90f)]
	public float MaxRotation = 45f;

	public float RotationTime = 15f;

	private Transform _leftTire;
	private Transform _rightTire;

	private bool _rotate = false;
	private bool _rotateRight = false;

	private float _currentAnimationTime = 0f;

	private float _startingAngle = 0f;

	private float _currentAngle = 0f;

	void Start () 
	{
		_leftTire = transform.Find("LeftTire").transform;	
		_rightTire = transform.Find("RightTire").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_rotate && (Mathf.Abs(_currentAngle) < MaxRotation))
		{
			var maxAngle = -MaxRotation;

			if(_rotateRight)
			{
				maxAngle = MaxRotation;
			}

			float currentRotation = 0f;
			float currentTime = (_currentAnimationTime/RotationTime);
			Debug.Log(currentTime);
			if(_startingAngle <= maxAngle)
			{
				currentRotation = Mathf.Lerp(_startingAngle, maxAngle, currentTime);
			}
			else
			{
				currentRotation = Mathf.Lerp(maxAngle, _startingAngle, currentTime);
			}

			_currentAngle = currentRotation;
			_leftTire.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
			_rightTire.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);

			_currentAnimationTime += Time.deltaTime;
		}	
	}

	public void StartRotationRight() 
	{
		_rotate = true;
		_rotateRight = true;
		_startingAngle = _currentAngle;
		_currentAnimationTime = 0f;
	}

	public void StartRotationLeft() 
	{
		_rotate = true;
		_rotateRight = false;
		_startingAngle = _currentAngle;
		_currentAnimationTime = 0f;
	}
}
