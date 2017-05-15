using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontAxle : MonoBehaviour
{

	[Range(0f, 90f)]
	public float MaxRotation = 45f;

	public float RotationTime = 1.0f;

	private Transform _leftTire;
	private Transform _rightTire;

	private bool _rotate = false;
	
	private float _currentAnimationTime = 0f;

	private float _startingAngle = 0f;

	private float _currentAngle = 0f;

    private enum RotationDirection 
    {
        LEFT,
        RIGHT,
        RESET
    }

    private RotationDirection _rotationDirection = RotationDirection.RESET;

	void Start()
	{
		_leftTire = transform.Find("LeftTire").transform;
		_rightTire = transform.Find("RightTire").transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (_rotate)
		{
			var maxAngle = MaxRotation;

            if (_rotationDirection == RotationDirection.RIGHT)
			{
				maxAngle = -MaxRotation;
			}

            if(_rotationDirection == RotationDirection.RESET)
            {
                maxAngle = 0f;
            }

			float currentTime = (_currentAnimationTime / RotationTime);			
            float currentRotation = Mathf.Lerp(_startingAngle, maxAngle, currentTime);

			_currentAngle = currentRotation;
			_leftTire.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
			_rightTire.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);

			_currentAnimationTime += Time.deltaTime;

            if (_currentAnimationTime >= RotationTime)
            {
                _rotate = false;
            }
		}

		DrawDebugLine(_leftTire);
		DrawDebugLine(_rightTire);

	}

	public void StartRotationRight()
	{
        if (_rotationDirection != RotationDirection.RIGHT)
        {
            _rotate = true;
            _rotationDirection = RotationDirection.RIGHT;
            _startingAngle = _currentAngle;
            _currentAnimationTime = 0f;
        }
	}

	public void StartRotationLeft()
	{
        if(_rotationDirection != RotationDirection.LEFT)
        {
			_rotate = true;
			_rotationDirection = RotationDirection.LEFT;
			_startingAngle = _currentAngle;
			_currentAnimationTime = 0f;    
        }		
	}

    public void ResetRotation() 
    {
        if ((!Mathf.Approximately(_currentAngle, 0f)) && 
            (_rotationDirection != RotationDirection.RESET))
        {
			_rotate = true;
            _rotationDirection = RotationDirection.RESET;
			_startingAngle = _currentAngle;
            _currentAnimationTime = 0f;
		}
    }

	public void DrawDebugLine(Transform tire)
	{
		var initialPosition = tire.transform.position;
		var tireRotation = (tire.transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
		//Debug.Log(tire.transform.rotation.eulerAngles);

		var direction = new Vector3(Mathf.Cos(tireRotation), Mathf.Sin(tireRotation), 0f).normalized;
		var directionPerp = new Vector3(direction.y, -direction.x, 0f);		

		if(_rotationDirection == RotationDirection.LEFT)
		{
			directionPerp *= -1;
		}

		Debug.DrawLine(initialPosition, initialPosition + directionPerp * 5);
	}
}
