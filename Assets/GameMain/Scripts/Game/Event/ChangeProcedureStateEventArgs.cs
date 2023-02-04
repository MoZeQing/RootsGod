using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class ChangeProcedureStateEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeProcedureStateEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public State State
        {
            get;
            set;
        }

        public static ChangeProcedureStateEventArgs Create(State state)
        {
            ChangeProcedureStateEventArgs args = ReferencePool.Acquire<ChangeProcedureStateEventArgs>();
            args.State = state;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}