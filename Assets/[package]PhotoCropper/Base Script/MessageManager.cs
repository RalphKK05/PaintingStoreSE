using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class MessageManager 
{
    static Dictionary<string, List<Action<object>>> m_message = new Dictionary<string, List<Action<object>>>();
    public static void RegisterMessage(string messageName, Action<object> fun)
    {
        List<Action<object>> lst= null;
        if (m_message.TryGetValue(messageName, out lst))
        {
            if (lst.IndexOf(fun) == -1) 
            {
                lst.Add(fun);
            }
        }
        else
        {
            lst = new List<Action<object>>();
            lst.Add(fun);
            m_message.Add(messageName, lst);
        }
    }

    public static void RemoveMessage(string messageName, Action<object> fun)
    {
        List<Action<object>> lst = null;
        if (m_message.TryGetValue(messageName, out lst))
        {
            if (lst.IndexOf(fun) != -1)
            {
                lst.Remove(fun);
            }
        }
    }

    public static void SendMessage(string messageName,object obj = null)
    {
        List<Action<object>> lst = null;
        if (m_message.TryGetValue(messageName, out lst))
        {
            foreach (Action<object> fun in lst)
            {
                fun(obj);
            }
        }
    }
}
