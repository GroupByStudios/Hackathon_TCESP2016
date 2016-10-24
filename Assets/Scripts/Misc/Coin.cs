using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

    [RequireComponent(typeof(Rigidbody))]
    public class Coin : MonoBehaviour
    {

        AudioSource audio = null;

        const string PLAYER_TAG = "Player";

        public float RotationSpeed = 135f;
        public float UpForce = 500f;
        public float MinSideWayForce = -100f;
        public float MaxSideWayForce = 100f;
        public float DestroyInTime = 5;
        
        Rigidbody rigidBody;

        // Use this for initialization
        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.AddForce(new Vector3(Random.Range(MinSideWayForce, MaxSideWayForce), UpForce, Random.Range(MinSideWayForce, MaxSideWayForce)));
            Destroy(gameObject, DestroyInTime);
        }

        bool play = false;

        // Update is called once per frame
        void Update()
        {
            if (audio == null)
            {

                audio = GetComponent<AudioSource>();

            }

			transform.Rotate(0, 90 * Time.deltaTime, 0);

        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(PLAYER_TAG))
            {
                Player player = collision.gameObject.GetComponent<Player>();

                if (player != null)
                {
                    if (audio != null && !play)
                    {
                        play = true;
                        //audio.Play();
                    }

                    player.GiveCoin();
					Destroy(this.gameObject);
                }
            }
        }
    }

}
