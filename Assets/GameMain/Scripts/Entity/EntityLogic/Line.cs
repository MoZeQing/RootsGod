using System;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
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
        private LineData m_Data = null;
        private LineRenderer m_LineRenderer = null;
        private Material m_Material = null;

        [Header("Mouse")]
        private float m_DepthZ = 10;
        private Vector3 m_MousePositionOnScreen = Vector3.zero;
        private Vector3 m_MousePositionInWorld = Vector3.zero; 
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameEntry.Event.Subscribe(HideLineEventArgs.EventId,HideLine);
            m_Data = userData as LineData;
            if (m_Data == null)
            {
                Log.Error("LineData object data is invalid.");
                return;
            }
            mLineState = LineState.NotConnect;
            m_LineRenderer = transform.GetComponent<LineRenderer>();
            m_Material = m_LineRenderer.material;
            m_LineRenderer.SetPosition(0,m_Data.Self.position);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(HideLineEventArgs.EventId,HideLine);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            switch (mLineState)
            {
                case LineState.Undefined:
                    break;
                case LineState.NotConnect:
                    GetMousePos(out m_MousePositionInWorld);
                    m_LineRenderer.SetPosition(1,m_MousePositionInWorld);
                    var distance = Vector3.Distance(m_LineRenderer.GetPosition(0),
                        m_LineRenderer.GetPosition(1));
                    Debug.Log(distance);
                    var cost = (Math.Round(distance,1)) * UCS.BloodPerUnit;
                    if (GameEntry.Utils.Blood >= cost)
                    {
                        m_Material.color = Color.white;
                        if (!Input.GetMouseButtonDown(0))
                            return;
                        RaycastHit2D hit = Physics2D.Raycast(m_MousePositionInWorld, Vector2.zero,
                            Mathf.Infinity,LayerMask.GetMask("Node"));
                        if (!hit)
                            return;
                        if (hit.collider.transform.position == m_Data.Self.position)
                            return;
                        var connectPair = new ConnectPair(m_Data.Self, hit.transform);
                        if (GameEntry.Utils.ConnectPairs.ContainsKey(connectPair))
                        {
                            if (GameEntry.Utils.ConnectPairs[connectPair])
                                return;
                        }
                        m_LineRenderer.SetPosition(1,hit.transform.position);
                        mLineState = LineState.Connect;
                        GameEntry.Utils.ConnectPairs.Add(new ConnectPair(m_Data.Self, hit.transform), true);
                        GameEntry.Utils.ConnectPairs.Add(new ConnectPair(hit.transform, m_Data.Self), true);
                        if (!GameEntry.Utils.LinePairs.ContainsKey(hit.transform))
                        {
                            GameEntry.Utils.LinePairs.Add(hit.transform,true);
                        }
                        GameEntry.Utils.Blood -= (int)cost;
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
            GameEntry.Entity.HideEntity(Entity.Id);
        }
    }
}
