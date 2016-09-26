using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class CollectPoint : MonoBehaviour {

		public float Range = 10f;

		public void OnDrawGizmos()
		{			
			Gizmos.DrawWireSphere(transform.position, Range);
		}
	}

}
