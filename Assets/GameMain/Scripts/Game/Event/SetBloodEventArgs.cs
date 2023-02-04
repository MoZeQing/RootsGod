using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class SetBloodEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SetBloodEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Blood
        {
            get;
            set;
        }

        public static SetBloodEventArgs Create(int blood)
        {
            SetBloodEventArgs args = ReferencePool.Acquire<SetBloodEventArgs>();
            args.Blood = blood;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}