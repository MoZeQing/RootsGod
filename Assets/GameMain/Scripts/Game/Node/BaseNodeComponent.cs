using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public abstract class BaseNodeComponent : MonoBehaviour
    {
        /// <summary>
        /// 父节点列表
        /// </summary>
        public abstract List<BaseNodeComponent> ParentNodes { get; }

        /// <summary>
        /// 子节点列表
        /// </summary>
        public abstract List<BaseNodeComponent> ChildNodes { get; }
    }
}