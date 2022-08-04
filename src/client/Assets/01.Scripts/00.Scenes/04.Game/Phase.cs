using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase : MonoBehaviour
{
    [SerializeField] List<PhaseData> _phaseDataList;
    [SerializeField] bool _running = false;
    [SerializeField] int _phaseIndex;
    [SerializeField] Text _phaseText;

    public void UpdateUI()
    {
        if (_running) _phaseText.text = "";
        else _phaseText.text = $"Phase\n{_phaseIndex + 1}/{_phaseDataList.Count}";
    }

    public void StartPhase()
    {
        _running = true;
        UpdateUI();
    }

    public void EndPhase()
    {
        _running = false;
        _phaseIndex++;
        if (_phaseIndex >= _phaseDataList.Count)
        {
            _phaseIndex = 0;
        }
    }

    private void Update()
    {
        if (!Storage.CurrentUser.IsMaster && _running) return;

        foreach (var phaseData in _phaseDataList)
        {
            // ��� ���� óġ�Ǿ��� ��
            if (NetworkManager.Instance.entityList.FindAll(x => x.Data.Type == EntityType.Enemy).Count == 0)
            {
                var spawnData = phaseData.SpawnDataList[_phaseIndex];
                // TODO: �����Ϳ� �°� ��ȯ
                // �����Ϳ� ����, �� Ÿ�� ����. ������ �°� ��� ��ȯ�Ǹ� ��. �ʿ��ϸ� �Ӽ� �߰��ص� ��.
                // <-

                // <-
                _phaseIndex++;
                UpdateUI();
            }
        }
    }
}
