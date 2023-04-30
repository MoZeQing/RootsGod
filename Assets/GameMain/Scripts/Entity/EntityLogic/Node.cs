using GameMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.DataTable;

public class Node : Entity
{
    public NodeData NodeData { get; set; } = null;
    public BaseNodeComponent Component { get; set; } = null;

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        NodeData = userData as NodeData;
        AttachType();
    }
    /// <summary>
    /// 切换组件
    /// </summary>
    /// <param name="userData"></param>
    public void SwitchType(object userData)
    {
        NodeData = userData as NodeData;
        GameEntry.Entity.DetachChildEntities(this.Id);
        AttachType();
    }
    /// <summary>
    /// 添加组件
    /// </summary>
    public void AttachType()
    {
        if (NodeData == null)
        {
            UnityGameFramework.Runtime.Log.Error("NodeData object data is invalid.");
            return;
        }
        int entityId = GameEntry.Entity.GenerateSerialId();
        IDataTable<DRNode> dtNode = GameEntry.DataTable.GetDataTable<DRNode>();
        DRNode drNode = dtNode.GetDataRow((int)NodeData.NodeType);
        if (drNode == null)
        {

            return;
        }
        ComponentData data = new ComponentData(entityId, 10002, this.Id, NodeData);
        switch (NodeData.NodeType)
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
            case NodeType.TreeNode:
                GameEntry.Entity.ShowTreeNode(data);
                break;
            case NodeType.BoomNode:
                GameEntry.Entity.ShowBoomNode(data);
                break;
            case NodeType.LeafNode:
                GameEntry.Entity.ShowLeafNode(data);
                break;
        }
        ///实体创建有加载时间，获取写在被生成体上，大坑！
    }
}
