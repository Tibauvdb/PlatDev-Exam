using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAIBehaviour : MonoBehaviour {
    private INode _rootNode;

    public bool IsAIKnockedOut;
    public bool IsAIFollowing;
    public Vector3 PlayerPosition;
    private float _knockedOutTimer = 10;

    private NavMeshAgent _agent;
    private float _maxRoamDistance = 10f; //10m

    private Animator _anim;
    private AIStateMachine _aiStateMachine;
	// Use this for initialization
	void Start () {
        _agent = this.gameObject.GetComponent<NavMeshAgent>();
        _anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        _aiStateMachine = _anim.GetBehaviour<AIStateMachine>();
        _aiStateMachine.AIBehaviour = this;

        _rootNode =
            new SelectorNode(
                new SequenceNode(
                    new ConditionNode(IsKnockedOut),
                    new ActionNode(KnockedOut),
                    new ActionNode(KnockedOutTimer)),
                new SequenceNode(
                    new ConditionNode(IsFollowing),
                    new ActionNode(FollowPlayer)),
                new ActionNode(Roaming));

        StartCoroutine(RunTree());
	}

    private void Update()
    {
        _anim.SetFloat("HorizontalVelocity", -_agent.velocity.z * this.gameObject.transform.forward.z);
        _anim.SetFloat("VerticalVelocity", _agent.velocity.x * this.gameObject.transform.forward.x);
    }
    // Update is called once per frame
    IEnumerator RunTree()
    {
        while (Application.isPlaying)
        {
            yield return _rootNode.Tick();
        }
    }

    bool IsKnockedOut()
    {
        Debug.Log("IsKnockedOut" + IsAIKnockedOut);
        return IsAIKnockedOut;
    }

    IEnumerator<NodeResult> KnockedOut()
    {
        IsAIFollowing = false;
        //Stop AI from following path
        ResetAgent();
        Debug.Log("KnockedOut | Playing Animation");
        //Play KnockedOut Animation
        _anim.ResetTrigger("IsRespawned");
        _anim.SetTrigger("IsKnockedOut");
        yield return NodeResult.Succes;
    }

    IEnumerator<NodeResult> KnockedOutTimer()
    {
        //Debug.Log("KnockedOutTimer" + _knockedOutTimer);
        _knockedOutTimer -= Time.deltaTime ;
        
        if (_knockedOutTimer < 0)
        {
            //KnockedOutState Is Over
            IsAIKnockedOut = false;
            _knockedOutTimer = 10; //Reset Timer

            //Reset Animation
            _anim.ResetTrigger("IsKnockedOut");
            _anim.SetTrigger("IsRespawned");
            yield return NodeResult.Failure;
        }
        Debug.Log("Returning Running");
        yield return NodeResult.Succes;
    }

    bool IsFollowing()
    {
        Debug.Log("IsFollowing" + IsAIFollowing);
        return IsAIFollowing; 
    }

    IEnumerator<NodeResult> FollowPlayer()
    {
        //Start Following The Player
        Debug.Log("Following Player");
        _agent.SetDestination(PlayerPosition);
        yield return NodeResult.Running;
    }

    IEnumerator<NodeResult> Roaming()
    {
        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {

            float newDestination = Random.Range(0, 100);
            //Debug.Log("Roaming" + newDestination);

            if (newDestination >= 99)
            {
                //Go To random Spot on NavMesh
                Vector3 dir = Random.insideUnitSphere * _maxRoamDistance;
                NavMeshHit hit;
                NavMesh.SamplePosition(dir, out hit, Random.Range(0f, _maxRoamDistance), 1);

                Vector3 destination = hit.position;

                _agent.SetDestination(destination);
            }

        }
        
        yield return NodeResult.Succes;
    }

    public void ResetAgent()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }
}
