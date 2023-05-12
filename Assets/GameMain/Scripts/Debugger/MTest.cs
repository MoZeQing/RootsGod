using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameMain;

/// <summary>
/// �����ýű���ʹ�ú�����ɾ��
/// </summary>
public class MTest : MonoBehaviour
{
    int entityId;
    // Start is called before the first frame update
    void Start()
    {
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10001, NodeType.CenterNode, NodeState.Active)
        {
            Position = new Vector3(1, 1, 10),
            Scale = new Vector3(0.3f, 0.3f, 1f)
        });
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10001, NodeType.BlockingNode, NodeState.Active)
        {
            Position = new Vector3(1, 1, 10),
            Scale = new Vector3(0.3f, 0.3f, 1f)
        }) ;
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10001, NodeType.Level1Node, NodeState.Active)
        {
            Position = new Vector3(2, 1, 10),
            Scale = new Vector3(0.3f, 0.3f, 1f)
        });
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10001, NodeType.Level2Node, NodeState.Active)
        {
            Position = new Vector3(3, 1, 10),
            Scale = new Vector3(0.3f, 0.3f, 1f)
        });
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10001, NodeType.Level2To1Node, NodeState.Active)
        {
            Position = new Vector3(4, 1, 10),
            Scale = new Vector3(0.3f, 0.3f, 1f)
        });
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10001, NodeType.EmptyNode, NodeState.Active)
        {
            Position = new Vector3(5, 1, 10),
            Scale = new Vector3(0.3f, 0.3f, 1f)
        });
        GameEntry.Entity.ShowNode(new NodeData(GameEntry.Entity.GenerateSerialId(), 10001, NodeType.ClearNode, NodeState.Active)
        {
            Position = new Vector3(6, 1, 10),
            Scale = new Vector3(0.3f, 0.3f, 1f)
        });
    }


    [MenuItem("Test/Test")]
    public static void SwitchNode()
    {
        GameEntry.Entity.GetEntity(-1).GetComponent<Node>().SwitchType(new NodeData(-1, 10001, NodeType.Level2Node, NodeState.Active));
    }
}
