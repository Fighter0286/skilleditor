using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载的脚本  
/// </summary>
public class Enemy : MonoBehaviour
{
    private FSMSystem fsm;
    private UIPate pate = null;

    // Start is called before the first frame update
    void Start()
    {
        pate = gameObject.GetComponent<UIPate>();
        InitFSM();
        MsgCenter.Ins.AddListener("MosterHp",SHpCallback);
    }

    private void SHpCallback(Notification notify)
    {
        Debug.Log((int)notify.data[0]);
        Debug.Log((float)notify.data[1]);

        pate.m_hp.value -= (int)notify.data[0] / (float)notify.data[1];
        Debug.Log(pate.m_hp.value);
    }

    private void InitFSM()
    {
        fsm = new FSMSystem();

        FSMState patrolState = new PatrolState(fsm);  //巡逻
        patrolState.AddTransition(Transition.SeePlayer,StateID.Chase);

        FSMState chaseState = new ChaseState(fsm);  //追击
        chaseState.AddTransition(Transition.LostPlayer, StateID.Patrol);
        chaseState.AddTransition(Transition.AttackPlayer, StateID.Attack);

        FSMState attackState = new Attackstate(fsm);
        attackState.AddTransition(Transition.SeePlayer, StateID.Chase);

        fsm.AddState(patrolState);
        fsm.AddState(chaseState);
        fsm.AddState(attackState);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.Updata(this.gameObject);
    }
}
