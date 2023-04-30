using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class SetBoomEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(SetBoomEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public bool Boom
        {
            get;
            set;
        }

        public static SetBoomEventArgs Create(bool boom)
        {
            SetBoomEventArgs args = ReferencePool.Acquire<SetBoomEventArgs>();
            args.Boom = boom;
            return args;
        }

        public override void Clear()
        {

        }
    }
}
