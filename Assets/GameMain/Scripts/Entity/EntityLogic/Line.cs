﻿using System;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

namespace GameMain
{
    /// <summary>
    /// 线的状态
    /// </summary>
    public enum LineState
    {
        Undefined,
        NotConnect,
        Connect,
    }
    public class Line : Entity
    {
        [Header("Line")]
        [SerializeField] private LineState mLineState = LineState.Undefined;
        [SerializeField] private float mWidth = 0.25f;
        private LineData m_Data = null;
        private LineRenderer m_LineRenderer = null;
        private Material m_Material = null;
        private bool m_LineValid = true;
        private PolygonCollider2D m_PolygonCollider2D = null;

        [Header("Mouse")]
        private float m_DepthZ = 10;
        private Vector3 m_MousePositionOnScreen = Vector3.zero;
        private Vector3 m_MousePositionInWorld = Vector3.zero; 
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameEntry.Event.Subscribe(HideLineEventArgs.EventId,HideLine);//注册事件
            GameEntry.Event.Subscribe(LineVaildEventArgs.EventId,LineValid);

            m_Data = userData as LineData;
            if (m_Data == null)
            {
                Log.Error("LineData object data is invalid.");
                return;
            }
            GameEntry.Event.FireNow(this,ShowTipsEventArgs.Create(0,true));
            GameEntry.Utils.dragLine = true;
            m_LineValid = true;
            m_PolygonCollider2D = transform.GetComponent<PolygonCollider2D>();
            mLineState = LineState.NotConnect;
            m_LineRenderer = transform.GetComponent<LineRenderer>();
            var randomNum = Random.Range(0, 2);
            m_LineRenderer.material = GameEntry.Utils.materials[randomNum];
            m_Material = m_LineRenderer.material;
            m_LineRenderer.SetPosition(0,m_Data.Self.position);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(HideLineEventArgs.EventId,HideLine);
            GameEntry.Event.Unsubscribe(LineVaildEventArgs.EventId,LineValid);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            switch (mLineState)//根据线的状态
            {
                case LineState.Undefined:
                    break;
                case LineState.NotConnect://未连接状态（即线在鼠标上的状态）
                    GetMousePos(out m_MousePositionInWorld);
                    m_LineRenderer.SetPosition(1,m_MousePositionInWorld);
                    Vector2 startPos = m_Data.Self.transform.position;
                    Vector2 endPos = m_MousePositionInWorld;
                    Vector2 seVec = startPos - endPos;
                    Vector2 rotateVec = new Vector2(-seVec.y, seVec.x).normalized;
                    m_PolygonCollider2D.points = new Vector2[]//多边形碰撞箱赋值
                    {
                        startPos + rotateVec * mWidth,
                        startPos - rotateVec * mWidth,
                        endPos - rotateVec * mWidth,
                        endPos + rotateVec * mWidth,
                    };
                    
                    var distance = Vector3.Distance(m_LineRenderer.GetPosition(0),
                        m_LineRenderer.GetPosition(1));
                    //Debug.Log(distance);
                    var cost = Math.Round(distance,1) * UCS.BloodPerUnit;//计算花费，USC是写死的花费系数
                    GameEntry.Utils.lineCost = cost;
                    if (m_LineValid && GameEntry.Utils.Blood >= cost)
                    {
                        m_Material.color = Color.white;
                        if (!Input.GetMouseButtonDown(0))
                            return;
                        RaycastHit2D hit = Physics2D.Raycast(m_MousePositionInWorld, Vector2.zero,
                            Mathf.Infinity,LayerMask.GetMask("Node"));
                        if (!hit)
                            return;
                        if (hit.collider.transform.position == m_Data.Self.position)//排除自身
                            return;
                        var connectPair = new ConnectPair(m_Data.Self, hit.transform);
                        if (GameEntry.Utils.ConnectPairs.ContainsKey(connectPair))
                        {
                            if (GameEntry.Utils.ConnectPairs[connectPair])//如果已经连接就直接跳过
                                return;
                        }
                        var selfNodeData = m_Data.Self.parent.GetComponent<Node>().NodeData;
                        var hitNodeData = hit.transform.parent.GetComponent<Node>().NodeData;
                        if (hitNodeData.NodeType == NodeType.CardPackage)
                            return;
                        m_LineRenderer.SetPosition(1,hit.transform.position);
                        mLineState = LineState.Connect;
                        
                        GameEntry.Utils.dragLine = false;
                        Debug.Log(m_Data.Self.name);
                        Debug.Log(hit.transform);
                        GameEntry.Utils.Lines.Add(new ConnectPair(m_Data.Self, hit.transform), this.m_Data);
                        GameEntry.Utils.ConnectPairs.Add(new ConnectPair(m_Data.Self, hit.transform), true);
                        GameEntry.Utils.ConnectPairs.Add(new ConnectPair(hit.transform, m_Data.Self), true);
                        if (!GameEntry.Utils.LinePairs.ContainsKey(hit.transform))
                        {
                            GameEntry.Utils.LinePairs.Add(hit.transform,true);
                        }
                        
                        Vector2 endPos2 = hit.transform.gameObject.transform.position;
                        Vector2 seVec2 = startPos - endPos2;
                        Vector2 rotateVec2 = new Vector2(-seVec2.y, seVec2.x).normalized;
                        m_PolygonCollider2D.points = new Vector2[]
                        {
                            startPos + rotateVec2 * mWidth,
                            startPos - rotateVec2 * mWidth,
                            endPos - rotateVec2 * mWidth,
                            endPos + rotateVec2 * mWidth,
                        };
                        
                        hitNodeData.NodeState = NodeState.Active;
                        hitNodeData.IsConnect = true;
                        if (!selfNodeData.ChildNodes.Contains(hitNodeData))
                        {
                            selfNodeData.ChildNodes.Add(hitNodeData);
                            GameEntry.Event.FireNow(this,AddChildNodeEventArgs.Create(selfNodeData));
                        }
                        if (!hitNodeData.ParentNodes.Contains(selfNodeData))
                        {
                            hitNodeData.ParentNodes.Add(selfNodeData);
                            GameEntry.Event.FireNow(this,AddParentNodeEventArgs.Create(hitNodeData));
                        }

                        if (hit.transform.childCount >= 2)
                        {
                            hit.transform.GetChild(1).gameObject.SetActive(true);//启动缠绕
                            GameEntry.Sound.PlaySound(10009);
                        }
                        GameEntry.Utils.Blood -= (int)cost;
                        GameEntry.Utils.lineCost = 0;
                        GameEntry.Event.FireNow(this,ShowTipsEventArgs.Create(0,false));
                        GameEntry.Event.FireNow(this,SetRigidbodyTypeEventArgs.Create(RigidbodyType2D.Static));
                    }
                    else
                    {
                        m_Material.color = Color.red;
                    }
                    break;
                case LineState.Connect:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// 获取鼠标坐标
        /// </summary>
        /// <param name="mousePositionInWorld"></param>
        private void GetMousePos(out Vector3 mousePositionInWorld)
        {
            m_MousePositionOnScreen = Input.mousePosition;
            m_MousePositionOnScreen.z = m_DepthZ;
            mousePositionInWorld = Camera.main.ScreenToWorldPoint(m_MousePositionOnScreen);
        }

        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity == null)
            {
                return;
            }
        }

        private void HideLine(object sender, GameEventArgs e)
        {
            HideLineEventArgs ne = (HideLineEventArgs)e;
            if (mLineState == LineState.Connect)
                return;
            GameEntry.Event.FireNow(this,SetRigidbodyTypeEventArgs.Create(RigidbodyType2D.Static));
            GameEntry.Utils.lineCost = 0;
            GameEntry.Event.FireNow(this,ShowTipsEventArgs.Create(0,false));
            GameEntry.Utils.dragLine = false;
            GameEntry.Entity.HideEntity(Entity.Id);
        }

        private void LineValid(object sender, GameEventArgs e)
        {
            LineVaildEventArgs ne = (LineVaildEventArgs)e;
            if (mLineState == LineState.Connect)
                return;
            ClearNodeComponent clear = null;
            if (m_Data.Self.TryGetComponent(out clear))
                return;
            m_LineValid = ne.Valid;
            if (!m_LineValid)
            {
                GameEntry.Sound.PlaySound(10020);
            }
            GameEntry.Event.FireNow(this,ShowTipsEventArgs.Create(1,!m_LineValid));
        }
    }
}
