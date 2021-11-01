using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理所有的状态  持有当前状态 负责执行相应的逻辑
/// </summary>
public class FSMSystem 
{
    private Dictionary<StateID, FSMState> states = new Dictionary<StateID, FSMState>();

    private StateID currentStateID;  //当前状态ID
    private FSMState currentState;  //当前状态

    public void Updata(GameObject npc)
    {
        currentState.Act(npc);  //当前状态动作
        currentState.Reason(npc);  //当前条件检查
    }
    /// <summary>
    /// 添加的方法
    /// </summary>
    /// <param name="s"></param>
    public void AddState(FSMState s)
    {
        if (s==null)
        {
            Debug.LogError("FSMState不能为空");
            return;
        }
        if (currentState == null)
        {
            currentState = s;
            currentStateID = s.ID;
        }
        if (states.ContainsKey(s.ID))
        {
            Debug.LogError("状态" + s.ID + "已经存在，无法重复添加"); 
            return;
        }
        states.Add(s.ID, s);
    }

    /// <summary>
    /// 删除的方法
    /// </summary>
    /// <param name="id"></param>
    public void DeleteState(StateID id)
    {
        if (id==StateID.NullStateID)
        {
            Debug.LogError("无法删除空状态");
            return;
        }
        if (states.ContainsKey(id) == false)
        {
            Debug.LogError("无法删除不存在的状态：" + id);
            return;
        }
        states.Remove(id);
    }

    /// <summary>
    /// 切换状态  判断状态不是空  
    /// </summary>
    /// <param name="lostPlayer"></param>
    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("无法执行空的转换条件"); 
            return;
        }
        StateID id = currentState.GetOutputState(trans);  //通过ID获取当前状态可以取到那个状态
        if (id == StateID.NullStateID)
        {
            Debug.LogWarning("当前状态" + currentStateID + "无法根据转换条件" + trans + "发生转换"); 
            return;
        }
        if (states.ContainsKey(id) == false)
        {
            Debug.LogError("在状态机里面不存在状态" + id + "，无法进行状态转换！");
            return;
        }
        FSMState state = states[id];
        currentState.DoAfterLeaving();
        currentState = state;
        currentStateID = id;
        currentState.DoBeforeEntering();
    }
}
