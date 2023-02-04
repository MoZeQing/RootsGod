using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
   public class MouseChecker : MonoBehaviour
   {
      [SerializeField] private float mDepthZ = 0;
      private Vector3 m_MousePositionOnScreen = Vector3.zero;
      private Vector3 m_MousePositionInWorld = Vector3.zero; 

      private void Update()
      {
         if (Input.GetMouseButtonDown(0))
         {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            m_MousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(m_MousePositionInWorld, Vector2.zero);
            if (!hit)
            {
               GameEntry.Event.FireNow(this, SetSelectEventArgs.Create(false));
            }
         }

         if (Input.GetMouseButtonDown(1))
         {
            GameEntry.Event.FireNow(this,HideLineEventArgs.Create());
         }
      }

      private void GetMousePos(out Vector3 mousePositionInWorld)
      {
         m_MousePositionOnScreen = Input.mousePosition;
         m_MousePositionOnScreen.z = mDepthZ;
         mousePositionInWorld = Camera.main.ScreenToWorldPoint(m_MousePositionOnScreen);
      }
   }
}