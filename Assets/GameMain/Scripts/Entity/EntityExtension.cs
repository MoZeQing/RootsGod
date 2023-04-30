//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        /// <summary>
        /// 实体管理的线
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="data"></param>
        public static void ShowLine(this EntityComponent entityComponent, LineData data)
        {
            entityComponent.ShowEntity(typeof(Line), "Line", Constant.AssetPriority.MyAircraftAsset, data);
        }

        public static void ShowNode(this EntityComponent entityComponent, NodeData data)
        {
            entityComponent.ShowEntity(typeof(Node), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }

        public static void ShowCenterNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(CenterNodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }

        public static void ShowLevel1Node(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(Level1NodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }

        public static void ShowLevel2Node(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(Level2NodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }

        public static void ShowLevel2To1Node(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(Level2To1NodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowEmptyNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(EmptyNodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowBlockingNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(BlockingNodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowClearNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(ClearNodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowTreeNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(TreeNode), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowBoomNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(BoomNode), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowLeafNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(LeafNode), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowCardPackage(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(CardPackageNodeComponent), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowAreaBoom(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(BoomArea), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowKennelNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(KennelNode), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }
        public static void ShowDogNode(this EntityComponent entityComponent, ComponentData data)
        {
            entityComponent.ShowEntity(typeof(DogNode), "Node", Constant.AssetPriority.MyAircraftAsset, data);
        }


        public static void ShowEffect(this EntityComponent entityComponent, EffectData data)
        {
            entityComponent.ShowEntity(typeof(Effect), "Effect", Constant.AssetPriority.EffectAsset, data);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }

        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
