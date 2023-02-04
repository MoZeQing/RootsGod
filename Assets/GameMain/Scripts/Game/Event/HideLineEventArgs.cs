using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class HideLineEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HideLineEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public static HideLineEventArgs Create()
        {
            HideLineEventArgs args = ReferencePool.Acquire<HideLineEventArgs>();
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}