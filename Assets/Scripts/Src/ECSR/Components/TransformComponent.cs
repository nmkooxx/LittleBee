﻿using LogicFrameSync.Src.LockStep.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    public class TransformComponent:IComponent
    {
        float2 LocalPosition;   
        float3x3 float3x3_translation = new float3x3();
        float3x3 float3x3_rotation = new float3x3();
        quaternion Quaternion;

        public TransformComponent(float2 pos)
        {
            LocalPosition = pos;
        }
        public override string ToString()
        {
            return string.Format("[TransformComponent Id:{0} Pos:{1},{2}]", EntityId, LocalPosition.x, LocalPosition.y);
        }

        public Vector2 GetPositionVector2() { return new Vector2(LocalPosition.x, LocalPosition.y); }


        public void Translate(float2 value)
        {
            float3x3_translation.c0 = new float3(1, 0, value.x);
            float3x3_translation.c1 = new float3(0, 1, value.y);
            float3x3_translation.c2 = new float3(0, 0, 1);
            float3 localpos = new float3(LocalPosition,1);
            LocalPosition = math.mul(localpos, float3x3_translation).xy;
        }
        public void Rotate(float degree)
        {
            float radians = math.radians(degree);
            float3x3_rotation.c0 = new float3(math.cos(radians), math.sin(radians), 0);
            float3x3_rotation.c1 = new float3(-math.sin(radians), math.cos(radians), 0);
            float3x3_rotation.c2 = new float3(0, 0, 1);
            Quaternion = new quaternion(float3x3_rotation);
            float3 localpos = new float3(LocalPosition, 1);
            LocalPosition = math.mul(localpos, float3x3_rotation).xy;
        }


        public bool Enable { set; get; }
        public string EntityId { set; get; }
        public IComponent Clone()
        {
            TransformComponent com = new TransformComponent(LocalPosition);
            com.Enable = Enable;
            com.EntityId = EntityId;
            com.Quaternion = Quaternion;
            return com;
        }
        public int GetCommand()
        {
            return FrameCommand.SYNC_TRANSFORM;
        }
    }
}