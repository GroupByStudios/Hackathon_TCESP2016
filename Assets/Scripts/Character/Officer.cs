using UnityEngine;
using System.Collections;

namespace Hackatoon_TCE
{

    public class Officer : MonoBehaviour
    {
        public OfficerState State;
        public Vector3? Target;
        public float MinDistance;

        public Animator animatorController;
        public NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            State = OfficerState.DontHaveTarget;
        }

        void Update()
        {
            if (animatorController != null)
                animatorController.SetFloat("Speed_f", navMeshAgent.velocity.magnitude);

            switch (State)
            {
                case OfficerState.DontHaveTarget:

                    if (Target.HasValue)
                        State = OfficerState.HasTarget;

                    break;

                case OfficerState.HasTarget:


                    State = OfficerState.Moving;
                    navMeshAgent.SetDestination(Target.Value);

                    break;
                case OfficerState.Moving:

                    Vector3 distanceFromTarget = Target.Value - transform.position;

                    if (distanceFromTarget.magnitude <= MinDistance)
                    {
                        State = OfficerState.FinishMove;
                        navMeshAgent.Stop();
                        navMeshAgent.velocity = Vector3.zero;
                    }
                    
                    break;
                case OfficerState.FinishMove:

                    transform.rotation = Quaternion.Euler(0, 180, 0);

                    break;
            }

        }

    }

    public enum OfficerState
    {
        DontHaveTarget,
        HasTarget,
        Moving,
        FinishMove

    }

}

