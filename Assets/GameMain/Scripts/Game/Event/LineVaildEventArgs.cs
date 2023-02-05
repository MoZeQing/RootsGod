using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class LineVaildEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LineVaildEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool Valid
        {
            get;
            set;
        }

        public static LineVaildEventArgs Create(bool valid)
        {
            LineVaildEventArgs args = ReferencePool.Acquire<LineVaildEventArgs>();
            args.Valid = valid;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}