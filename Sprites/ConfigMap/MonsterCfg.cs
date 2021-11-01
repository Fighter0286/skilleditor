using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// 怪物管理类  读取怪物配置表
/// </summary>
public class MonsterCfg
{
    static MonsterCfg _instacn;  //单例
    public static MonsterCfg Ins
    {
        get
        {
            if (_instacn == null)
            {
                _instacn = new MonsterCfg();
                _instacn.Init();
            }
            return _instacn;
        }
    }

    public JsonData data;
    string path;
    /// <summary>
    /// 获取路径
    /// </summary>
    void Init()
    {
        path = Application.streamingAssetsPath + @"/monster.txt";
        string json = File.ReadAllText(path);
        data = JsonConvert.DeserializeObject<JsonData>(json);
    }
    public JsonData GetJsonData()
    {
        return data;
    }
}

public class JsonData
{
    public List<MonsterData> datas = new List<MonsterData>();
}

public class MonsterData
{
    public string name;
    public float x;
    public float y;
    public float z;
    public MonsterType type;

    public MonsterData(string name, float x, float y, float z, MonsterType type)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
        this.type = type;
    }
}
