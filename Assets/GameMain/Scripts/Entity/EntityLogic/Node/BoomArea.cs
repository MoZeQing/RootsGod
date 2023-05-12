using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 爆炸动画以及碰撞检测
    /// </summary>
    public class BoomArea : Entity
    {
        [SerializeField] private SpriteRenderer m_spriteRenderer;
        [SerializeField]private Animator m_animator;
        private ComponentData m_Data = null;
        public NodeData NodeData = null;
        private bool isBoom = false;
        [SerializeField] private GameObject mBoomProgress = null;
        private int m_Component;

        public bool IsBoom { get; set; }

        private float m_nowTime = 0f;
        private float m_boomTime = 1f;//爆炸读条的时间

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            BoomAreaDara boomAreaDara = (BoomAreaDara)userData;
            m_Component = boomAreaDara.OwnerId;

            m_boomTime = boomAreaDara.BoomTime;

            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_animator = GetComponent<Animator>();

            m_animator.Play("Smoke");

            mBoomProgress = transform.Find("boomjingdutiao1").gameObject;
            mBoomProgress.SetActive(true);
        }

        private void OnEnable()
        {
            m_nowTime = Time.time;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (IsBoom)
            {
                mBoomProgress.SetActive(true);
                mBoomProgress.transform.SetLocalScaleX(m_nowTime / m_boomTime);//
                Debug.Log(m_nowTime / m_boomTime);
                m_nowTime -= Time.deltaTime;
                if (m_nowTime <= 0)
                {
                    IsBoom = false;
                    //生成爆炸动画
                    BaseNodeComponent component = GameEntry.Entity.GetEntity(m_Component).GetComponent<BaseNodeComponent>();
                    //mBoom
                    component.RemoveNode();
                    //播放爆炸动画
                    component.SetConnect(false);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            BaseNodeComponent node = null;
            //播放爆炸动画
            if (collision.TryGetComponent<BaseNodeComponent>(out node))
            {
                node.RemoveNode();
                node.SetConnect(false);
            }
        }
    }
}

