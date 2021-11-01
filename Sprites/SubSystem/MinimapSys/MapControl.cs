using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图控制中心
/// </summary>
public class MapControl : MonoBehaviour
{
    public float xMap, yMap;
    public float xoffset, yoffset;

    private Transform player;
    //存放怪物的字典  类型 transform
    Dictionary<MonsterType, Transform> monsterdic = new Dictionary<MonsterType, Transform>();

    List<ObjectBase> otherGoPos = new List<ObjectBase>();  //存放所有的怪物
    Vector3 playerpos = new Vector3(0,0,0);  //玩家的位置
    List<Vector3> otherpos = new List<Vector3>();

    /// <summary>
    /// 初始化地图视图
    /// </summary>
    private void Awake()
    {
        xMap = this.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        yMap = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        xoffset = xMap / World.Ins.xlength;
        yoffset = xMap / World.Ins.ylength;

        player = transform.Find("player");  //玩家
        monsterdic.Add(MonsterType.Gather, transform.Find("gather"));
        monsterdic.Add(MonsterType.Normal, transform.Find("monster"));
        monsterdic.Add(MonsterType.NPC, transform.Find("npc"));

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //实例的怪物数量和地图数量不匹配  清空 重新赋值
        if (World.Ins.m_insDic.Count != otherpos.Count)
        {
            otherGoPos.Clear();
            otherpos.Clear();

            foreach (var item in World.Ins.m_insDic)
            {
                otherGoPos.Add(item.Value);
                otherpos.Add(new Vector3(0,0,0));
            }
        }
        //世界玩家的位置 * 缩放比 赋值给地图的位置
        if (player && World.Ins.m_player.m_go)
        {
            playerpos.Set(World.Ins.m_player.m_go.transform.position.x  * xoffset,World.Ins.m_player.m_go.transform.position.z*yoffset,0);
            player.localPosition = playerpos;
        }
        if (otherGoPos != null && otherGoPos.Count >0)
        {
            for (int i = 0; i < otherGoPos.Count; i++)
            {
                otherpos[i] = new Vector3(otherGoPos[i].m_go.transform.position.x*xoffset,otherGoPos[i].m_go.transform.position.z*yoffset,0);
                monsterdic[otherGoPos[i].m_type].transform.localPosition = otherpos[i];
            }
        }
    }
    private void OnDestroy()
    {
        CancelInvoke("UpdateMap");
    }
}
