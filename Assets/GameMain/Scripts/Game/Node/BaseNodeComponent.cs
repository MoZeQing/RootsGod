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
        Unknown,
        CenterNode,
        Level1Node,
        Level2Node,
        Level3Node,
        Level2To1Node,
        Level3To2Node,
        EmptyNode,
        ObstacleNode,
        ClearNode,
        BoomNode,
    }
    public abstract class BaseNodeComponent : MonoBehaviour
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        public NodeType NodeType
        {
            get; 
            set;
        }

        /// <summary>
        /// 节点状态
        /// </summary>
        public NodeState NodeState
        {
            get;
            set;
        }

        public bool Select
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否消耗资源
        /// </summary>
        public bool Cost
        {
            get;
            set;
        }
        
        /// <summary>
        /// 是否可移动
        /// </summary>
        public bool Movable
        {
            get;
            set;
        }

        /// <summary>
        /// 是否可连接
        /// </summary>
        public bool Connectable
        {
            get;
            set;
        }

        /// <summary>
        /// 父节点列表
        /// </summary>
        public abstract List<BaseNodeComponent> ParentNodes
        {
            get;
        }

        /// <summary>
        /// 子节点列表
        /// </summary>
        public abstract List<BaseNodeComponent> ChildNodes
        {
            get;
        }
    }
}