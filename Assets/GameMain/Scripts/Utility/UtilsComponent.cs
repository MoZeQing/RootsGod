using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        public int startBlood = 100;
        public Dictionary<ConnectPair, LineData> Lines = new Dictionary<ConnectPair, LineData>();
        public Dictionary<ConnectPair, bool> ConnectPairs = new Dictionary<ConnectPair, bool>();//�Ƿ�����
        public Dictionary<Transform, bool> LinePairs = new Dictionary<Transform, bool>();//�ߵ�
        public Material[] materials = null;
        public Sprite[] sprites = null;//�ڵ����ͼ
        public GameObject cardNode = null;
        public GameObject[] nodes = null;
        public int[] cardCost = null;
        [HideInInspector] public double lineCost = 0;
        [HideInInspector] public List<GameObject> entityNode = null;
        [HideInInspector] public int depth1 = 0;
        [HideInInspector] public int depth2 = 0;
        [HideInInspector] public bool dragLine = false;
        public int Blood
        {
            get;
            set;
        }

        private void Start()
        {
            Blood = startBlood;
            entityNode = new List<GameObject>();
        }

        public void ShowCardPackage()
        {
            var node = Instantiate(GameEntry.Utils.cardNode, new Vector3(2, 0, 10.5f), 
                Quaternion.Euler(0, 0, 0));
            GameEntry.Utils.entityNode.Add(node);
        }

        //rate:几率数组�?），  total：几率总和�?00%�?
        // Debug.Log(rand(new int[] { 10, 5, 15, 20, 30, 5, 5,10 }, 100));
        public int rand(int[] rate, int total)
        {
            int r = Random.Range(1, total+1);
            int t = 0;
            for (int i = 0; i < rate.Length; i++)
            {
                t += rate[i];
                if (r < t)
                {
                    return i;
                }
            }
            return 0;
        }

        //rate:几率数组�?），  total：几率总和�?00%�?
        // Debug.Log(rand(new int[] { 10, 5, 15, 20, 30, 5, 5,10 }, 100));
        public int rand(Dictionary<int,int> rate, int total)
        {
            int r = Random.Range(1, total+1);
            int t = 0;
            print(r);
            foreach(var item in rate.Keys)
            {
                t += rate[item];
                if (r < t)
                {
                    return item;
                }
            }
            return 0;
        }
        
    }
}