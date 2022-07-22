using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _rolePanel;

    [SerializeField]
    private int _roleNumber;
    public void OnClickReady(){
        _rolePanel.SetActive(true);
        RoomManager.Instance.SetRole(_roleNumber);
    }

    public void OnClickCancel(){
        _rolePanel.SetActive(false);
        RoomManager.Instance.SetCancel(_roleNumber);
    }
}
