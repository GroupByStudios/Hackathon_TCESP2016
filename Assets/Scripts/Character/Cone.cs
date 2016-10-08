using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class Cone : MonoBehaviour {

		[HideInInspector]
		public bool Reporting = false;

		void OnTriggerStay(Collider other)
		{
			Reporting = true;
		}

		void LateUpdate()
		{
			Reporting = false;
		}
	}

}
