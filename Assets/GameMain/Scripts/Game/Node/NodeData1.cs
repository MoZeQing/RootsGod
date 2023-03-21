using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 节点状态
    /// </summary>
    public enum NodeState
    {
        Unknown,
        InActive,
        Active,
    }

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
    public class NodeData1 : MonoBehaviour
    {
        
        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType NodeType { get; set; }

        /// <summary>
        /// 节点状态
        /// </summary>
        public NodeState NodeState { get; set; }

        /// <summary>
        /// 是否被选择
        /// </summary>
        public bool Select { get; set; }

        /// <summary>
        /// 是否消耗资源
        /// </summary>
        public bool Costable { get; set; }

        /// <summary>
        /// 是否可移动
        /// </summary>
        public bool Movable { get; set; }

        /// <summary>
        /// 是否可连接
        /// </summary>
        public bool Connectable { get; set; }

        public float Total
        {
            get;
            set;
        }
        
        public int Income
        {
            get;
            set;
        }

        public int CostPersecond
        {
            get;
            set;
        }
        
        public bool IsPhysic
        {
            get;
            set;
        }

        /// <summary>
        /// 父节点列表
        /// </summary>
        public List<NodeData> ParentNodes = new List<NodeData>();

        /// <summary>
        /// 子节点列表
        /// </summary>
        public List<NodeData> ChildNodes = new List<NodeData>();
    }
}