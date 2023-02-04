using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace GameMain
{
    public class AddIncomeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(AddIncomeEventArgs).GetHashCode();
        
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public int Income
        {
            get;
            set;
        }

        public static AddIncomeEventArgs Create(int output)
        {
            AddIncomeEventArgs args = ReferencePool.Acquire<AddIncomeEventArgs>();
            args.Income = output;
            return args;
        }
        
        public override void Clear()
        {
            
        }
    }
}