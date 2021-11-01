using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneUtils
{
    /// <summary>
    /// 加载场景之后再执行方法
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="call"></param>
    static public void LoadSceneAsync(string sceneName, Action call)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.completed += (_ao) => {
            call?.Invoke();
        };
    }
}
