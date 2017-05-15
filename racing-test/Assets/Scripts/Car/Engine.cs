using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {
	
	[SerializeField]
	int[] TorqueCurve = new int[4] { 100, 280, 300, 100 };

	[SerializeField]
	float[] GearRatios = new float[] { 4.8f, 3.5f, 0.79f, 3.2f };

	public int CurrentGear { get; private set; }

	public float GearRatio {
		get { return GearRatios[CurrentGear]; }
	}

	public float EffectiveGearRatio {
		get { return GearRatios[GearRatios.GetLength(0) - 1]; }
	}

	public void ShiftUp() {
		CurrentGear++;
	}

	public void ShiftDown() {
		CurrentGear++;
	}

	public float GetTorque(Rigidbody2D rb) {
		return GetTorque(GetRPM (rb));
	}

	public float GetRPM(Rigidbody2D rb) {
		return rb.velocity.magnitude / (Mathf.PI * 2 / 60f) * (GearRatio * EffectiveGearRatio);
	}

	public float GetTorque(float rpm)
	{		
		if (rpm < 1000) {			
			return Mathf.Lerp (TorqueCurve [0], TorqueCurve [1], rpm / 1000f);
		} else if (rpm < 2000) {
			return Mathf.Lerp (TorqueCurve [1], TorqueCurve [2], (rpm - 1000) / 1000f);
		} else if (rpm < 3000) {
			return Mathf.Lerp (TorqueCurve [2], TorqueCurve [3], (rpm - 2000) / 1000f);
		} else {			
			return TorqueCurve [3];
		}

	}

	public void UpdateAutomaticTransmission(Rigidbody2D rb) {
		float rpm = GetRPM (rb);

		if (rpm > 300) {
			if (CurrentGear < 3)
				CurrentGear++;
		} else if (rpm < 2000) {
			if (CurrentGear > 0)
				CurrentGear--;
		}
	}


}
