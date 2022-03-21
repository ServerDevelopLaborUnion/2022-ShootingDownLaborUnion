using System.Collections.Generic;
using System;

public class FuncEventManager<T>
{
    private static Dictionary<string, Func<T>> eventFuncDictionary = new Dictionary<string, Func<T>>();

    public static void StartListening(string eventName, Func<T> listener)
    {
        Func<T> thisEvent;
        if (eventFuncDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventFuncDictionary[eventName] = thisEvent;
        }
        else
        {
            eventFuncDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(string eventName, Func<T> listener)
    {
        Func<T> thisEvent;
        if (eventFuncDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventFuncDictionary[eventName] = thisEvent;
        }
        else
        {
            eventFuncDictionary.Remove(eventName);
        }
    }

    public static void FuncTriggerEvent(string eventName, T param)
    {
        Func<T> thisEvent;
        if (eventFuncDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    private static Dictionary<string, Func<EventParam, T>> eventParamFuncDictionary = new Dictionary<string, Func<EventParam, T>>();

    public static void StartListening(string eventName, Func<EventParam, T> listener)
    {
        Func<EventParam, T> thisEvent;
        if (eventParamFuncDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventParamFuncDictionary[eventName] = thisEvent;
        }
        else
        {
            eventParamFuncDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(string eventName, Func<EventParam, T> listener)
    {
        Func<EventParam, T> thisEvent;
        if (eventParamFuncDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventParamFuncDictionary[eventName] = thisEvent;
        }
        else
        {
            eventParamFuncDictionary.Remove(eventName);
        }
    }

    public static void FuncParamTriggerEvent(string eventName, EventParam param)
    {
        Func<EventParam, T> thisEvent;
        if (eventParamFuncDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
        }
    }
}
