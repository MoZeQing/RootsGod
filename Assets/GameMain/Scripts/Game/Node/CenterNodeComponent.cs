using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using GameMain;
using UnityEngine;

public class CenterNodeComponent : BaseNodeComponent
{
    private int m_CostValue = 0;
    private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
    public override List<BaseNodeComponent> ParentNodes
    {
        get
        {
            return m_ParentNodes;
        }
    }
    private List<BaseNodeComponent> m_ChildNodes = new List<BaseNodeComponent>();

    public override List<BaseNodeComponent> ChildNodes
    {
        get
        {
            return m_ChildNodes;
        }
    }

    private int m_AllOutput = 0;
    
    private void Start()
    {
        NodeType = NodeType.CenterNode;
        NodeState = NodeState.Active;
        m_CostValue = 0;
        Select = false;
        Cost = false;
        Movable = false;
        Connectable = true;
    }

    private void OnEnable()
    {
        //GameEntry.Event.Subscribe(AddOutputEventArgs.EventId,AddOutput);
        //GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
    }

    private void OnDisable()
    {
        //GameEntry.Event.Unsubscribe(AddOutputEventArgs.EventId,AddOutput);
        //GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
    }

    private void AddOutput(object sender, GameEventArgs e)
    {
        AddIncomeEventArgs ne = (AddIncomeEventArgs)e;
        m_AllOutput += ne.Income;
    }

    private void SetSelect(object sender, GameEventArgs e)
    {
        SetSelectEventArgs ne = (SetSelectEventArgs)e;
        Select = ne.Select;
    }
}
