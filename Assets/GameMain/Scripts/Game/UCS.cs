using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 节点类型
/// </summary>
public enum NodeType
{
    Unknown,//未知
    CenterNode,//中心节点
    Level1Node,//等级1节点（牛牛）
    Level2Node,//等级2节点（鼠鼠）
    Level2To1Node,//等级2到1节点（消化）
    EmptyNode,//空节点
    BlockingNode,//阻挡节点
    ClearNode,//腐蚀节点
    CardPackage,//卡包
                //Level3To2Node,
                //Level3Node,
                //BoomNode,
}

public enum NodeState
{
    Undefined,
    InActive,
    Active,
}

public enum CardPackageType
{
    UnDefined,
    Normal,
}


public static class UCS
{
    public static string ProcedureMain = "GameMain.ProcedureMain";
    public static int BloodPerUnit = 20;
    
}
