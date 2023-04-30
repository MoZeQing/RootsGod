using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMain
{
    public class Level1NodeComponent : BaseNodeComponent, IPointerDownHandler
    {
        [SerializeField] private int mTotal = 0;
        [SerializeField] private int mIncome = 0;
        [SerializeField] private int mCostPerSecond = 0;
        [SerializeField] private GameObject mFrame = null;
        [SerializeField] private GameObject mProgress = null;
        [SerializeField] private GameObject mBoomProgress = null;
        [SerializeField] private float mLerpTime = 0.5f;
<<<<<<< HEAD
<<<<<<< HEAD
        private ComponentData m_Data = null;
=======
>>>>>>> parent of 4c875e3a (Update Creep)
=======
>>>>>>> parent of 4c875e3a (Update Creep)
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private Rigidbody2D m_Rigidbody2D = null;
        private bool m_IsAdd = false;
        private float mboomTime = 2f;
        private float m_boomTime = 2f;
        private float mboomSpeed = 1f;
        private bool m_IsFire = false;
        private SpriteRenderer m_SpriteRenderer = null;
        private Color32 m_Color32 = new Color32(176, 176, 176, 255);

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as ComponentData;
            m_NodeData = m_Data.NodeData;
            GameEntry.Entity.AttachEntity(this, m_NodeData.Id);
            transform.parent.GetComponent<Node>().Component = this;

            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.sprites[0];
            m_SpriteRenderer.color = Color.red;

            m_Rigidbody2D = transform.GetComponent<Rigidbody2D>();
            m_NodeData.NodeType = NodeType.Level1Node;
            m_NodeData.NodeState = NodeState.InActive;
            m_NodeData.Select = false;

            mFrame = transform.Find("NodeFrame").gameObject;
            mFrame.SetActive(m_NodeData.Select);

            mProgress = transform.Find("jingdutiao1").gameObject;
            mProgress.SetActive(m_NodeData.Select);

            mBoomProgress = transform.Find("boomjingdutiao1").gameObject;
            mBoomProgress.SetActive(m_NodeData.Select);

            m_NodeData.Costable = false;
            m_NodeData.Movable = false;
            m_NodeData.Connectable = true;
            m_NodeData.Total = mTotal;
            m_NodeData.Income = mIncome;
            m_NodeData.CostPersecond = mCostPerSecond;
            m_IsAdd = false;
            m_IsDead = false;
            m_ZoneScale = 0;

            IsConnect = true;
            IsBoom = false;
        }

        //private void Start()
        //{
        //    m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
        //    m_SpriteRenderer.color = Color.red;
        //    m_Rigidbody2D = transform.GetComponent<Rigidbody2D>();
        //    m_NodeData = transform.GetComponent<NodeData>();
        //    m_NodeData.NodeType = NodeType.Level1Node;
        //    m_NodeData.NodeState = NodeState.InActive;
        //    m_NodeData.Select = false;
        //    mFrame.SetActive(m_NodeData.Select);
        //    m_NodeData.Costable = false;
        //    m_NodeData.Movable = false;
        //    m_NodeData.Connectable = true;
        //    m_NodeData.Total = mTotal;
        //    m_NodeData.Income = mIncome;
        //    m_NodeData.CostPersecond = mCostPerSecond;
        //    m_IsAdd = false;
        //}

        //protected override void OnShow(object userData)
        //{
        //    base.OnShow(userData);
        //    GameEntry.Event.Subscribe(SetSelectEventArgs.EventId, SetSelect);
        //    GameEntry.Event.Subscribe(SetRigidbodyTypeEventArgs.EventId, SetRigidType);
        //}

        //protected override void OnHide(bool isShutdown, object userData)
        //{
        //    base.OnHide(isShutdown, userData);
        //    GameEntry.Event.Unsubscribe(SetSelectEventArgs.EventId, SetSelect);
        //    GameEntry.Event.Unsubscribe(SetRigidbodyTypeEventArgs.EventId, SetRigidType);
        //}

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

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            // if (m_NodeData.IsPhysic)
            // {
            //     m_NodeData.Connectable = false;
            //     if (m_Rigidbody2D.velocity.magnitude <= 0.2f)
            //     {
            //         Invoke(nameof(SetRigid),0.5f);
            //         m_NodeData.IsPhysic = false;
            //     }
            // }
            if (!IsConnect)
            {
                return;
            }
            if (IsBoom)
            {
                mBoomProgress.SetActive(true);
                mBoomProgress.transform.SetLocalScaleX(mboomTime / m_boomTime);//
                Debug.Log(mboomTime / m_boomTime);
                mboomTime -= mboomSpeed * Time.deltaTime;
                if (mboomTime <= 0)
                {
                    IsConnect = false;
                    IsBoom = false;
                    //���ɱ�ը����
                    GameEntry.Entity.ShowAreaBoom(new ComponentData(GameEntry.Entity.GenerateSerialId(), 10003, this.Id, m_NodeData)
                    {
                        Position = this.transform.position,
                        Scale = new Vector3(2f, 2f, 1f)
                    });
                    //mBoom
                    RemoveNode();
                    //���ű�ը����
                    SetConnect(false);
                }
            }
            if (m_NodeData.Total <= 0)
            {
                m_NodeData.Total = 0;
                m_SpriteRenderer.DOColor(m_Color32, mLerpTime);
                m_NodeData.NodeState = NodeState.InActive;
                return;
            }
            m_NodeData.Total -= m_NodeData.CostPersecond * Time.deltaTime;
            mProgress.transform.SetLocalScaleX(1 - (1 - m_NodeData.Total / mTotal));
            //Debug.Log(m_NodeData.Total);
            if (m_NodeData.NodeState != NodeState.Active)
                return;
            m_NodeData.NodeType = NodeType.BlockingNode;
            if (!m_IsAdd)
            {
                GameEntry.Event.FireNow(this, AddIncomeEventArgs.Create(m_NodeData.Income));
                m_IsAdd = true;
            }
            if (m_NodeData.Total < m_NodeData.Income)
            {
                if (!m_IsFire)
                {
                    GameEntry.Event.FireNow(this, AddIncomeEventArgs.Create(-m_NodeData.Income));
                    m_IsFire = true;
                }
                return;
            }
            m_NodeData.Total -= m_NodeData.Income * Time.deltaTime;
        }

        //private void Update()
        //{
        //    // if (m_NodeData.IsPhysic)
        //    // {
        //    //     m_NodeData.Connectable = false;
        //    //     if (m_Rigidbody2D.velocity.magnitude <= 0.2f)
        //    //     {
        //    //         Invoke(nameof(SetRigid),0.5f);
        //    //         m_NodeData.IsPhysic = false;
        //    //     }
        //    // }
            
        //    if (m_NodeData.Total <= 0)
        //    {
        //        m_NodeData.Total = 0;
        //        m_SpriteRenderer.DOColor(m_Color32,mLerpTime);
        //        m_NodeData.NodeState = NodeState.InActive;
        //        return;
        //    }
        //    m_NodeData.Total -= m_NodeData.CostPersecond * Time.deltaTime;
        //    mProgress.transform.SetLocalScaleX(1-(1 - m_NodeData.Total / mTotal));
        //    //Debug.Log(m_NodeData.Total);
        //    if (m_NodeData.NodeState != NodeState.Active)
        //        return;
        //    m_NodeData.NodeType = NodeType.BlockingNode;
        //    if (!m_IsAdd)
        //    {
        //        GameEntry.Event.FireNow(this,AddIncomeEventArgs.Create(m_NodeData.Income));
        //        m_IsAdd = true;
        //    }
        //    if (m_NodeData.Total < m_NodeData.Income)
        //    {
        //        if (!m_IsFire)
        //        {
        //            GameEntry.Event.FireNow(this,AddIncomeEventArgs.Create(-m_NodeData.Income));
        //            m_IsFire = true;
        //        }
        //        return;
        //    }
        //    m_NodeData.Total -= m_NodeData.Income * Time.deltaTime;
        //}
        
        private void SetSelect(object sender, GameEventArgs e)
        {
            SetSelectEventArgs ne = (SetSelectEventArgs)e;
            m_NodeData.Select = ne.Select;
            mFrame.SetActive(m_NodeData.Select);
        }
        
        private void SetRigidType(object sender,GameEventArgs e)
        {
            SetRigidbodyTypeEventArgs ne = (SetRigidbodyTypeEventArgs)e;
            m_Rigidbody2D.bodyType = ne.Type;
        }

        
        private void SetRigid()
        {
            m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
            m_NodeData.Connectable = true;
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
                if (!GameEntry.Utils.LinePairs.ContainsKey(transform))
                    return;
                if (!m_NodeData.Connectable)
                    return;
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (GameEntry.Utils.dragLine)
                        return;
                    m_Rigidbody2D.bodyType = RigidbodyType2D.Static;
                    GameEntry.Sound.PlaySound(10010);
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(),10000,transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
        }

        public override void RemoveNode()
        {
            foreach (NodeData parent in m_NodeData.ParentNodes)
            {
                if (GameEntry.Utils.Lines.ContainsKey(new ConnectPair(GameEntry.Entity.GetEntity(parent.Id).transform, transform.parent)))
                {
                    GameEntry.Entity.GetEntity(GameEntry.Utils.Lines[new ConnectPair(GameEntry.Entity.GetEntity(parent.Id).transform, transform.parent)].Id).gameObject.SetActive(false);
                }
                parent.ChildNodes.Remove(m_NodeData);
            }
            foreach (NodeData child in m_NodeData.ChildNodes)
            {
                if (GameEntry.Utils.Lines.ContainsKey(new ConnectPair(transform.parent, GameEntry.Entity.GetEntity(child.Id).transform)))
                {
                    GameEntry.Entity.GetEntity(GameEntry.Utils.Lines[new ConnectPair(transform.parent, GameEntry.Entity.GetEntity(child.Id).transform)].Id).gameObject.SetActive(false);
                }
                child.ParentNodes.Remove(m_NodeData);
                Node node = GameEntry.Entity.GetEntity(child.Id).GetComponent<Node>();
                node.Component.SetConnect(false);
            }
            this.gameObject.SetActive(false);
        }

        public override void SetConnect(bool flag)
        {
            foreach (NodeData parent in m_NodeData.ParentNodes)
            {
                if (GameEntry.Entity.GetEntity(parent.Id).GetComponent<Node>().Component.IsConnect)
                {
                    flag = true;
                    break;
                }
            }
            IsConnect = flag;
            foreach (NodeData child in m_NodeData.ChildNodes)
            {
                GameEntry.Entity.GetEntity(child.Id).GetComponent<Node>().Component.SetConnect(flag);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this,LineVaildEventArgs.Create(false));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Line line = null;
            if (!other.TryGetComponent(out line))
                return;
            if (m_NodeData.NodeState == NodeState.InActive)
                return;
            GameEntry.Event.FireNow(this,LineVaildEventArgs.Create(true));
        }
    }
}