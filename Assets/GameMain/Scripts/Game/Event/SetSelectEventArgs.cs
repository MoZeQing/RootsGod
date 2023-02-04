using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class SetSelectEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SetSelectEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool Select
        {
            get;
            set;
        }

        public static SetSelectEventArgs Create(bool select)
        {
            SetSelectEventArgs args = ReferencePool.Acquire<SetSelectEventArgs>();
            args.Select = select;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}