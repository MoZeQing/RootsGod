using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class Level1NodeComponent : BaseNodeComponent
    {
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        public override List<BaseNodeComponent> ParentNodes
        {
            get
            {
                return m_ParentNodes;
            }
        }
        private List<BaseNodeComponent> m_ChildNodes = new List<BaseNodeComponent>();

        public override List<BaseNodeComponent> ChildNodes
        {
            get
            {
                return m_ChildNodes;
            }
        }

        private void Start()
        {
            NodeType = NodeType.Level1Node;
            NodeState = NodeState.InActive;
            Select = false;
            Cost = false;
            Movable = false;
            Connectable = true;
        }

        private void Update()
        {
            
        }
    }
}