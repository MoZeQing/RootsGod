using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : Entity
{
    private NodeData m_Data = null;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_Data = userData as NodeData;
        AttachType();
    }
    /// <summary>
    /// 切换组件
    /// </summary>
    /// <param name="userData"></param>
    public void SwitchType(object userData)
    {
        m_Data = userData as NodeData;
        GameEntry.Entity.DetachChildEntities(this.Id);
        AttachType();
    }
    /// <summary>
    /// 添加组件
    /// </summary>
    public void AttachType()
    {
        if (m_Data == null)
        {
            UnityGameFramework.Runtime.Log.Error("NodeData object data is invalid.");
            return;
        }
        ComponentData data = new ComponentData(GameEntry.Entity.GenerateSerialId(), 10002, this.Id, m_Data);
        switch (m_Data.NodeType)
        {
            case NodeType.Unknown:
                break;
            case NodeType.CenterNode:
                GameEntry.Entity.ShowCenterNode(data);
                break;
            case NodeType.Level1Node:
                GameEntry.Entity.ShowLevel1Node(data);
                break;
            case NodeType.Level2Node:
                GameEntry.Entity.ShowLevel2Node(data);
                break;
            case NodeType.Level2To1Node:
                GameEntry.Entity.ShowLevel2To1Node(data);
                break;
            case NodeType.EmptyNode:
                GameEntry.Entity.ShowEmptyNode(data);
                break;
            case NodeType.BlockingNode:
                GameEntry.Entity.ShowBlockingNode(data);
                break;
            case NodeType.ClearNode:
                GameEntry.Entity.ShowClearNode(data);
                break;
        }
    }
}
