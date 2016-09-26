using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{
	
	public class MinimapManager : MonoBehaviour {

		Player player;

		// Use this for initialization
		void Start () {
		
			player = GameObject.FindObjectOfType<Player>();

		}
		
		// Update is called once per frame
		void Update () {

			if (player != null)
			{
				transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
			}
		}
	}

}
