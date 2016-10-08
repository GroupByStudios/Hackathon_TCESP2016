using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Hackatoon_TCE
{

	public class Cone : MonoBehaviour {

        [HideInInspector]
        public Dictionary<int, Enemy> EnemiesDictionary;

		[HideInInspector]
		public bool Reporting = false;

        void Awake()
        {
            EnemiesDictionary = new Dictionary<int, Enemy>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!EnemiesDictionary.ContainsKey(other.gameObject.GetInstanceID()))
            {
                Enemy enemyPtr = other.gameObject.GetComponent<Enemy>();

                if (enemyPtr != null && !enemyPtr.Reported)
                {
                    EnemiesDictionary.Add(other.gameObject.GetInstanceID(), other.gameObject.GetComponent<Enemy>());
                }
            }
        }

		void OnTriggerStay(Collider other)
		{
            Enemy enemyPtr = other.gameObject.GetComponent<Enemy>();

            if (enemyPtr != null && !enemyPtr.Reported)
            {
                Reporting = true;
            }
		}

        void OnTriggerExit(Collider other)
        {
            if (EnemiesDictionary.ContainsKey(other.gameObject.GetInstanceID()))
            {
                EnemiesDictionary.Remove(other.gameObject.GetInstanceID());
            }
        }

		void LateUpdate()
		{
			Reporting = false;
		}
	}

}
