using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class SetRigidbodyTypeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SetRigidbodyTypeEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public RigidbodyType2D Type
        {
            get;
            set;
        }

        public static SetRigidbodyTypeEventArgs Create(RigidbodyType2D type)
        {
            SetRigidbodyTypeEventArgs args = ReferencePool.Acquire<SetRigidbodyTypeEventArgs>();
            args.Type = type;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}