//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-04-26 23:00:10.264
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
    /// 节点的数据配置。
    /// </summary>
    public class DRNode : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取Node编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否可被选择。
        /// </summary>
        public bool Select
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否消耗资源。
        /// </summary>
        public bool Costable
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否可移动。
        /// </summary>
        public bool Movable
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取是否可连接。
        /// </summary>
        public bool Connectable
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取资源总量。
        /// </summary>
        public float Total
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取收入。
        /// </summary>
        public int Income
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取消耗速率。
        /// </summary>
        public int CostPersecond
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
            AssetName = columnStrings[index++];
            Select = bool.Parse(columnStrings[index++]);
            Costable = bool.Parse(columnStrings[index++]);
            Movable = bool.Parse(columnStrings[index++]);
            Connectable = bool.Parse(columnStrings[index++]);
            Total = float.Parse(columnStrings[index++]);
            Income = int.Parse(columnStrings[index++]);
            CostPersecond = int.Parse(columnStrings[index++]);

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
                    AssetName = binaryReader.ReadString();
                    Select = binaryReader.ReadBoolean();
                    Costable = binaryReader.ReadBoolean();
                    Movable = binaryReader.ReadBoolean();
                    Connectable = binaryReader.ReadBoolean();
                    Total = binaryReader.ReadSingle();
                    Income = binaryReader.Read7BitEncodedInt32();
                    CostPersecond = binaryReader.Read7BitEncodedInt32();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
