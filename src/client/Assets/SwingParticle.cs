using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingParticle : MonoBehaviour
{
    private ParticleSystem _ps = null;
    // Start is called before the first frame update
    void Start()
    {
        _ps = transform.Find("SwingParticle").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ps != null)
        {
            ParticleSystem.MainModule main = _ps.main;
            if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
            {
                main.startRotation = -(transform.eulerAngles.z + 30) * Mathf.Deg2Rad;
            }
        }
        _ps.transform.localScale = transform.parent.parent.localScale;
    }
}
