using System;
using UnityEngine;

namespace TeamEssell.Screenshake2D
{
    /// <summary> An enum describing how channels work. </summary>
    public enum Screenshake2DChannelMode
    {
        /// <summary> If requested strength is greater than current strength, the effect will be applied. </summary>
        [Tooltip("If requested strength is greater than current strength, the effect will be applied.")]
        RefreshStrength,
        /// <summary> The effect will not be applied if the effect is not finished. </summary>
        [Tooltip("The effect will not be applied if the effect is not finished.")]
        IgnoreNew,
    }

    /// <summary> A description of a screenshake. </summary>
    [Serializable]
    public class Screenshake2DDescription
    {
        /// <summary> Whether or not to use a channel. If false, it cannot be individually stopped until completion. </summary>
        [Tooltip("Whether or not to use a channel. If false, it cannot be individually stopped until completion.")]
        public bool UseChannel;
        /// <summary> The ID used for channels. </summary>
        [Tooltip("The ID used for channels.")]
        public string ChannelID = "";
        /// <summary> The channel mode. Affects how it interacts with the channel. </summary>
        [Tooltip("The channel mode. Affects how it interacts with the channel.")]
        public Screenshake2DChannelMode ChannelMode;

        /// <summary> How hard to shake. Larger numbers equate to more shaking. </summary>
        [Tooltip("How hard to shake. Larger numbers equate to more shaking.")]
        public float ShakeStrength;
        /// <summary> The flat rate of strength decay per second. Higher is more. </summary>
        [Tooltip("The flat rate of strength decay per second. Higher is more.")]
        public float DecayFlatRate;
        /// <summary> The percent rate of strength decay per second. Higher is more. </summary>
        [Tooltip("The percent rate of strength decay per second. Higher is more.")]
        public float DecayPercentRate;
        /// <summary> The rate at which the animation cycle occurs. Higher is faster. </summary>
        [Tooltip("The rate at which the animation cycle occurs. Higher is faster.")]
        public float CycleRate = 1;

        /// <summary> Whether or not to offset position when shaking. </summary>
        [Tooltip("Whether or not to offset position when shaking.")]
        public bool UsePosition = true;
        /// <summary> How much to scale the position relative to the other effects. </summary>
        [Tooltip("How much to scale the position relative to the other effects.")]
        public float PositionScale = 1;
        /// <summary> How much in degress to rotate the rail when shaking. Higher is more, but the actual effect is randomized. </summary>
        [Range(0,360)]
        [Tooltip("How much in degress to rotate the rail when shaking. Higher is more, but the actual effect is randomized.")]
        public float RailRotationRate;

        /// <summary> Whether or not to rotate the screen when shaking. </summary>
        [Tooltip("Whether or not to rotate the screen when shaking.")]
        public bool UseRotation = false;
        /// <summary> How much in degress to rotate the screen when shaking. Higher is more, but the actual effect is randomized. </summary>
        [Range(0, 360)]
        [Tooltip("How much in degress to rotate the screen when shaking. Higher is more, but the actual effect is randomized.")]
        public float ScreenRotationRate;

        /// <summary> Whether or not to scale the screen when shaking. </summary>
        [Tooltip("Whether or not to scale the screen when shaking.")]
        public bool UseScale = false;
        /// <summary> The maximum amount to zoom. Goes +- the amount, but amounts under zero are treated as 1 / amount to have accurate zoom levels. THIS SHOULD BE A POSITIVE NUMBER. </summary>
        [Tooltip("The maximum amount to zoom. Goes +- the amount, but amounts under zero are treated as 1 / amount to have accurate zoom levels. THIS SHOULD BE A POSITIVE NUMBER.")]
        public float MaxScaleDifference;

        /// <summary> Whether or not to override the default curves. </summary>
        [Tooltip("Whether or not to override the default curves.")]
        public bool OverrideCurves = false;
       /// <summary> Whether or not to use different curves for position, rotation, and scaling. </summary>
        [Tooltip("Whether or not to use different curves for position, rotation, and scaling.")]
        public bool UseSplitCurves = false;
        /// <summary> The default animation curve. </summary>
        [Tooltip("The default animation curve.")]
        public AnimationCurve Curve;
        /// <summary> The animation curve used for positioning. </summary>
        [Tooltip("The animation curve used for positioning.")]
        public AnimationCurve PositionCurve;
        /// <summary> The animation curve used for rotation. </summary>
        [Tooltip("The animation curve used for rotation.")]
        public AnimationCurve RotationCurve;
        /// <summary> The animation curve used for scaling. </summary>
        [Tooltip("The animation curve used for scaling.")]
        public AnimationCurve ScaleCurve;

    }
} 
