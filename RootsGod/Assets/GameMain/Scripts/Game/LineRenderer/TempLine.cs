using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
    public class TempLine : MonoBehaviour
    {
        [SerializeField] private float mNodeZ = 0;
        private Vector3 m_ScreenPosition;
        private Vector3 m_MousePositionOnScreen;
        private Vector3 m_MousePositionInWorld;
        private LineRenderer m_LineRenderer = null;

        private void Start()
        {
            m_LineRenderer = transform.GetComponent<LineRenderer>();
        }

        private void Update()
        {
            MouseFollow();
        }

        public void MouseFollow()
        {
            if (Camera.main == null) 
                return;
            m_ScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            m_MousePositionOnScreen = Input.mousePosition;
            m_MousePositionOnScreen.z = m_ScreenPosition.z;
            m_MousePositionInWorld = Camera.main.ScreenToWorldPoint(m_MousePositionOnScreen);
        }
    }
}