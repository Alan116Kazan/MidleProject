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
    public class AiEvaluateSystem : ComponentSystem
    {
        private EntityQuery _evaluateQuery;

        protected override void OnCreate()
        {
            // Запрос для сущностей, имеющих компоненты InputData, MoveData и Transform.
            _evaluateQuery = GetEntityQuery(ComponentType.ReadOnly<AiAgent>());
        }

        protected override void OnUpdate()
        {
            // Перебираем все сущности из запроса.
            Entities.With(_evaluateQuery).ForEach(
                (Entity entity, BehaviorManager manager) =>
                {
                    float highScore = float.MinValue;

                    manager.activeBahaviour = null;
                    foreach (var behaviour in manager.behaviours)
                    {
                        if (behaviour is IBehaviour ai)
                        {
                            var currentScore = ai.Evaluate();
                            if (currentScore > highScore)
                            {
                                highScore = currentScore;
                                manager.activeBahaviour = ai;
                            }
                        }
                    }

                });
        }
    }
}
