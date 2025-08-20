using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Validate", story: "[Data] is Validate", category: "Variable Conditions", id: "6a5d772723ac9b6b390f3a0de8a69b64")]
public partial class ValidateCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Data;

    public override bool IsTrue()
    {
        if (Data != null)
            return true;
        else
            return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
