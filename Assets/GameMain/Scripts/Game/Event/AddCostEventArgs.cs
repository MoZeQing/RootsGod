using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class AddCostEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AddCostEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Cost
        {
            get;
            set;
        }

        public static AddCostEventArgs Create(int cost)
        {
            AddCostEventArgs args = ReferencePool.Acquire<AddCostEventArgs>();
            args.Cost = cost;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}