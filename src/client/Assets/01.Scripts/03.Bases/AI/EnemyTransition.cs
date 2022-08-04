using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTransition : AITransition
{
    [field:SerializeField]
    public override AIState StartState { get; set; }
    [field:SerializeField]
    public override List<AICondition> PositiveConditions { get; set; }
    [field:SerializeField]
    public override List<AICondition> NegativeConditions { get; set; }
    [field:SerializeField]
    public override bool IsOr { get; set; }
    [field:SerializeField]
    public override AIState NextState { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
