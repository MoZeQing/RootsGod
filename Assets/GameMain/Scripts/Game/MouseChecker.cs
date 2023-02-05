using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
   public class MouseChecker : MonoBehaviour
   {
      [SerializeField] private float mDepthZ = 0;
      [SerializeField] private float mSpeed = 0.1f;
      [SerializeField] private float mLeft = 30;
      [SerializeField] private float mRight = 1890;
      [SerializeField] private float mUp = 1060;
      [SerializeField] private float mDown = 20;
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
            else
            {
               GameEntry.Sound.PlaySound(10003);
            }

            GameEntry.Sound.PlaySound(10007);
         }

         if (Input.GetMouseButtonDown(1))
         {
            GameEntry.Event.FireNow(this,HideLineEventArgs.Create());
            GameEntry.Sound.PlaySound(10012);
         }
         
         Debug.Log(Input.mousePosition);
         if (Input.mousePosition.x / 1920 * Screen.width < mLeft)
         {
            if (Camera.main.transform.position.x < -5.5f)
            {
               return;
            }
            Camera.main.transform.position -= new Vector3(1, 0, 0) * mSpeed;
         }

         if (Input.mousePosition.x / 1920 * Screen.width > mRight)
         {
            if (Camera.main.transform.position.x > 6.5f)
            {
               return;
            }
            Camera.main.transform.position += new Vector3(1, 0, 0) * mSpeed;
         }
         
         if (Input.mousePosition.y / 1080 * Screen.height < mDown)
         {
            if (Camera.main.transform.position.y < -5f )
            {
               return;
            }
            Camera.main.transform.position -= new Vector3(0, 1, 0) * mSpeed;
         }

         if (Input.mousePosition.y / 1080 * Screen.height > mUp)
         {
            if (Camera.main.transform.position.y> 3.4f)
            {
               return;
            }
            Camera.main.transform.position += new Vector3(0, 1, 0) * mSpeed;
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