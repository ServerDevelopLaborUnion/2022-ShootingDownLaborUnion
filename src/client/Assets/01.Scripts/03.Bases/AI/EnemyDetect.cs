using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    private Transform _target = null;
    private List<Transform> _playerList = new List<Transform>();

    public Transform Target
    {
        get
        {
            if (_target == null)
            {
                ChangeTarget();
            }
            return _target;
        }
    }

    private void Start()
    {
        FindPlayers();
        ChangeTarget();
    }

    private void FindPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            _playerList.Add(player.transform);
        }
    }

    public void ChangeTarget()
    {
        float minDistance = float.MaxValue;
        float distance;
        _playerList.ForEach(player =>
        {
            distance = Vector3.Distance(transform.position, player.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _target = player;
            }
        });
    }

}
