using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巡逻状态
/// </summary>
public class PatrolState : FSMState
{
    private List<Transform> path = new List<Transform>();
    private int index = 0;
    private Transform playerTransform;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="fsm"></param>
    public PatrolState(FSMSystem fsm):base(fsm)
    {
        stateID = StateID.Patrol;
        Transform pathTransform = GameObject.Find("Path").transform;
        Transform[] children = pathTransform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child != pathTransform)
            {
                path.Add(child);
            }
        }
        playerTransform = GameObject.Find("0").transform;
    }

    public override void Act(GameObject npc)
    {
       // Debug.Log("巡逻");
        npc.transform.LookAt(path[index].position);
        npc.transform.Translate(Vector3.forward * Time.deltaTime);
        if (Vector3.Distance(npc.transform.position,path[index].position)<1)
        {
            index++;
            index %= path.Count;
        }
    }
    public override void Reason(GameObject npc)
    {
        if (Vector3.Distance(playerTransform.position,npc.transform.position)<3)
        {
            fsm.PerformTransition(Transition.SeePlayer);
        }
        if (Vector3.Distance(playerTransform.position, npc.transform.position) < 0.5)
        {
            fsm.PerformTransition(Transition.AttackPlayer);
        }
    }
}
