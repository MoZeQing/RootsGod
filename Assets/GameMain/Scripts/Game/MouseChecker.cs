using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMain
{
   public class MouseChecker : MonoBehaviour
   {
      [SerializeField] private float mDepthZ = 0;
      // [SerializeField] private float mSpeed = 0.1f;
      // [SerializeField] private float mLeft = 30;
      // [SerializeField] private float mRight = 1890;
      // [SerializeField] private float mUp = 1060;
      // [SerializeField] private float mDown = 20;
      [SerializeField] private float mScrollSpeed = 6;
      [SerializeField] private float mScrollLerp = 0.5f;
      [SerializeField] private float mMouseSpeed = 0.5f;
      [SerializeField] private Vector4 mCameraLimit = new Vector4(-5.5f,6.5f,-5f,3.4f);

      private float m_StartScroll = 0;
      private float m_Scroll = 0;
      private Vector3 m_MousePositionOnScreen = Vector3.zero;
      private Vector3 m_MousePositionInWorld = Vector3.zero;
      private bool m_Move = false;
      private Vector3 m_BasePos = Vector3.zero;
      private Vector2 m_BaseMousePos = Vector2.zero;
      private void Start()
      {
         m_StartScroll = Camera.main.fieldOfView;
      }

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
         
         SetCameraView();
         CheckRightClick();
         CameraMove2();
      }

      private void CheckRightClick()
      {
         if (Input.GetMouseButtonDown(1))
         {
            GameEntry.Event.FireNow(this,HideLineEventArgs.Create());
            GameEntry.Sound.PlaySound(10012);
            m_MousePositionInWorld.z = 0;
            m_BasePos = Camera.main.transform.position;
            m_BaseMousePos = new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y"));
            m_Move = true;
         }

         if (Input.GetMouseButtonUp(1))
         {
            m_Move = false;
         }
      }
      
      private void CameraMove2()
      {
         if (!m_Move)
            return;
         var mouseDragX = Camera.main.transform.right * (Input.GetAxisRaw("Mouse X") - m_BaseMousePos.x) * mMouseSpeed;
         var mouseDragY = Camera.main.transform.up * (Input.GetAxisRaw("Mouse Y") - m_BaseMousePos.y) * mMouseSpeed;
         Camera.main.transform.position -= mouseDragX + mouseDragY;
         
         var posX = Mathf.Clamp(Camera.main.transform.position.x, mCameraLimit.x, mCameraLimit.y);
         var posY = Mathf.Clamp(Camera.main.transform.position.y, mCameraLimit.z, mCameraLimit.w);
         Camera.main.transform.position = new Vector3(posX, posY, Camera.main.transform.position.z);
      }

      private void SetCameraView()
      {
         m_Scroll += Input.GetAxis("Mouse ScrollWheel") * mScrollSpeed;
         //Debug.Log(m_Scroll);
         Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 
            m_StartScroll - m_Scroll, mScrollLerp);
      }

      private void GetMousePos(out Vector3 mousePositionInWorld)
      {
         m_MousePositionOnScreen = Input.mousePosition;
         m_MousePositionOnScreen.z = mDepthZ;
         mousePositionInWorld = Camera.main.ScreenToWorldPoint(m_MousePositionOnScreen);
      }
      
      // private void CameraMove()
      // {
      //    //Debug.Log(Input.mousePosition);
      //    if (Input.mousePosition.x / 1920 * Screen.width < mLeft)
      //    {
      //       if (Camera.main.transform.position.x < -5.5f)
      //       {
      //          return;
      //       }
      //       Camera.main.transform.position -= new Vector3(1, 0, 0) * mSpeed;
      //    }
      //
      //    if (Input.mousePosition.x / 1920 * Screen.width > mRight)
      //    {
      //       if (Camera.main.transform.position.x > 6.5f)
      //       {
      //          return;
      //       }
      //       Camera.main.transform.position += new Vector3(1, 0, 0) * mSpeed;
      //    }
      //    
      //    if (Input.mousePosition.y / 1080 * Screen.height < mDown)
      //    {
      //       if (Camera.main.transform.position.y < -5f )
      //       {
      //          return;
      //       }
      //       Camera.main.transform.position -= new Vector3(0, 1, 0) * mSpeed;
      //    }
      //    if (Input.mousePosition.y / 1080 * Screen.height > mUp)
      //    {
      //       if (Camera.main.transform.position.y> 3.4f)
      //       {
      //          return;
      //       }
      //       Camera.main.transform.position += new Vector3(0, 1, 0) * mSpeed;
      //    }
      // }
   }
}