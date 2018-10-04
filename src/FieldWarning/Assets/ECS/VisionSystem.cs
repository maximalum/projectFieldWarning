/**
 * Copyright (c) 2017-present, PFW Contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See
 * the License for the specific language governing permissions and limitations under the License.
 */

using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace PFW.ECS
{
    public struct VisionComponent : IComponentData
    {
        public float max_spot_range;
    }

    // This is our first component system and thus contains extra comments as an introduction to ECS. Please do not remove the comments:
    public class VisionSystem : JobComponentSystem
    {
        // A component group contains all the dependencies of the job system and can be loaded from entities that have all required components using the "Inject" keyword:
        public struct VisionGroup
        {
            [ReadOnly]
            public ComponentDataArray<VisionComponent> VisionComponents;
            [ReadOnly]
            public ComponentDataArray<Position> Positions;
        }
        [Inject]
        private VisionGroup _group;

        // The system decides here whether a job shall be executed or not. Note that if scheduled, the job's execute method will not be called once but rather as many times as there are components:
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            for (int i = 0; i < _group.VisionComponents.Length; i++) {


                // Spot anything that needs spotting:
                for (int j = 0; j < _group.VisionComponents.Length; j++) {

                    if (i == j)
                        continue;

                    if (Vector3.Distance(_group.Positions[i].Value, _group.Positions[j].Value) < _group.VisionComponents[i].max_spot_range) {
                        // _group.VisionComponents[j] = spotted
                    }
                }
            }

            //VisibilityJob job = new VisibilityJob();
            //return job.Schedule(this, inputDeps);
            return inputDeps;
        }

        private struct VisibilityJob : 
            IJobProcessComponentData<VisionComponent, Position>
        {
            public void Execute(
                ref VisionComponent vid, 
                [ReadOnly] ref Position pos)
            {
                // iterate over all other positions and set spotted
                Debug.Log("not called");

                //Debug.Log(pos.Value);
                return;
            }
        }
    }
}
