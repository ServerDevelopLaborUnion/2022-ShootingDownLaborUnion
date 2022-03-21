/**
     Couroutine 사용시 return Yield return의 Object가 동적으로 생성되는 것 같다.
    반복적인 생성이 GC를 생성하기 때문에 캐싱하여 사용한다.
*/
using UnityEngine;
using System.Collections.Generic;
using System;

public static class Yields {


    //WaitForEndOfFrame을 만들어 두고 반환
    private static WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame WaitEndOfFrame
    {
        get { return waitForEndOfFrame; }
    }

    //WaitForFixedUpdate를 만들어 두고 반환
    private static WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate WaitFixedUpdate
    {
        get { return WaitForFixedUpdate; }
    }

    //WaitForSeconds 캐싱을 위해 정보를 담아두기 위한 자료구조
    private static Dictionary<float, WaitForSeconds> timeInterval = new Dictionary<float, WaitForSeconds>();

    //WaitForSeconds 정보를 만들어 두고 반환
    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (!timeInterval.ContainsKey(seconds))
            timeInterval.Add(seconds, new WaitForSeconds(seconds));
        return timeInterval[seconds];
    }

    //WaitForRealtime
    private static Dictionary<float, WaitForSecondsRealtime> realTimeInterval = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSecondsRealtime WaitForSecondsRealtime(float seconds){
        if (!realTimeInterval.ContainsKey(seconds))
            realTimeInterval.Add(seconds, new WaitForSecondsRealtime(seconds));
        return realTimeInterval[seconds];
    }

    //WaitUntil
    private static Dictionary<Func<bool>, WaitUntil> untilInterval = new Dictionary<Func<bool>, WaitUntil>();

    public static WaitUntil WaitUntil(Func<bool> func){
        if(!untilInterval.ContainsKey(func))
            untilInterval.Add(func, new WaitUntil(func));
        return untilInterval[func];
    }

    //WaitWhile
    private static Dictionary<Func<bool>, WaitWhile> whileInterval = new Dictionary<Func<bool>, WaitWhile>();

    public static WaitWhile WaitWhile(Func<bool> func){
        if(whileInterval.ContainsKey(func))
            whileInterval.Add(func, new WaitWhile(func));
        return whileInterval[func];
    }

}