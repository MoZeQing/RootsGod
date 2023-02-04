using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class AddParentNodeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AddParentNodeEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public NodeData NodeData
        {
            get;
            set;
        }

        public static AddParentNodeEventArgs Create(NodeData nodeData)
        {
            AddParentNodeEventArgs args = ReferencePool.Acquire<AddParentNodeEventArgs>();
            args.NodeData = nodeData;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}