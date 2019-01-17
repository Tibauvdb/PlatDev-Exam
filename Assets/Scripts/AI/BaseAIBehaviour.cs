﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Found in AI - Behaviour Tree | Thought Process 
[RequireComponent(typeof(AiFieldOfView))]
[RequireComponent(typeof(NavMeshAgent))]
public class BaseAIBehaviour : MonoBehaviour {

    private INode _rootNode;

    public bool IsAIKnockedOut;
    public bool IsAIFollowing;
    public bool IsAILooking;

    public Vector3 PlayerPosition;

    private float _knockedOutTimer = 10;

    private NavMeshAgent _agent;
    private float _maxRoamDistance = 10f; //10m

    private Animator _anim;
    private AIStateMachine _aiStateMachine;

    public  ConditionNode.Condition AISpecificCondition { get; set; }
    public ActionNode.Action AISpecificAction { get; set; }

    private AiFieldOfView _aiFOV;

	void Start () {
        //Check if the AI is following or looking type
        CheckAIType();

        SetComponents();

        _rootNode =
            new SelectorNode(
                new SequenceNode(
                    new ConditionNode(IsKnockedOut),
                    new ActionNode(KnockedOut),
                    new ActionNode(KnockedOutTimer)),
                new SequenceNode(
                    new ConditionNode(AISpecificCondition),
                    new SelectorNode(
                        new SequenceNode(
                            new ConditionNode(IsPlayerPushingBox),
                            new ActionNode(RunAway)),
                        new ActionNode(AISpecificAction))),
                new ActionNode(Roaming));


        StartCoroutine(RunTree());
	}

    private void Update()
    {
        _anim.SetFloat("HorizontalVelocity", _agent.velocity.z);
        _anim.SetFloat("VerticalVelocity", -_agent.velocity.x);

        CheckNavMeshLink();
    }

    IEnumerator RunTree()
    {
        while (Application.isPlaying)
        {
            yield return _rootNode.Tick();
        }
    }

    bool IsKnockedOut()
    {
        return IsAIKnockedOut;
    }

    IEnumerator<NodeResult> KnockedOut()
    {
        IsAIFollowing = false;
        //Stop AI from following path
        ResetAgent();
        //Play KnockedOut Animation
        _anim.ResetTrigger("IsRespawned");
        _anim.SetTrigger("IsKnockedOut");
        yield return NodeResult.Succes;
    }

    IEnumerator<NodeResult> KnockedOutTimer()
    {
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
        yield return NodeResult.Succes;
    }

    bool IsFollowing()
    {
        IsAIFollowing = _aiFOV.CheckFieldOfView(true);
        return IsAIFollowing;
    }

    IEnumerator<NodeResult> FollowPlayer()
    {
        //Start Following The Player
        Debug.Log("Set Player as destination");
        _agent.SetDestination(PlayerPosition);

        yield return NodeResult.Running;
    }

    bool IsLooking()
    {
        IsAILooking = _aiFOV.CheckFieldOfView(false);
        return IsAILooking;
    }

    IEnumerator<NodeResult> LookAtPlayer()
    {
        yield return NodeResult.Succes;
    }

    bool IsPlayerPushingBox()
    {
        return _aiFOV.CheckBoxPushing();        
    }

    IEnumerator<NodeResult> RunAway()
    {
        _agent.ResetPath();
        Vector3 distToPlayer = this.gameObject.transform.position - PlayerPosition;
        Vector3 targetPos = this.gameObject.transform.position + distToPlayer;
        _agent.SetDestination(targetPos);

        yield return NodeResult.Running;
    }

    IEnumerator<NodeResult> Roaming()
    {
        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            float newDestination = Random.Range(0, 100);

            if (newDestination >= 99) //1% Chance to pick new location
            {
                RandomDestination();
            }

        }
        
        yield return NodeResult.Succes;
    }

    private void RandomDestination()
    {
        //Go To random Spot on NavMesh
        Vector3 dir = this.gameObject.transform.position + (Random.insideUnitSphere * _maxRoamDistance);
        NavMeshHit hit;
        NavMesh.SamplePosition(dir, out hit, Random.Range(0f, _maxRoamDistance), 1);

        Vector3 destination = hit.position;

        _agent.SetDestination(destination);
    }

    public void ResetAgent()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    private void CheckAIType()
    {
        if (this.name.Contains("Type01"))
        {
            AISpecificCondition = IsFollowing;
            AISpecificAction = FollowPlayer;
        }
        else if (this.name.Contains("Type02"))
        {
            AISpecificCondition = IsLooking;
            AISpecificAction = LookAtPlayer;

        }
    }

    private void CheckNavMeshLink()
    {
        if (_agent.isOnOffMeshLink)
            _anim.SetBool("IsJumping", true);
        else
            _anim.SetBool("IsJumping", false);
    }

    private void SetComponents()
    {
        _agent = this.gameObject.GetComponent<NavMeshAgent>();
        _anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        _aiStateMachine = _anim.GetBehaviour<AIStateMachine>();
        _aiStateMachine.AIBehaviour = this;
        _aiFOV = this.gameObject.GetComponent<AiFieldOfView>();
    }

}
