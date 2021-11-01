using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击状态
/// </summary>
public class Attackstate : FSMState
{
    FSMSystem fSM;
    public float time = 1;
    public bool flag = true;
    public Transform playerTransform;

    public Attackstate(FSMSystem fSM) : base(fSM)
    {
        stateID = StateID.Attack;
        this.fSM = fSM;
        this.playerTransform = GameObject.Find("0").transform;
    }

    /// <summary>
    /// 攻击
    /// </summary>
    /// <param name="npc"></param>
    public override void Act(GameObject npc)
    {
        //Debug.Log("攻击" + (float)100 / World.Ins.m_player.m_info.m_hpMax);
        //Debug.Log(time);
        time -= Time.deltaTime;
        if (flag && time <=0)
        {
            playerTransform.GetComponent<UIPate>().m_hp.value -= (float)20 / World.Ins.m_player.m_info.m_hpMax;
            //攻击 发给服务器
            //Notification notify = new Notification();
            //notify.Refresh("attackMonstertoPlayer");
            //MsgCenter.Ins.SendMsg();
           time = 1;
            if (playerTransform.GetComponent<UIPate>().m_hp.value>=1)
            {
                flag = false;
            }
        }
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="npc"></param>
    public override void Reason(GameObject npc)
    {
        if (Vector3.Distance(playerTransform.position, npc.transform.position) > 1)
        {
            fsm.PerformTransition(Transition.SeePlayer);
            time = 1;
        }
    }
}

