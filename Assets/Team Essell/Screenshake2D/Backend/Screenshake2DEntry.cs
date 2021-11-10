using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TeamEssell.Screenshake2D
{
    internal class Screenshake2DEntry //This is internal so it's undocumented.
    {
        public Screenshake2DDescription Description { get; private set; }

        protected float CurrentStrength;
        protected float CurrentRailAngle;
        protected float TargetScreenRotation;
        protected float TargetScale;

        protected float InitialStrength;

        protected float Percent;
        protected bool Direction;

        public Screenshake2DEntry()
        {
            Reset();
        }

        public void Shake(Screenshake2DDescription description)
        {
            switch (description.ChannelMode)
            {
                case Screenshake2DChannelMode.RefreshStrength:
                    if (description.ShakeStrength < CurrentStrength)
                    {
                        return;
                    }

                    break;
                case Screenshake2DChannelMode.IgnoreNew:
                    if (!IsFinished())
                    {
                        return;
                    }

                    break;
                default:
                    throw new Exception("Screenshake mode \"" + description + "\" is not implemented.");
            }

            Description = description;

            CurrentStrength = description.ShakeStrength;
            InitialStrength = CurrentStrength;
        }

        public void Reset()
        {
            Description = null;
            CurrentRailAngle = Random.Range(-180, 180);
            Direction = true;
        }

        public void UpdatePercent(float strengthend)
        {
            if (Mathf.Approximately(CurrentStrength, 0))
            {
                return;
            }

            //Apply logic change.
            if (Direction) //Moving right
            {
                var islessthanzero = Percent < 0;

                Percent += Description.CycleRate * Time.deltaTime;

                if (islessthanzero && Percent > 0)
                {
                    OnPassCenter(strengthend);
                }

                if (Percent >= 1)
                {
                    Direction = false;
                    Percent = 1 - (Percent - 1);
                }
            }
            else //Moving left
            {
                var isgreaterthanzero = Percent > 0;

                Percent -= Description.CycleRate * Time.deltaTime;

                if (isgreaterthanzero && Percent < 0)
                {
                    OnPassCenter(strengthend);
                }

                if (Percent <= -1)
                {
                    Direction = true;
                    Percent = -1 - (Percent + 1);
                }
            }
        }

        private void OnPassCenter(float strengthend)
        {
            //Apply decay
            CurrentStrength *= (1 - (Description.DecayPercentRate * Time.deltaTime));
            CurrentStrength -= Description.DecayFlatRate * Time.deltaTime;

            if (CurrentStrength <= strengthend)
            {
                CurrentStrength = 0;
                return;
            }

            //Apply Rail Rotation
            CurrentRailAngle += Random.Range(-Description.RailRotationRate, Description.RailRotationRate);

            //Apply Screen Rotation
            TargetScreenRotation = Random.Range(-Description.ScreenRotationRate, Description.ScreenRotationRate);

            //Apply Scale
            TargetScale = Random.Range(-Description.MaxScaleDifference, Description.MaxScaleDifference);
        }
        
        public Vector2 GetPosition(AnimationCurve curve, bool usechunking, float chunksize)
        {
            if (!Description.UsePosition || Mathf.Approximately(Description.PositionScale, 0))
            {
                return Vector2.zero;
            }

            if (Description.OverrideCurves)
            {
                curve = Description.Curve;

                if (Description.UseSplitCurves)
                {
                    curve = Description.PositionCurve;
                }
            }

            var pos = Quaternion.Euler(0, 0, CurrentRailAngle) * new Vector3(0, CurrentStrength * curve.Evaluate(Percent), 0);

            pos *= Description.PositionScale;

            if (usechunking)
            {
                pos = new Vector2(ChunkPercent(pos.x, chunksize), ChunkPercent(pos.y, chunksize));
            }

            return pos;
        }

        public float GetRotation(AnimationCurve curve)
        {
            if (!Description.UseRotation)
            {
                return 0;
            }

            if (Description.OverrideCurves)
            {
                curve = Description.Curve;

                if (Description.UseSplitCurves)
                {
                    curve = Description.RotationCurve; 
                }
            }

            return TargetScreenRotation * (CurrentStrength / InitialStrength) * curve.Evaluate(Percent);
        }

        public float GetScale(AnimationCurve curve)
        {
            if (!Description.UseScale)
            {
                return 0;
            }

            if (Description.OverrideCurves)
            {
                curve = Description.Curve;

                if (Description.UseSplitCurves)
                {
                    curve = Description.ScaleCurve;
                }
            }

            var percent = (CurrentStrength / InitialStrength);

            return TargetScale * percent * curve.Evaluate(Percent);
        }

        public bool IsFinished()
        {
            return Mathf.Approximately(CurrentStrength, 0);
        }

        private float ChunkPercent(float value, float rate)
        {
            var i = (int) (value / rate);

            if (i > 0)
            {
                i++;
            }

            return i * rate;
        }
    }
}