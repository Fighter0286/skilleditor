using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition=0,
    SeePlayer,  //巡逻
    LostPlayer,  //看见玩家
    AttackPlayer  //攻击玩家
}

public enum StateID
{
    NullStateID=0,
    Patrol,  //巡逻
    Chase,  //追击
    Attack
}

/// <summary>
/// 状态机的基类  有库，持有一系列状态
/// </summary>
public abstract class FSMState
{
    protected StateID stateID;
    public StateID ID { get { return stateID; } }

    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected FSMSystem fsm;

    public FSMState(FSMSystem fSM)
    {
        this.fsm = fSM;
    }


    public void AddTransition(Transition trans,StateID id)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("不允许NullTransition");
            return;
        }
        if (id == StateID.NullStateID)
        {
            Debug.LogError("不允许NullStateID");
            return;
        }
        if (map.ContainsKey(trans))
        {
            Debug.LogError("添加转换条件的时候，"+trans+"已经存才与map字典中");
            return;
        }
        map.Add(trans, id);
    }

    public void DeleteTransiton(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("不允许NullTransition"); 
            return;
        }
        if (map.ContainsKey(trans) == false)
        {
            Debug.LogError("删除转换条件的时候，" + trans + "不存在于map中");
            return;
        }
        map.Remove(trans);
    }

    public StateID GetOutputState(Transition trans)
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }
        return StateID.NullStateID;
    }
    public virtual void DoBeforeEntering() { }  //进入前  技能前摇和后摇
    public virtual void DoAfterLeaving() { }  //离开后  
    public abstract void Act(GameObject npc);  //当前动作  
    public abstract void Reason(GameObject npc);  //当前状态需要进行的操作
}
