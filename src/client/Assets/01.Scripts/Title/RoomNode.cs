using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNode : MonoBehaviour
{
    [SerializeField]
    private Text roomName = null;

    [SerializeField]
    private Text personnel = null;

    [SerializeField]
    private Image isPrivate = null;

    public void SetInfo(string roomName, int maxPersonnel, int currentPersonnel, bool isPrivate)
    {
        this.roomName.text = roomName;
        this.personnel.text = $"{currentPersonnel.ToString()}/{maxPersonnel.ToString()}";
    }
}
