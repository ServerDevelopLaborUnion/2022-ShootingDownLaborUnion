using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNode : MonoBehaviour
{
    private Text roomName = null;
    private Text personnel = null;

    void Start()
    {
        roomName = GetComponent<Text>();
        personnel = GetComponent<Text>();
    }

    public void SetInfo(string roomName, int maxPersonnel, int currentPersonnel)
    {
        this.roomName.text = roomName;
        this.personnel.text = $"{currentPersonnel.ToString()}/{maxPersonnel.ToString()}";
    }
}
