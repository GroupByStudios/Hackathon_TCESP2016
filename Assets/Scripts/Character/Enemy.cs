using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

	public class Enemy : MonoBehaviour {

		public int AvatarIndex;
		public GameObject[] AvatarList;
		public Transform SpawnPoint;

		public bool AnimationStatic = false;
		public Animator AnimatorController;

		public float Speed = 5f;
		public float AnimationSpeedMultiplier = 1;

		const string ANIM_SPEED = "Speed_f";
		const string ANIM_SPEED_MULTIPLIER = "SpeedMultiplier_f";
		const string ANIM_JUMP = "Jump_b";
		const string ANIM_STATIC = "Static_b";

		void Awke()
		{
			if (SpawnPoint != null)
			{
				transform.position = SpawnPoint.position;
				transform.rotation = SpawnPoint.rotation;
			}
		}


		// Use this for initialization
		void Start () {
		
			if (AvatarList != null)
			{				
				AvatarIndex = Random.Range(0, AvatarList.Length);

				// Palhaco esta bugado
				while(AvatarIndex == 5 || AvatarIndex == 10)
				{
					AvatarIndex = Random.Range(0, AvatarList.Length);
				}
				
				AvatarList[AvatarIndex].SetActive(true);
			}

		}
		
		// Update is called once per frame
		void Update () {
		

			transform.Translate(Vector3.forward * Speed * Time.deltaTime);

			if (AnimatorController != null)
			{
				AnimatorController.SetFloat(ANIM_SPEED_MULTIPLIER, AnimationSpeedMultiplier);
				//AnimatorController.SetBool(ANIM_STATIC, AnimationStatic);
				//AnimatorController.SetFloat(ANIM_SPEED, 1f);
			}
		}
	}

}
