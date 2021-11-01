using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : SingleTon<World>
{
    //实例对象字典
    public Dictionary<int, ObjectBase> m_insDic = new Dictionary<int, ObjectBase>();
   
    public HostPlayer m_player;  //对象
    private GameObject NpcRoot;  //爹
    public Camera m_main;  //相机

    public float xlength;
    public float ylength;

    public Action action;
    public bool cjfalg = false;

    public bool gatherflag = false;

    public int gathernum;  //当前采集数量
    public int gatherAllnum=2;  //采集总数

    public void Init()
    {
#if CWSDK
     CWSdkPubmeth.GetSdk.Init();
#endif
        GameObject plan = GameObject.Find("Plane");

        Vector3 length = plan.GetComponent<MeshFilter>().mesh.bounds.size;  

        //地图尺寸
        xlength = length.x * plan.transform.lossyScale.x;
        ylength = length.z * plan.transform.lossyScale.z;
        Debug.Log("地图尺寸是：x:" + xlength + "   " + "y:" + ylength);

        m_main = GameObject.Find("Main Camera").GetComponent<Camera>();
        NpcRoot = GameObject.Find("NPC_Root");
        UIMgr.Ins.Init(GameObject.Find("UIRoot"), GameObject.Find("HUD"));

        player_info info = new player_info();
        info.ID = 0;
        info.m_name = "tony";
        info.m_level = 9;
        info.m_pos = Vector3.zero;
        info.m_res = "Teddy";
        info.m_HP = 2000;
        info.m_MP = 1000;
        info.m_hpMax = 2000;
        info.m_mpMax = 2000;

        m_player = new HostPlayer(info);
        m_player.CreateObj(MonsterType.Null);

        JoyStickMgr.Ins.SetJoyArg(m_main, m_player);
        JoyStickMgr.Ins.JoyActive = true;


        CreateIns();
    }

    private void CreateIns()
    {
        //获得解析到的json 数据
        JsonData data = MonsterCfg.Ins.GetJsonData();
        object_info info;

        for (int i = 0; i < data.datas.Count; i++)
        {
            info = new object_info();
            info.ID = m_insDic.Count + 1;
            info.m_name = string.Format(data.datas[i].name, info.ID);
            info.m_res = data.datas[i].name;
            info.m_pos = new Vector3(data.datas[i].x, data.datas[i].y, data.datas[i].z);
            info.m_type = data.datas[i].type;
            CreateObj(info);
        }
    }

    ObjectBase monster = null;


    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="info"></param>
    private void CreateObj(object_info info)
    {
        monster = null;
        if (info!=null)
        {
            switch (info.m_type)
            {
                case MonsterType.Normal:
                    monster = new Normal(info);
                    break;
                case MonsterType.Gather:
                    monster = new Gather(info);
                    break;
                case MonsterType.NPC:
                    monster = new NpcObj(1, info);
                    break;
                default:
                    break;
            }
        }
        if (monster!=null)
        {
            monster.CreateObj(info.m_type);
            monster.m_go.transform.SetParent(NpcRoot.transform,false);
            m_insDic.Add(info.ID, monster);
        }
        else
        {
            Debug.Log("生成失败！！！");
        }
    }

    /// <summary>
    /// ???????????????????
    /// </summary>
    /// <param name="target"></param>
    /// <param name="pos"></param>
    public void AutoMoveByInsId(int target,Vector3 pos)
    {
        using (var tmp = m_insDic.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (target == tmp.Current.Key)
                {
                    //TODO  让实例移动
                    tmp.Current.Value.AutoMove(pos, pos);
                }
            }
        }
    }
}
