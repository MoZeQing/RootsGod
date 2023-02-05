using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public struct ConnectPair
    {
        private readonly Transform m_First;
        private readonly Transform m_Second;

        public ConnectPair(Transform first, Transform second)
        {
            m_First = first;
            m_Second = second;
        }
        public Transform First
        {
            get
            {
                return m_First;
            }
        }
        public Transform Second
        {
            get
            {
                return m_Second;
            }
        }
    }
    public class UtilsComponent : GameFrameworkComponent
    {
        public Dictionary<ConnectPair, bool> ConnectPairs = new Dictionary<ConnectPair, bool>();
        public Dictionary<Transform, bool> LinePairs = new Dictionary<Transform, bool>();
        public Material[] materials = null;
        public GameObject[] nodes = null;
        public int Blood
        {
            get;
            set;
        }

        private void Start()
        {
            Blood = 100;
        }
    }
}