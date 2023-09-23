using SiraUtil.Affinity;
using System;
using System.Collections.Generic;
using System.Text;
using SiraUtil.Logging;
using Zenject;

namespace JDHelper.AffinityPatches
{
    [AffinityPatch]
    internal class HalfJumpDistancePatch : IAffinity
    {
        [Inject] private Config _config;
        [Inject] private SiraLog _log;

        [AffinityPrefix]
        [AffinityPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.Init))]
        public void Prefix(float startNoteJumpMovementSpeed, ref BeatmapObjectSpawnMovementData.NoteJumpValueType noteJumpValueType, ref float noteJumpValue)
        {
            noteJumpValueType = BeatmapObjectSpawnMovementData.NoteJumpValueType.JumpDuration;
            noteJumpValue = _config.JumpDistance/startNoteJumpMovementSpeed/2;
        }
    }
}
