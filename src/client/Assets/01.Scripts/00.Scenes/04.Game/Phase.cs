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
            // 모든 적이 처치되었을 때
            if (NetworkManager.Instance.entityList.FindAll(x => x.Data.Type == EntityType.Enemy).Count == 0)
            {
                var spawnData = phaseData.SpawnDataList[_phaseIndex];
                // TODO: 데이터에 맞게 소환
                // 데이터에 도형, 적 타입 있음. 도형에 맞게 몇마리 소환되면 됨. 필요하면 속성 추가해도 됨.
                // <-

                // <-
                _phaseIndex++;
                UpdateUI();
            }
        }
    }
}
