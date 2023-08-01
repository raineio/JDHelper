using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace JDHelper
{
    public class Config
    {
        public virtual bool Enabled { get; set; } = false;
        public virtual bool ShowReactionTime { get; set; } = true;
        public virtual bool UseReactionTime { get; set; } = false;

        public virtual float BaseJumpDistance { get; set; }
        public virtual float NewJumpDistance { get; set; } = 16f;

        [UseConverter(typeof(ListConverter<JumpDistancePreference>))]
        public virtual List<JumpDistancePreference> PreferredJumpDistanceValue { get; set; } = new List<JumpDistancePreference>();

        [UseConverter(typeof(ListConverter<ReactionTimePreference>))]
        public virtual List<ReactionTimePreference> PreferredReactionTime { get; set; } = new List<ReactionTimePreference>();
    }

    public class JumpDistancePreference
    {
        public float jumpDistance;

        public JumpDistancePreference() { }

        public JumpDistancePreference(float jumpDistance)
        {
            this.jumpDistance = jumpDistance;
        }
    }

    public class ReactionTimePreference
    {
        public float reactionTime;

        public ReactionTimePreference() { }

        public ReactionTimePreference(float reactionTime)
        {
            this.reactionTime = reactionTime;
        }
    }
}
