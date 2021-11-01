using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBase
{
    public GameObject m_go;  //存储当前物体
    public Vector3 m_local_pos;  //当前物体在本地的位置
    public Animator m_anim;
    public UIPate m_pate;  //血条脚本
    public MonsterType m_type;  

    public int m_insID;
    public string m_modelPath;  //模型路径

    public ObjectBase()
    {
           
    }

    /// <summary>
    /// 创建物体的方法
    /// </summary>
    public virtual void CreateObj(MonsterType type)
    {
        m_type = type;
        if (!string.IsNullOrEmpty(m_modelPath) && m_insID >=0 )
        {
            m_go = (GameObject)GameObject.Instantiate(Resources.Load(m_modelPath));
            m_go.name = m_insID.ToString();
            m_go.transform.position = m_local_pos;
            if (m_go)
            {
                OnCreate();
            }
        }
    }

    /// <summary>
    /// 在创建的时候初始化的逻辑
    /// </summary>
    public virtual void OnCreate()
    {
       
    }

    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="pos"></param>
    public virtual void SetPos(Vector3 pos)
    {
        m_local_pos = pos;
    }

    /// <summary>
    /// 移动
    /// </summary>
    public void MoveByTranslate(Vector3 look,Vector3 move)
    {
        //设置朝向
        m_go.transform.LookAt(look);  // 移动角色或物体的位置（按其所朝向的位置移动）
        m_go.transform.Translate(move);  //移动
    }

    /// <summary>
    /// 自定义
    /// </summary>
    public void AutoMove(Vector3 look,Vector3 move)
    {
        MoveByTranslate(look, move);
    }

    /// <summary>
    /// 销毁物体  清空变量
    /// </summary>
    public virtual void Destory()
    {
        //if (m_pate)
        //{
        //    GameObject.Destroy(m_pate);
        //}
        GameObject.Destroy(m_go);
        m_local_pos = Vector3.zero;
        m_anim = null;
        m_insID = -1;
    }
}

/// <summary>
/// 类型枚举
/// </summary>
public enum MonsterType
{
    Null = 0,
    Normal, //怪物
    Gather, //采集物
    Biaoche, //跟随物
    NPC,
}
/// <summary>
/// 怪物基类
/// </summary>
public class Monster : ObjectBase
{
    public monster_info m_info;

    public Monster(MonsterType type, monster_info info)
    {
        info.m_type = type;
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public override void OnCreate()
    {
        base.OnCreate();
    }
}

/// <summary>
/// 正常怪物
/// </summary>
public class Normal : Monster
{
    public Normal(monster_info info) : base(MonsterType.Normal, info)
    {

    }
    public Normal(object_info info) : base(MonsterType.Normal, new monster_info(MonsterType.Normal, info))
    {

    }
    public override void CreateObj(MonsterType type)
    {
        base.CreateObj(type);
    }
    public override void OnCreate()
    {

        base.OnCreate();
        m_go.AddComponent<Enemy>();
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();

        m_pate.m_name.gameObject.SetActive(true);
        m_pate.m_hp.gameObject.SetActive(true);
        m_pate.m_mp.gameObject.SetActive(true);
        m_pate.m_gather.gameObject.SetActive(false);
    }
}

/// <summary>
/// 采集物
/// </summary>
public class Gather : Monster
{
    public Gather(monster_info info) : base(MonsterType.Gather, info)
    {
    }
    public Gather(object_info info) : base(MonsterType.Gather, new monster_info(MonsterType.Gather, info))
    {
    }
    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
    }

    public override void OnCreate()
    {
        base.OnCreate();
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();  //范围检测脚本
        check.m_taget = World.Ins.m_player.m_go;
        check.m_call = (isenter) =>
        {
            if (World.Ins.gatherflag)
            {
                if (isenter)
                {
                    //Debug.Log("可以采集了" + m_info.m_res);
                    Notification notify = new Notification();
                    notify.Refresh("gather_trigger", m_info.ID);
                    MsgCenter.Ins.SendMsg("ClientMsg", notify);
                }
                else
                {
                    Notification notify = new Notification();
                    notify.Refresh("gather_Untrigger", m_info.ID);
                    MsgCenter.Ins.SendMsg("Close", notify);
                }
            }
        };

        MsgCenter.Ins.AddListener("ServerMsg", ServerNotify);

        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();

        m_pate.m_name.gameObject.SetActive(false);
        m_pate.m_hp.gameObject.SetActive(false);
        m_pate.m_mp.gameObject.SetActive(false);
        m_pate.m_gather.gameObject.SetActive(true);
    }

