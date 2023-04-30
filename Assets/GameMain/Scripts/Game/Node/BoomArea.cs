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

        protected float m_nowTime = 0f;
        protected float m_time = 1f;//爆炸读条的时间

        private void Start()
        {
            //重新调整进度条的位置

            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_animator = GetComponent<Animator>();

            m_animator.Play("Smoke");
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        private void OnEnable()
        {
            m_nowTime = Time.time;
        }

        private void Update()
        {
            if (!this.gameObject.activeSelf)
                return;
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

