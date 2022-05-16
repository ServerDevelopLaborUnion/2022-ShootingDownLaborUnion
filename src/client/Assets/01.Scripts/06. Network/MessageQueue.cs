using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTask : MonoSingleton<MainTask>
{
    [RuntimeInitializeOnLoadMethod]
    static void OnSecondRuntimeMethodLoad()
    {
        Initialize(true);
    }

    private static Queue<Action> _messageQueue = new Queue<Action>();

    private void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        _messageQueue.Clear();
    }

    public static void Enqueue(Action action)
    {
        _messageQueue.Enqueue(action);
    }

    private void Update()
    {
        if (_messageQueue.Count > 0)
        {
            _messageQueue.Dequeue()();
        }
    }
}