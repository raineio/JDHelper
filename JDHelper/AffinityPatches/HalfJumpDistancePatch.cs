using SiraUtil.Affinity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDHelper.AffinityPatches
{
    [AffinityPatch]
    internal class HalfJumpDistancePatch
    {
        [AffinityPostfix]
        public void Postfix(ref BeatmapObjectSpawnMovementData movementData)
        {
            
        }
    }
}
