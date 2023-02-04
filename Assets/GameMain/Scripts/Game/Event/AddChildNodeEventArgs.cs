using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class AddChildNodeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AddChildNodeEventArgs).GetHashCode();
        
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

        public static AddChildNodeEventArgs Create(NodeData nodeData)
        {
            AddChildNodeEventArgs args = ReferencePool.Acquire<AddChildNodeEventArgs>();
            args.NodeData = nodeData;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}