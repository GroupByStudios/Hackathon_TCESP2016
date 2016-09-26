using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class SineWave : MonoBehaviour {

		public float amplitudeX = 10.0f;
		public float amplitudeY = 5.0f;
		public float omegaX = 1.0f;
		public float omegaY = 5.0f;
		public float index;

		public float RotationSpeed = 5f;

		// Update is called once per frame
		void Update () {

			index += Time.deltaTime;
			float x = amplitudeX * Mathf.Cos (omegaX*index);
			float y = amplitudeY*Mathf.Sin (omegaY*index);
			transform.localPosition= new Vector3(transform.localPosition.x,y,transform.localPosition.z);

			transform.Rotate(new Vector3(0, RotationSpeed * Time.deltaTime, 0));
		}
	}

}
