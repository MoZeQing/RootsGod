using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class AddOutputEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AddOutputEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Output
        {
            get;
            set;
        }

        public static AddOutputEventArgs Create(int output)
        {
            AddOutputEventArgs args = ReferencePool.Acquire<AddOutputEventArgs>();
            args.Output = output;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}