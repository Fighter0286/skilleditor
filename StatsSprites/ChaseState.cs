using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 追击状态
/// </summary>
public class ChaseState : FSMState
{
    private Transform playerTransform;

    public ChaseState(FSMSystem fsm):base(fsm)
    {
        stateID = StateID.Chase;
        this.playerTransform = GameObject.Find("0").transform;
    }

    /// <summary>
    /// 设置朝向和移动
    /// </summary>
    /// <param name="npc"></param>
    public override void Act(GameObject npc)
    {
        //Debug.Log("追击");
        npc.transform.LookAt(playerTransform.position);
        npc.transform.Translate(Vector3.forward*2*Time.deltaTime);
    }

    /// <summary>
    /// //判断距离  条件检查
    /// </summary>
    /// <param name="npc"></param>
    public override void Reason(GameObject npc)
    {
        
        if (Vector3.Distance(playerTransform.position,npc.transform.position)>4)
        {
            //失去玩家的逻辑  切换状态
            fsm.PerformTransition(Transition.LostPlayer);
        }
        if (Vector3.Distance(playerTransform.position, npc.transform.position) < 0.5)
        {
            fsm.PerformTransition(Transition.AttackPlayer);
        }
    }
}
