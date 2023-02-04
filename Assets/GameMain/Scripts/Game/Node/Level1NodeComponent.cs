using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class Level1NodeComponent : BaseNodeComponent
    {
        public override NodeType NodeType { get; set; }
        public override NodeState NodeState { get; set; }
        public override bool Cost { get; set; }
        public override bool Movable { get; set; }
        public override bool Connectable { get; set; }
        public override List<BaseNodeComponent> ParentNodes { get; set; }
        public override List<BaseNodeComponent> ChildNodes { get; set; }

        private void Start()
        {
            NodeType = NodeType.Level1Node;
        }

        private void Update()
        {
            
        }
    }
}