using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public abstract class BaseNodeComponent : Entity
    {
        /// <summary>
        /// 是否被起爆
        /// </summary>
        public bool IsBoom
        {
            get;
            set;
        }
        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnect
        {
            get;
            set;
        }
        /// <summary>
        /// 重新刷新节点的连接
        /// </summary>
        public virtual void SetConnect(bool flag)
        { 
            
        }

        public virtual void RemoveNode()
        { 
            
        }
    }
}