using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// 怪物编辑器
/// </summary>
public class MosterEdutor : EditorWindow
{
    static MosterEdutor win;  //怪物窗口

    static List<GameObject> monsters = new List<GameObject>();  //怪物集合
    static List<Monstervalue> monster_value = new List<Monstervalue>();  //要写入json 的信息

    static JsonData _json = new JsonData();  //json时候要用到的类

    static string path;  //路径
    static GameObject root;  //父级

    [MenuItem("Tooks/怪物编辑器")]
    public static void Init()
    { 
        path = Application.streamingAssetsPath + "/monster.txt";  //生成路径
        root = GameObject.Find("NPC_Root");  //查找父级
        if (root == null)
        {
            Debug.Log("父级是空");
        }
        else
        { //先清空
            monsters.Clear();
            monster_value.Clear();
            _json.datas.Clear();
            //打开一个窗口
            win = EditorWindow.GetWindow<MosterEdutor>(typeof(MosterEdutor));
            win.Show();
        }
    }


    MonsterData data;
    ////布局
    //GUILayoutOption[] size1 = {
    //        GUILayout.Width(200),
    //        GUILayout.Height(50)
    //     };
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        //数量》0
        if (monsters.Count>0)
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                //隐藏
                if (monsters[i].activeSelf)
                {
                    EditorGUILayout.BeginHorizontal();
                    monster_value[i].isselect = EditorGUILayout.Toggle(monsters[i].name, monster_value[i].isselect);
                    monster_value[i].type = (MonsterType)EditorGUILayout.EnumPopup("type",monster_value[i].type);
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    monster_value[i].isselect = false;
                }
            }

            if (GUILayout.Button("保存"))
            {
                SaveData();
            }
        }

        EditorGUILayout.EndVertical();
    }

    private void SaveData()
    {
        for (int i = 0; i < monster_value.Count; i++)
        {
            if (monster_value[i].isselect)
            {
                //Debug.Log(string.Format("统计数据 name:{0} pos:{1},{2},{3}", monsters[i].name, monsters[i].transform.position.x
                //    , monsters[i].transform.position.y, monsters[i].transform.position.z));

                data = new MonsterData(monsters[i].name,monsters[i].transform.position.x,monsters[i].transform.position.y,monsters[i].transform.position.z,monster_value[i].type);

                _json.datas.Add(data);
            }
        }

        string json = JsonConvert.SerializeObject(_json);
        Debug.Log(json);
        File.WriteAllText(path, json);
        Debug.Log("生成成功");
    }

    int count = 0;
    Monstervalue value;
    private void Update()
    {
        //如果父级存在
        if (root)
        {
            count = root.transform.childCount;
            if (count>0  &&count!=monsters.Count)
            {
                monsters.Clear();
                monster_value.Clear();

                for (int i = 0; i < count; i++)
                {
                    monsters.Add(root.transform.GetChild(i).gameObject);
                    value = new Monstervalue();
                    value.isselect = true;  //所有默认是可以添加
                    monster_value.Add(value);
                }
            }
        }
    }
}

/// <summary>
/// 怪物类
/// </summary>
public class Monstervalue
{
    public bool isselect = true;
    public MonsterType type = MonsterType.Normal;
}

///// <summary>
///// 类型枚举
///// </summary>
//public enum MonsterType
//{
//    Null = 0,
//    Normal, //怪物
//    Gather, //采集物
//    Biaoche, //跟随物
//    NPC,
//}

public class JsonData
{
    public List<MonsterData> datas = new List<MonsterData>();
}
//[Serializable]
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