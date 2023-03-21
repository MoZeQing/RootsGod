using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    [Serializable]
    public class NodeData : EntityData
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

        public NodeData(int entityId, int typeId, NodeType nodeType, NodeState nodeState)
            : base(entityId, typeId)
        {
            this.NodeType = nodeType;
            this.NodeState = nodeState;
        }
    }
}
