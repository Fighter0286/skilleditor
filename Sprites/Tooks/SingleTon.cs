using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> where T : class, new()
{
    private static T ins;
    private static object LocObj = new object();
    public static T Ins
    {
        get
        {
            if (ins == null)
            {
                lock (LocObj)
                {
                    if (ins == null)
                    {
                        ins = new T();
                    }
                }
            }
            return ins;
        }
    }
}
