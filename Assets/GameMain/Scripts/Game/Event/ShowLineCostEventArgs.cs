using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ShowLineCostEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowLineCostEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool Active
        {
            get;
            set;
        }

        public static ShowLineCostEventArgs Create(bool active)
        {
            ShowLineCostEventArgs args = ReferencePool.Acquire<ShowLineCostEventArgs>();
            args.Active = active;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}