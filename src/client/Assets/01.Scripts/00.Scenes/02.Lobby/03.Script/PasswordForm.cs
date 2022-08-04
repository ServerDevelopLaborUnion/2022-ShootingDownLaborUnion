using UnityEngine;
using UnityEngine.UI;

public class PasswordForm : MonoBehaviour
{
    [SerializeField] InputField passwordInput = null;
    [SerializeField] Button passwordButton = null;
    [SerializeField] Button cancelButton = null;
    
    private RoomInfo _roomInfo = null;

    public void Show(RoomInfo roomInfo)
    {
        gameObject.SetActive(true);
        _roomInfo = roomInfo;

        passwordInput.text = string.Empty;

        passwordButton.onClick.RemoveAllListeners();
        passwordButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.JoinRoom(_roomInfo.UUID, passwordInput.text);
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
