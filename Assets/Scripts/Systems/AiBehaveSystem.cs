using Assets.Scripts.Components.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class AiBehaveSystem : ComponentSystem
    {
        private EntityQuery _behaveQuery;

        protected override void OnCreate()
        {
            _behaveQuery = GetEntityQuery(ComponentType.ReadOnly<AiAgent>());
        }

        protected override void OnUpdate()
        {
            Entities.With(_behaveQuery).ForEach(
                (Entity entity, BehaviorManager manager) =>
                {
                    manager.activeBahaviour?.Behave();
                });
        }
    }
}

