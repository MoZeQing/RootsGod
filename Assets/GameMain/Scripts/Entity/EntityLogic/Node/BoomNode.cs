using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using UnityEngine.EventSystems;
using System;

namespace GameMain
{
    public class BoomNode : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mTotal = 0;
        [SerializeField] private int mIncome = 0;
        [SerializeField] private int mCostPerSecond = 0;
        [SerializeField] private GameObject mFrame = null;
        [SerializeField] private GameObject mProgress = null;
        [SerializeField] private float mLerpTime = 0.5f;
        private ComponentData m_Data = null;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private bool m_IsAdd = false;
        private SpriteRenderer m_SpriteRenderer = null;
        private CircleCollider2D m_circleCollider2D = null;
        private Rigidbody2D m_Rigidbody2D = null;
        private Color32 m_Color32 = new Color32(176, 176, 176, 255);
        private float m_cd;//¶¾±¬³æ³²µÄCD

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as ComponentData;
            m_NodeData = m_Data.NodeData;
            GameEntry.Entity.AttachEntity(this, m_NodeData.Id);

            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.sprites[10];//¸ü¸Ä
            m_SpriteRenderer.color = Color.red;
            m_Rigidbody2D = transform.GetComponent<Rigidbody2D>();
            m_NodeData.NodeType = NodeType.Level1Node;
            m_NodeData.NodeState = NodeState.InActive;
            m_NodeData.Select = false;

            mFrame = transform.Find("NodeFrame").gameObject;
            mFrame.SetActive(m_NodeData.Select);

            mProgress = transform.Find("jindutiao1").gameObject;
            mProgress.SetActive(true);

            m_NodeData.Costable = false;
            m_NodeData.Movable = false;
            m_NodeData.Connectable = true;
            m_NodeData.Total = mTotal;
            m_NodeData.Income = mIncome;
            m_NodeData.CostPersecond = mCostPerSecond;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!IsConnect)
            {
                return;
            }
            foreach (NodeData node in m_NodeData.ChildNodes)
            {
                BaseNodeComponent component = GameEntry.Entity.GetEntity(node.Id).GetComponent<Node>().Component;
                if (component.IsConnect)
                {

                }
            }
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId, SetSelect);
            GameEntry.Event.Subscribe(SetRigidbodyTypeEventArgs.EventId, SetRigidType);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId, SetSelect);
            GameEntry.Event.Unsubscribe(SetRigidbodyTypeEventArgs.EventId, SetRigidType);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            switch (m_NodeData.NodeState)
            {
                case NodeState.Unknown:
                    break;
                case NodeState.InActive:
                    break;
                case NodeState.Active:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!m_NodeData.Select)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GameEntry.Event.FireNow(this, SetSelectEventArgs.Create(false));
                    m_NodeData.Select = true;
                    mFrame.SetActive(m_NodeData.Select);
                }
            }
            else
            {
                if (!GameEntry.Utils.LinePairs.ContainsKey(transform))
                    return;
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (GameEntry.Utils.dragLine)
                        return;
                    m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
                    GameEntry.Sound.PlaySound(10010);
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(), 10000, transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }
        private void SetSelect(object sender, GameEventArgs e)
        {
            SetSelectEventArgs ne = (SetSelectEventArgs)e;
            m_NodeData.Select = ne.Select;
            mFrame.SetActive(m_NodeData.Select);
        }

        private void SetRigidType(object sender, GameEventArgs e)
        {
            SetRigidbodyTypeEventArgs ne = (SetRigidbodyTypeEventArgs)e;
            m_Rigidbody2D.bodyType = ne.Type;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this, LineVaildEventArgs.Create(false));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this, LineVaildEventArgs.Create(true));
        }
    }
}

