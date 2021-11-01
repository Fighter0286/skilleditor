using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_info   //信息基类
{
    public int ID;        //1 or 2
    public string m_name;  //名称
    public Vector3 m_pos;  //位置
    public string m_res;  //读取的名字
    public MonsterType m_type;  //怪物类型
}

/// <summary>
/// 角色信息
/// </summary>
public class player_info: object_info
{
    public int m_level;  //等级
    public float m_HP;  //血量
    public float m_hpMax;  //最大血量
    public float m_MP;  //当前血量
    public float m_mpMax;
    //技能列表
    public List<SkillXml> skillList;
}

/// <summary>
/// npc信息
/// </summary>
public class npc_info : object_info
{
    public int m_plotId = 0; //0是不响应

    public npc_info(int plot, object_info info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = MonsterType.NPC;
    }
}

/// <summary>
/// 怪物的信息
/// </summary>
public class monster_info : object_info
{
    public monster_info(MonsterType type, object_info info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = type;
    }

}