using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameFramework.Event;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 树根节点
/// </summary>
namespace GameMain
{
    public class TreeNode : BaseNodeComponent, IPointerDownHandler
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
        private Rigidbody2D m_Rigidbody2D = null;
        private bool m_IsAdd = false;
        private bool m_IsFire = false;
        private SpriteRenderer m_SpriteRenderer = null;
        private Color32 m_Color32 = new Color32(176, 176, 176, 255);

        [SerializeField] private float mCD=2f;//生成节点的方法
        private bool mAlive = false;//节点是否正在运行
        private List<Vector3> m_leafNodes = new List<Vector3>();

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as ComponentData;
            m_NodeData = m_Data.NodeData;
            GameEntry.Entity.AttachEntity(this, m_NodeData.Id);

            m_SpriteRenderer = transform.GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = GameEntry.Utils.sprites[9];
            m_SpriteRenderer.color = Color.red;

            m_Rigidbody2D = transform.GetComponent<Rigidbody2D>();
            m_NodeData.NodeType = NodeType.TreeNode;
            m_NodeData.Select = false;

            mFrame = transform.Find("NodeFrame").gameObject;
            mFrame.SetActive(m_NodeData.Select);

            mProgress = transform.Find("jindutiao1").gameObject;
            mProgress.SetActive(true);

            m_NodeData.NodeState = NodeState.Active;
            m_NodeData.Costable = false;
            m_NodeData.Movable = false;
            m_NodeData.Connectable = true;
            m_NodeData.Total = mTotal;
            m_NodeData.Income = mIncome;
            m_NodeData.CostPersecond = mCostPerSecond;
            m_IsAdd = false;
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

        private float _time = 0f;

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            Vector3 playerPos = Vector3.zero;/*GameObject.FindWithTag("Player").GetComponent<Transform>().position;*/
            if (m_NodeData.NodeState == NodeState.Active)
            {
                //mProgress.transform.SetLocalScaleX(1 - (1 - _time / mCD));
                if (_time > Time.time)
                    return;
                _time = Time.time + mCD;
                //存活状态下开始生成子枝干
                //生成的逻辑：获取player的位置，以自身为圆心生成一条线，在随机的距离内决定一个交点，然后在这个交点生成一个节点，并加入列表，每次生成都会在列表中寻找最远的节点
                Vector3 pos = new Vector3();
                if (m_leafNodes.Count != 0)
                    pos = m_leafNodes[m_leafNodes.Count - 1];
                else
                    pos = transform.position;
                float angle = UnityEngine.Random.Range(0, 180);
                float length = UnityEngine.Random.Range(1f, 3f);
                Vector3 newPos1 = pos + new Vector3(length * Mathf.Sin(angle), length * Mathf.Cos(angle), 0);
                Vector3 newPos2 = pos - new Vector3(length * Mathf.Sin(angle), length * Mathf.Cos(angle), 0);
                Debug.Log(pos);
                Debug.Log(new Vector3(length * Mathf.Sin(angle), length * Mathf.Cos(angle), 0));
                Vector3 newPos = (newPos1 - playerPos).magnitude < (newPos2 - playerPos).magnitude ? newPos1 : newPos2;
                int id = GameEntry.Entity.GenerateSerialId();
                GameEntry.Entity.ShowNode(new NodeData(id, 10001, NodeType.LeafNode, NodeState.Active)
                {
                    Position = newPos,
                    Scale = new Vector3(0.4f, 0.4f, 1f)
                });
                m_leafNodes.Add(newPos);
                Debug.Log("生成树叶");
            }
            else if (m_NodeData.NodeState == NodeState.InActive)
            {
                m_SpriteRenderer.color = Color.gray;
            }

            //Debug.Log(m_NodeData.Total);
            //if (!m_IsAdd)
            //{
            //    GameEntry.Event.FireNow(this, AddIncomeEventArgs.Create(m_NodeData.Income));
            //    m_IsAdd = true;
            //}
            //if (m_NodeData.Total < m_NodeData.Income)
            //{
            //    if (!m_IsFire)
            //    {
            //        GameEntry.Event.FireNow(this, AddIncomeEventArgs.Create(-m_NodeData.Income));
            //        m_IsFire = true;
            //    }
            //    return;
            //}
            //m_NodeData.Total -= m_NodeData.Income * Time.deltaTime;
        }

        private void AddParent(object sender, GameEventArgs e)
        {
            AddParentNodeEventArgs ne = (AddParentNodeEventArgs)e;
            if (ne.NodeData != m_NodeData)
                return;
            for (int i = 0; i < m_NodeData.ParentNodes.Count; i++)
            {
                if (m_NodeData.ChildNodes.Contains(m_NodeData.ParentNodes[i]))
                    return;
                if (m_NodeData.ParentNodes[i].NodeType == NodeType.ClearNode)
                {
                    m_NodeData.NodeState = NodeState.InActive;
                }
            }

        }

        private void AddChild(object sender, GameEventArgs e)
        {
            AddChildNodeEventArgs ne = (AddChildNodeEventArgs)e;
            if (ne.NodeData != m_NodeData)
                return;
            for (int i = 0; i < m_NodeData.ChildNodes.Count; i++)
            {
                if (m_NodeData.ParentNodes.Contains(m_NodeData.ChildNodes[i]))
                    return;
                if (m_NodeData.ChildNodes[i].NodeType == NodeType.ClearNode)
                {
                    m_NodeData.NodeState = NodeState.InActive;
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
                    GameEntry.Event.FireNow(this, SetSelectEventArgs.Create(false));
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
                    var lineData = new LineData(GameEntry.Entity.GenerateSerialId(), 10000, transform);
                    GameEntry.Entity.ShowLine(lineData);
                }
            }
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
