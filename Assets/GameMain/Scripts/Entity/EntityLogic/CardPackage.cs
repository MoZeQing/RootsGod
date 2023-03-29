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
    public class CardPackage : Entity, IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField] private GameObject mFrame = null;
        [SerializeField] private GameObject[] mPos = null;
        [SerializeField] private int mDrawNum = 3;
        [SerializeField] private float mCardMoveTime = 0.5f;
        private CardPackageData m_Data = null;
        private NodeData m_NodeData = null;
        private List<BaseNodeComponent> m_ParentNodes = new List<BaseNodeComponent>();
        private Vector3 m_MousePositionInWorld = Vector3.zero;
        private bool m_Follow = false;
        private SpriteRenderer m_SpriteRenderer = null;
        private bool Select = false;

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

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!m_Follow)
                return;
            GetMousePos(out m_MousePositionInWorld);
            transform.parent.position = m_MousePositionInWorld;
        }

        private void SetSelect(object sender, GameEventArgs e)
        {
            SetSelectEventArgs ne = (SetSelectEventArgs)e;
            Select = ne.Select;
            //mFrame.SetActive(Select);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Select)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    GameEntry.Event.FireNow(this, SetSelectEventArgs.Create(false));
                    Select = true;
                    //mFrame.SetActive(Select);
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
                Debug.Log("第" + i + "抽");
                var randomNum = GameEntry.Utils.rand(GameEntry.Utils.normalPoolDic, 100);
                var entity = Instantiate(GameEntry.Utils.nodes[randomNum], transform.position,
                    Quaternion.Euler(0, 0, 0));
                var rigid2D = entity.GetComponent<Rigidbody2D>();
                var entityCollider = entity.GetComponent<BoxCollider2D>();
                //collider.enabled = false;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(entity.transform.DOMoveX(mPos[i].transform.position.x, mCardMoveTime));
                sequence.Insert(0, entity.transform.DOMoveY(mPos[i].transform.position.y, mCardMoveTime));
            }
            transform.gameObject.SetActive(false);
        }

        /// <summary>
        /// No Used
        /// </summary>
        /// <param name="depth1"></param>
        /// <param name="depth2"></param>
        /// <returns></returns>
        private int GetNode(int depth1, int depth2)
        {
            int randomNum = 0;
            Debug.Log("卡池1深度:" + depth1 + "| 卡池2深度:" + depth2);
            if (depth1 == 0 && depth2 == 0)
            {
                Debug.LogError("Invalid Num");
                return 0;
            }

            if (depth1 == 0 && depth2 != 0)
            {
                randomNum = Random.Range(0, depth2);
                Debug.Log("随机范围:" + 0 + "-" + (depth2 - 1));
                switch (randomNum)
                {
                    case 0:
                        randomNum = 0;
                        Debug.Log("随机数:" + randomNum);
                        return randomNum;
                    case 1:
                        randomNum = 3;
                        Debug.Log("随机数:" + randomNum);
                        return randomNum;
                    default:
                        return 0;
                }
                return randomNum;
            }

            if (depth1 != 0 && depth2 == 0)
            {
                randomNum = Random.Range(0, depth1);
                Debug.Log("随机范围:" + 0 + "-" + (depth1 - 1));
                Debug.Log("随机数:" + randomNum);
                return randomNum;
            }
            var depth = depth1 + depth2;
            Debug.Log("随机范围:" + 0 + "-" + (depth1 - 1));
            switch (depth2)
            {
                case 1:
                    randomNum = Random.Range(0, depth);
                    if (randomNum == depth - 1)
                        randomNum = 0;
                    Debug.Log("随机数:" + randomNum);
                    return randomNum;
                case 2:
                    randomNum = Random.Range(0, depth);
                    if (randomNum == depth - 1)
                        randomNum = 3;
                    if (randomNum == depth - 2)
                        randomNum = 0;
                    Debug.Log("随机数:" + randomNum);
                    return randomNum;
                default:
                    return 0;
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