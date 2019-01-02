using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAIBehaviour : MonoBehaviour {
    private INode _rootNode;

    public bool IsAIKnockedOut;
    public bool IsAIFollowing;
    private float _knockedOutTimer = 10;

    private NavMeshAgent _agent;
    private float _maxRoamDistance = 10f; //10m
	// Use this for initialization
	void Start () {
        _agent = this.gameObject.GetComponent<NavMeshAgent>();


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
        _agent.isStopped = true;
        _agent.ResetPath();
        Debug.Log("KnockedOut | Playing Animation");
        //Play KnockedOut Animation

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
        yield return NodeResult.Running;
    }

    IEnumerator<NodeResult> Roaming()
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

        
        yield return NodeResult.Succes;
    }


}
