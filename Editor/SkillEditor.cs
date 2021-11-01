using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SkillEditor :  EditorWindow
{
    public class PlayerEditor
    {
        public int _folderIndex = 0;  //选择文件的下标
        public int _characterIndex = 0;  //选择预制体的下标
        public string characterName = string.Empty;  //预制体的名字
        public string folderName = string.Empty;  //文件的名字
        public string characterFilter = string.Empty;  //暂时没用
        public List<string> characterlist = new List<string>();  //存放预制体的集合
        public Player player = null;  //玩家类
    }

    PlayerEditor m_player = new PlayerEditor();

    List<string> m_folderlist = new List<string>();  //文件名

    List<string> m_characterList = new List<string>();  //所有预制体名字

    Dictionary<string, List<string>> m_folderPrefabs = new Dictionary<string, List<string>>();  //按照文件名存储预制体名

    SkillWindew skillWindew;  //技能详情窗口

    string newSkillName = string.Empty;  //存储新的技能的名字

    /// <summary>
    /// 用来初始化数据
    /// </summary>
    private void OnEnable()
    {
        DoSearchFolder();
        DoSearchCharacter();
    }

    /// <summary>
    /// 操作文件  获取指定路径下所有的文件夹的名字
    /// </summary>
    private void DoSearchFolder()
    {
        m_folderlist.Clear();
        m_folderlist.Add("All");
        string[] folders = Directory.GetDirectories(GetPath());  //查找固定路径下的所有文件名字
        foreach (var item in folders)
        {
            m_folderlist.Add(Path.GetFileName(item));
        }
    }


    private string GetPath()
    {
        return Application.dataPath + "/GameDate/Model";
    }

    /// <summary>
    /// 操作预制体 获取指定目录下的所有预制体
    /// </summary>
    private void DoSearchCharacter()
    {
        m_characterList.Clear();
        string[] files = Directory.GetFiles(GetPath(),"*.prefab",SearchOption.AllDirectories);
        foreach (var item in files)
        {
            m_characterList.Add(Path.GetFileNameWithoutExtension(item));
        }
        m_characterList.Sort();
        m_characterList.Insert(0,"Null");
        m_player.characterlist.AddRange(m_characterList);
    }

    [MenuItem("Tooks/技能编辑器")]
  static void Init()
    {
        if (Application.isPlaying)
        {
            SkillEditor skill = SkillEditor.GetWindow<SkillEditor>("技能编辑器");
            //显示窗口
            if (skill!=null)
            {
                skill.Show();
                
            }
        }
        else
        {
            Debug.Log("请先运行项目");
        }

    }
    private void OnGUI()
    {
        int folderIndex = EditorGUILayout.Popup(m_player._folderIndex, m_folderlist.ToArray());  //文件夹筛选弹窗
        if (folderIndex!=m_player._folderIndex)
        {
            m_player._folderIndex = folderIndex;
            m_player._characterIndex = -1;
            string folderName = m_folderlist[m_player._folderIndex];
            List<string> list;
            if (folderName.Equals("All"))
            {
                list = m_characterList;
            }
            else
            {
                if (!m_folderPrefabs.TryGetValue(folderName,out list))  //判断字典中有没有这个key，没有就添加
                {
                    list = new List<string>();
                    string[] files = Directory.GetFiles(GetPath()+"/" + folderName, "*.prefab", SearchOption.AllDirectories);
                    foreach (var item in files)
                    {
                        list.Add(Path.GetFileNameWithoutExtension(item));
                    }
                    m_folderPrefabs.Add(folderName, list);
                }
            }
            m_player.characterlist.Clear();
            m_player.characterlist.AddRange(list);
        }
        int characterIndex = EditorGUILayout.Popup(m_player._characterIndex,m_player.characterlist.ToArray());
        if (characterIndex != m_player._characterIndex)
        {
            m_player._characterIndex = characterIndex;
            if (m_player.characterName!= m_player.characterlist[m_player._characterIndex])
            {
                m_player.characterName = m_player.characterlist[m_player._characterIndex];
                if (!string.IsNullOrEmpty(m_player.characterName))
                {
                    if (m_player.player !=null)
                    {
                        m_player.player.Destroy();
                    }
                    m_player.player = Player.Init(m_player.characterName);
                }
            }
        }
        newSkillName = EditorGUILayout.TextField(newSkillName);
        if (GUILayout.Button("创建新的技能"))
        {
            Debug.Log("创建成功");
            if (!string.IsNullOrEmpty(newSkillName) && m_player.player !=null)
            {
                Debug.Log(newSkillName);
                List<SkillBase> skils = m_player.player.AddNewSkill(newSkillName);
                OpenSkillWindew(newSkillName,skils);

                newSkillName = "";
            }
        }

        if (m_player.player!=null)
        {
            ScrollViewPos = GUILayout.BeginScrollView(ScrollViewPos,false,true);  //开启一个
            foreach (var item in m_player.player.skilllist)  //遍历player的字典
            {
                GUILayout.BeginHorizontal();  //开启一个横向布局
                if (GUILayout.Button(item.Key))
                {
                    List<SkillBase> skillComponents = m_player.player.GetSkill(item.Key);
                    foreach (var ite in skillComponents)
                    {
                        ite.Init();
                    }
                    //打开一个新窗口  传过来技能名字和 player
                    OpenSkillWindew(item.Key,skillComponents);
                }

                GUILayoutOption[] opyion = new GUILayoutOption[]
                {
                    GUILayout.Width(60),
                    GUILayout.Height(19)
                };

                if (GUILayout.Button("删除技能",opyion))
                {
                    m_player.player.RevSkill(item.Key);
                    break;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        if (GUILayout.Button("保存")&& m_player.player != null)
        {
            string str= m_player.player.Save();
            Debug.Log(str);
        }


    }
    Vector2 ScrollViewPos = new Vector2(0,0);
    /// <summary>
    /// 打开一个新窗口
    /// </summary>
    /// <param name="newSkillName"></param>
    /// <param name="skilComponents"></param>
    private void OpenSkillWindew(string newSkillName, List<SkillBase> skilComponents)
    {
        if (skilComponents!= null)
        {
            if (skillWindew ==null)
            {
                skillWindew = EditorWindow.GetWindow<SkillWindew>("");
            }
            skillWindew.titleContent = new GUIContent(newSkillName);//更改窗口的名字

            skillWindew.SetInitSkill(skilComponents,m_player.player);
            skillWindew.Show();
            skillWindew.Repaint();
        }
    }
}
