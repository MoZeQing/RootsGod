using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڵ�����
/// </summary>
public enum NodeType
{
    Unknown,//δ֪
    CenterNode,//���Ľڵ�
    Level1Node,//�ȼ�1�ڵ㣨ţţ��
    Level2Node,//�ȼ�2�ڵ㣨����
    Level2To1Node,//�ȼ�2��1�ڵ㣨������
    EmptyNode,//�սڵ�
    BlockingNode,//�赲�ڵ�
    ClearNode,//��ʴ�ڵ�
    CardPackage,//����
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
