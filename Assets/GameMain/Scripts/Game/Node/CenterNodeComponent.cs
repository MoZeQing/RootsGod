using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameMain;
using UnityEngine;

public class CenterNodeComponent : BaseNodeComponent
{
    public override NodeType NodeType { get; set; }
    public override NodeState NodeState { get; set; }
    public override bool Cost { get; set; }
    public override bool Movable { get; set; }
    public override bool Connectable { get; set; }
    public override List<BaseNodeComponent> ParentNodes { get; set; }
    public override List<BaseNodeComponent> ChildNodes { get; set; }

    private int m_AllOutput = 0;
    
    private void Start()
    {
        NodeType = NodeType.CenterNode;
        NodeState = NodeState.Active;
        Cost = true;
        Movable = false;
    }

    private void OnEnable()
    {
        GameEntry.Event.Subscribe(AddOutputEventArgs.EventId,AddOutput);
    }

    private void OnDisable()
    {
        GameEntry.Event.Unsubscribe(AddOutputEventArgs.EventId,AddOutput);
    }

    private void AddOutput(object sender, GameEventArgs e)
    {
        AddOutputEventArgs ne = (AddOutputEventArgs)e;
        m_AllOutput += ne.Output;
    }
}
