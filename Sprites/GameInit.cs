using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject[] DontDestory;//不销毁的物体

    public List<ETCButton> Attack;  //攻击按钮

    public ETCJoystick joystick;  //遥感

    public GameObject uiroot;


    // Start is called before the first frame update
    void Start()
    {
#if CWSDK
        GameObject skdcallback = new GameObject("PhotoCallback");
        skdcallback.AddComponent<CWSdkCallBack>();
#endif
        //每个物体不销毁
        for (int i = 0; i < DontDestory.Length; i++)
        {
            GameObject.DontDestroyOnLoad(DontDestory[i]);
        }
        GameSceneUtils.LoadSceneAsync("Lobby",()=> {
            JoyStickMgr.Ins.m_joyGO = DontDestory[0];
            JoyStickMgr.Ins.m_joystick = joystick;
            JoyStickMgr.Ins.m_skillBtn = Attack;

            //解析配置表数据
            GameData.Ins.InitByRoleName("Teddy");
            //任务配置表解析
            GameData.Ins.InitTaskData();

            World.Ins.Init();
        });
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
