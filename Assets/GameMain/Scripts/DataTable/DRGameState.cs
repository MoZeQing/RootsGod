//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-26 23:00:10.262
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 游戏状态配置表。
    /// </summary>
    public class DRGameState : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取状态编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取结算消耗血量。
        /// </summary>
        public int Cost
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卡池深度。
        /// </summary>
        public int PoolDepth1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取卡池深度2。
        /// </summary>
        public int PoolDepth2
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            Cost = int.Parse(columnStrings[index++]);
            PoolDepth1 = int.Parse(columnStrings[index++]);
            PoolDepth2 = int.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Cost = binaryReader.Read7BitEncodedInt32();
                    PoolDepth1 = binaryReader.Read7BitEncodedInt32();
                    PoolDepth2 = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] m_PoolDepth = null;

        public int PoolDepthCount
        {
            get
            {
                return m_PoolDepth.Length;
            }
        }

        public int GetPoolDepth(int id)
        {
            foreach (KeyValuePair<int, int> i in m_PoolDepth)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetPoolDepth with invalid id '{0}'.", id));
        }

        public int GetPoolDepthAt(int index)
        {
            if (index < 0 || index >= m_PoolDepth.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetPoolDepthAt with invalid index '{0}'.", index));
            }

            return m_PoolDepth[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_PoolDepth = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(1, PoolDepth1),
                new KeyValuePair<int, int>(2, PoolDepth2),
            };
        }
    }
}
