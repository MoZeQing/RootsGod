using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ShowTipsEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowTipsEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int TipType
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        public static ShowTipsEventArgs Create(int tipType,bool active)
        {
            ShowTipsEventArgs args = ReferencePool.Acquire<ShowTipsEventArgs>();
            args.TipType = tipType;
            args.Active = active;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}