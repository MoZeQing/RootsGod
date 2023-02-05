
using UnityEngine;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class MenuForm : UGuiForm
    {
        private ProcedureMenu m_ProcedureMenu = null;
        [SerializeField] private PlayableDirector m_Director = null;
        [SerializeField] private GameObject m_Director2 = null;
        [SerializeField] private GameObject mDeveloper = null;
        private bool m_IsOver = false;
        public void OnStartButtonClick()
        {
            GameEntry.Sound.PlaySound(10014);
            m_ProcedureMenu.StartGame();
        }
        
        public void OnQuitButtonClick()
        {
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
        }

        public void OnDeveloperButtonClick()
        {
            GameEntry.Sound.PlaySound(10014);
            if (mDeveloper.activeSelf)
            {
                mDeveloper.SetActive(false);
            }
            else
            {
                mDeveloper.SetActive(true);
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureMenu = (ProcedureMenu)userData;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
            m_IsOver = false;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            m_ProcedureMenu = null;
            base.OnClose(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_IsOver)
            {
                m_Director.gameObject.SetActive(false);
                return;
            }
            if (m_Director.time >= m_Director.duration - 0.1f)
            {
                m_IsOver = true;
                GameEntry.Utils.first = false;
                m_Director2.SetActive(true);
            }
        }
    }
}