    /// <summary>
    /// 服务器采集回调
    /// </summary>
    /// <param name="obj"></param>
    private void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_callback"))
        {
            int insID = (int)obj.data[0];
            //这里可以判断一下是不是当前采集的物品
            m_pate.SetData((int)obj.data[1]);
        }
    }

    //private void ServerNotify(Notification obj)
    //{

    //}

    public void RefreshGatherCount(int cnt)
    {

    }
}

public class Biaoche : Monster
{
    public Biaoche(monster_info info) : base(MonsterType.Biaoche, info)
    {
    }
    public Biaoche(object_info info) : base(MonsterType.Biaoche, new monster_info(MonsterType.Biaoche, info))
    {
    }
    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
    }
    public override void OnCreate()
    {
        base.OnCreate();
    }
}

/// <summary>
/// 玩家基类
/// </summary>
public class PlayerObj : ObjectBase
{
    public player_info m_info;

    public PlayerObj(player_info info)
    {
        m_info = info;
    }
    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }
    public void SetPos(Vector3 pos, float speed)
    {
        //平滑移动
    }

    public override void OnCreate()
    {
        base.OnCreate();
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.m_gather.SetActive(false);
        m_pate.SetData(m_info.m_name, m_info.m_HP / m_info.m_hpMax, m_info.m_MP / m_info.m_mpMax);
    }

    public void AddBuff(string path)
    {
        //TODO
    }
}
public class HostPlayer : PlayerObj
{
    Player player;
    public HostPlayer(player_info info) : base(info)
    {
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }
    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
    }

    public override void OnCreate()
    {
        base.OnCreate();
        if (m_go.GetComponent<Player>() == null)
        {
            player = m_go.AddComponent<Player>();
        }
        else
        {
            player = m_go.GetComponent<Player>();
        }

        player.InitData();
    }

    /// <summary>
    /// 按钮回调
    /// </summary>
    /// <param name="axisValue1"></param>
    /// <param name="axisValue2"></param>
    public void JoystickHandlerMoving(float h, float v)
    {
        if (Mathf.Abs(h) > 0.05f || (Mathf.Abs(v) > 0.05f))
        {
            //调用基类的移动方法
            MoveByTranslate(new Vector3(m_go.transform.position.x + h, m_go.transform.position.y, m_go.transform.position.z + v), Vector3.forward * Time.deltaTime * 5);
            //notify.Refresh("Player", m_go.transform.position);

        }
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="name"></param>
    public void JoyButtonHandler(string btnName)
    {
        List<SkillBase> componentList;
        switch (btnName)
        {
            case "skill1":
                player.SetData("2");
                player.play();
                break;
            case "skill2":
                player.SetData("3");
                player.play();
                break;
            case "skill3":
                player.SetData("4");
                player.play();
                break;
            case "skill4":
                player.SetData("5");
                player.play();
                break;
            case "attack":
                player.SetData("1");
                player.play();
                break;
        }
    }
}

/// <summary>
/// NPC基类
/// </summary>
public class NpcObj : ObjectBase
{
    public npc_info m_info;

    public NpcObj(npc_info info)
    {
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }
    public NpcObj(int plot, object_info info)
    {
        m_info = new npc_info(plot, info);
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
    }

    public override void OnCreate()
    {
        base.OnCreate();
        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();  //范围检测脚本
        check.m_taget = World.Ins.m_player.m_go;
        check.m_call = (isenter) =>
        {
            if (isenter)
            {
                //Debug.Log("NPC" + m_info.m_res);
                Notification notify = new Notification();
                notify.Refresh("NPC_speck", m_info.ID);
                MsgCenter.Ins.SendMsg("SpeckToNpc", notify);
            }
            else
            {
                Notification notify = new Notification();
                notify.Refresh("NPC_speckClose", m_info.ID);
                MsgCenter.Ins.SendMsg("CloseSpeckToNpc", notify);
            }

        };
    }
}
