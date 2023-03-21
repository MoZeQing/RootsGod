using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class ClearNodeComponent : BaseNodeComponent, IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField] private int mCostValue = 0;
        [SerializeField] private GameObject mFrame = null;
        private ComponentData m_Data = null;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private bool m_Follow = false;
        private Vector3 m_MousePositionInWorld = Vector3.zero;
        private SpriteRenderer m_SpriteRenderer = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as ComponentData;
            m_NodeData = m_Data.NodeData;
            GameEntry.Entity.AttachEntity(this, m_NodeData.Id);
            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.sprites[4];
            m_NodeData.NodeType = NodeType.ClearNode;
            m_NodeData.NodeState = NodeState.Active;
            m_NodeData.Select = false;

            mFrame = transform.Find("NodeFrame").gameObject;
            mFrame.SetActive(m_NodeData.Select);
        }
        //private void Start()
        //{
        //    m_NodeData = transform.GetComponent<NodeData>();
        //    m_NodeData.NodeType = NodeType.ClearNode;
        //    m_NodeData.NodeState = NodeState.Active;
        //    m_NodeData.Select = false;
        //    mFrame.SetActive(m_NodeData.Select);
        //    m_NodeData.Costable = false;
        //    m_NodeData.Movable = false;
        //    m_NodeData.Connectable = false;
        //    m_NodeData.Total = 0;
        //    m_NodeData.Income = 0;
        //    m_NodeData.CostPersecond = 1;
        //}

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId, SetSelect);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId, SetSelect);
        }
        //private void OnEnable()
        //{
        //    GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
        //}

        //private void OnDisable()
        //{
        //    GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
        //}

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!m_Follow)
                return;
            GetMousePos(out m_MousePositionInWorld);
            transform.position = m_MousePositionInWorld;
        }
        //private void Update()
        //{
        //    if (!m_Follow)
        //        return;
        //    GetMousePos(out m_MousePositionInWorld);
        //    transform.position = m_MousePositionInWorld;
        //}
        
        private void SetSelect(object sender, GameEventArgs e)
        {
            SetSelectEventArgs ne = (SetSelectEventArgs)e;
            m_NodeData.Select = ne.Select;
            mFrame.SetActive(m_NodeData.Select);
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
                    GameEntry.Event.FireNow(this,SetSelectEventArgs.Create(false));
                    m_NodeData.Select = true;
                    mFrame.SetActive(m_NodeData.Select);
                }
            }
            else
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (!m_Follow)
                    {
                        m_Follow = true;
                    }
                }
                if (!GameEntry.Utils.LinePairs.ContainsKey(transform))
                    return;
                m_Follow = false;
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (GameEntry.Utils.dragLine)
                        return;
                    GameEntry.Sound.PlaySound(10010);
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(),10000,transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (m_Follow)
                {
                    m_Follow = false;
                }
            }
        }
        
        private void GetMousePos(out Vector3 mousePositionInWorld)
        {
            var m_MousePositionOnScreen = Input.mousePosition;
            m_MousePositionOnScreen.z = 10;
            mousePositionInWorld = Camera.main.ScreenToWorldPoint(m_MousePositionOnScreen);
        }
    }
}