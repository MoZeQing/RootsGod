using ET;
using GameFramework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class TimerComponent : GameFrameworkComponent
    {
        Dictionary<int, ETTask> mapTask = new Dictionary<int, ETTask>();
        //Dictionary<int, VarSingle> mapCountdown = new Dictionary<int, VarSingle>();
        Dictionary<int, Func<bool>> mapFunc = new Dictionary<int, Func<bool>>();

        int _idGen;

        List<int> toRemove = new List<int>();

        private void Update()
        {
            toRemove.Clear();

            //foreach (var tuple in mapCountdown)
            //{
            //    float time = tuple.Value.Value;
            //    time -= Time.deltaTime;
            //    tuple.Value.Value = time;

            //    if (time < 0)
            //    {
            //        toRemove.Add(tuple.Key);
            //    }
            //}

            foreach (var tuple in mapFunc)
            {
                if (tuple.Value.Invoke())
                {
                    toRemove.Add(tuple.Key);
                }
            }

            foreach (var id in toRemove)
            {
                ETTask tcs = mapTask[id];
                tcs.SetResult();
                tcs = null;

                mapTask.Remove(id);

                //if (mapCountdown.ContainsKey(id))
                //{
                //    mapCountdown.Remove(id);
                //}

                if (mapFunc.ContainsKey(id))
                {
                    mapFunc.Remove(id);
                }
            }
        }

        int GenId()
        {
            return _idGen++;
        }

        public async ETTask WaitForEvent(int eventId)
        {
            ETTask tcs = ETTask.Create();

            bool isDone = false;
            EventHandler<GameEventArgs> callback = (_, __) =>
            {
                isDone = true;
            };

            GameEntry.Event.Subscribe(eventId, callback);

            int id = GenId();
            mapTask.Add(id, tcs);
            mapFunc.Add(id, () => isDone);

            await tcs;

            GameEntry.Event.Unsubscribe(eventId, callback);
            tcs = null;
        }

        public async ETTask WaitUntil(Func<bool> func)
        {
            ETTask tcs = ETTask.Create();

            int id = GenId();

            mapTask.Add(id, tcs);
            mapFunc.Add(id, func);

            await tcs;
        }

        public async ETTask WaitSeconds(float time, ETCancellationToken cancellationToken = null)
        {
            ETTask tcs = ETTask.Create();

            int id = GenId();

            cancellationToken?.Add(() =>
            {
                tcs.SetResult();
                mapTask.Remove(id);
                mapFunc.Remove(id);
            });

            mapTask.Add(id, tcs);
            //mapCountdown.Add(id, time);
            float toTime = Time.time + time;
            mapFunc.Add(id, () => { return Time.time > toTime; });

            await tcs;
        }

        public async ETTask DelaySeconds(float time, ETTask tcs)
        {
            await WaitSeconds(time);
            await tcs;
        }

        public async ETTask DelaySeconds(float time, Action callback)
        {
            await WaitSeconds(time);
            callback();
        }

        public Coroutine DoCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void KillCoroutine(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        public async ETTask DelayAniPlay(Animator animator, string aniname,Action callback)
        {
            await WaitSeconds(GetAnimatorLength(animator, aniname));
            callback();
        }

        /// <summary>
        /// ���animator��ĳ������Ƭ�ε�ʱ������
        /// </summary>
        /// <param animator="animator">Animator���</param> 
        /// <param name="name">Ҫ��õĶ���Ƭ������</param>
        /// <returns></returns>
        public float GetAnimatorLength(Animator animator, string name)
        {
            //����Ƭ��ʱ�䳤��
            float length = 0;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            Debug.Log(clips.Length);
            foreach (AnimationClip clip in clips)
            {
                Debug.Log(clip.name);
                if (clip.name.Equals(name))
                {
                    length = clip.length;
                    break;
                }
            }
            return length;
        }

    }
}