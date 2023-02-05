using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace GameMain
{
    public class CardPackageNodeComponent : BaseNodeComponent, IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField] private int mDepth = 4;
        [SerializeField] private GameObject mFrame = null;
        [SerializeField] private GameObject[] mPos = null;
        [SerializeField] private int mDrawNum = 3;
        [SerializeField] private float mCardMoveTime = 0.5f;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private Vector3 m_MousePositionInWorld = Vector3.zero;
        private bool m_Follow = false;
        private void Start()
        {
            m_NodeData = transform.GetComponent<NodeData>();
            m_NodeData.NodeType = NodeType.ClearNode;
            m_NodeData.NodeState = NodeState.Active;
            m_NodeData.Select = false;
            mFrame.SetActive(m_NodeData.Select);
            m_NodeData.Costable = false;
            m_NodeData.Movable = false;
            m_NodeData.Connectable = false;
            m_NodeData.Total = 0;
            m_NodeData.Income = 0;
            m_NodeData.CostPersecond = 1;
        }
        
        private void OnEnable()
        {
            GameEntry.Event.Subscribe(SetSelectEventArgs.EventId,SetSelect);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId,SetSelect);
        }

        private void Update()
        {
            if (!m_Follow)
                return;
            GetMousePos(out m_MousePositionInWorld);
            transform.parent.position = m_MousePositionInWorld;
        }
        
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

                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    transform.parent.GetChild(1).gameObject.SetActive(true);
                    ShowNode();
                }
            }
        }

        private void ShowNode()
        {
            for (int i = 0; i < mDrawNum; i++)
            {
                var randomNum = Random.Range(1, GameEntry.Utils.depth + 1);
                var entity = Instantiate(GameEntry.Utils.nodes[randomNum], transform.position,
                    Quaternion.Euler(0, 0, 0));
                entity.transform.DOMoveX(mPos[i].transform.position.x,mCardMoveTime);
                entity.transform.DOMoveY(mPos[i].transform.position.y,mCardMoveTime);
            }
            transform.gameObject.SetActive(false);  
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