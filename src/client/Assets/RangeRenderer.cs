using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform RangeTrm = null;

    [SerializeField]
    private float _range = 0;

    public void RenderRange()
    {
        RangeTrm.localScale = new Vector2(1 + _range, (1 + _range) / 2);
    }

    private void Update()
    {
        RenderRange();
    }
}
