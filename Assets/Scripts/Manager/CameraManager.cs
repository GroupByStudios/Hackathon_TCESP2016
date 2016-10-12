using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class CameraManager : MonoBehaviour {

      
        
		public GameObject Player;
		public float CameraLerp;
		public float CameraBoom;

		public void Update()
		{

			if (Player != null)
			{
				this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z + CameraBoom), CameraLerp);
			}

		}


	}

}
