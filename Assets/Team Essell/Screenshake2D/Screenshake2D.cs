using System.Collections.Generic;
using UnityEngine;

namespace TeamEssell.Screenshake2D
{
    /// <summary> A class for causing the current object to shake. Intended for cameras. </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Screenshake2D : MonoBehaviour
    {
        /// <summary> The strength to stop the effect at. The default value is 1. </summary>
        [Tooltip("The strength to stop the effect at. The default value is 1.")]
        public float StrengthEnd = 1f;

        /// <summary> Whether or not to use chunking. This is primarily for pixel games, as it makes it so output values can only be equal to values divisible by chunks. </summary>
        [Tooltip("Whether or not to use chunking. This is primarily for pixel games, as it makes it so output values can only be equal to values divisible by chunks.")]
        public bool UseChunking = true;
        /// <summary> The size of the chunk. Should equal 1 / your PPU typically. </summary>
        [Tooltip("The size of the chunk. Should equal 1 / your PPU typically.")]
        public float ChunkSize = 0.01f;

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

        /// <summary> The initial size of the entry pool. Setting this to a higher value to start with may be ideal if you have many shake effects happening at once. Powers of 2 are recommended. </summary>
        [Tooltip("The initial size of the entry pool. Setting this to a higher value to start with may be ideal if you have many shake effects happening at once. Powers of 2 are recommended.")]
        public int InitialPoolSize = 16;
        /// <summary> Whether or not to trim active entries on update automatically to add them to the pool. Disabling this means you will have to do it yourself by calling Trim() manually.  </summary>
        [Tooltip("Whether or not to trim active entries on update automatically to add them to the pool. Disabling this means you will have to do it yourself by calling Trim() manually.")]
        public bool TrimOnUpdate = true;

        [SerializeField]
        [Tooltip("Whether or not to enable testing. You can either test with fixed data or a scriptable object.")]
        protected bool EnableTesting;
        [SerializeField]
        [Tooltip("Whether or not to use a scriptable object.")]
        protected bool UseScriptableObject;
        [SerializeField]
        protected Screenshake2DDescription TestingData;
        [SerializeField]
        protected Screenshake2DData TestingSO;

        internal List<Screenshake2DEntry> Entries;
        internal List<Screenshake2DEntry> Pool;

        /// <summary> A quick access point to the first made instance. Is only managed on Awake and Destroy, so it may not be accurate if you have multiple instances. </summary>
        public static Screenshake2D Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                //Not destroying the object allows instances of the screenshake manager.
                return;
            }

            Entries = new List<Screenshake2DEntry>();
            Pool = new List<Screenshake2DEntry>();

            for (int i = 0; i < InitialPoolSize; i++)
            {
                Pool.Add(new Screenshake2DEntry());
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            if (TrimOnUpdate)
            {
                Trim();
            }

            var position = Vector2.zero;
            var rotation = 0f;
            var scale = 0f;

            var positioncurve = PositionCurve;
            var rotationcurve = RotationCurve;
            var scalecurve = ScaleCurve;

            if (!UseSplitCurves)
            {
                positioncurve = Curve;
                rotationcurve = Curve;
                scalecurve = Curve;
            }

            foreach (var entry in Entries)
            {
                if (entry.IsFinished())
                {
                    continue;
                }

                entry.UpdatePercent(StrengthEnd);

                if (entry.IsFinished())
                {
                    continue;
                }

                position += entry.GetPosition(positioncurve, UseChunking, ChunkSize);
                rotation += entry.GetRotation(rotationcurve);
                scale += entry.GetScale(scalecurve);
            }

            transform.localPosition = position;
            SetTransformRotation(rotation);
            SetTransformScale(scale);
        }

        private void SetTransformRotation(float value)
        {
            var euler = transform.eulerAngles;
            euler.z = value;
            transform.eulerAngles = euler;
        }

        private void SetTransformScale(float value)
        {
            var scale = 1 + value;

            if (value < 0)
            {
                scale = 1f / 1f - value;
            }

            transform.localScale = new Vector3(scale, scale, 1);
        }

        /// <summary> Trims the finished entries from the active list and pools them. </summary>
        public void Trim()
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].IsFinished())
                {
                    PoolEntry(Entries[i]);
                    Entries.RemoveAt(i);
                    i--;
                }
            }
        }

        private void PoolEntry(Screenshake2DEntry entry) //Doesn't remove from Entries() for performance reasons.
        {
            Pool.Add(entry);
            entry.Reset();
        }

        private Screenshake2DEntry UnpoolEntry()
        {
            Screenshake2DEntry entry;

            if (Pool.Count == 0)
            {
                entry = new Screenshake2DEntry();
            }
            else
            {
                entry = Pool[0];
                Pool.RemoveAt(0);
            }

            entry.Reset();

            return entry;
        }

        /// <summary> Causes the screenshake to happen. </summary>
        /// <param name="data">The scriptable object to use.</param>
        public void Shake(Screenshake2DData data)
        {
            Shake(data.Data, data.Data.UseChannel, data.Data.ChannelID, data.Data.ChannelMode);
        }

        /// <summary> Causes the screenshake to happen. </summary>
        /// <param name="description">The description to use.</param>
        public void Shake(Screenshake2DDescription description)
        {
            Shake(description, description.UseChannel, description.ChannelID, description.ChannelMode);
        }

        /// <summary> Causes the screenshake to happen. </summary>
        /// <param name="data">The scriptable object to use.</param>
        /// <param name="usechannel">Whether or not to use a channel. If false, it cannot be individually stopped until completion.</param>
        /// <param name="channelid">The ID used for channels.</param>
        /// <param name="channelmode">The channel mode. Affects how it interacts with the channel.</param>
        public void Shake(Screenshake2DData data, bool usechannel, string channelid, Screenshake2DChannelMode channelmode)
        {
            Shake(data.Data, usechannel, channelid, channelmode);
        }

        /// <summary> Causes the screenshake to happen. </summary>
        /// <param name="description">The description to use.</param>
        /// <param name="usechannel">Whether or not to use a channel. If false, it cannot be individually stopped until completion.</param>
        /// <param name="channelid">The ID used for channels.</param>
        /// <param name="channelmode">The channel mode. Affects how it interacts with the channel.</param>
        public void Shake(Screenshake2DDescription description, bool usechannel, string channelid, Screenshake2DChannelMode channelmode)
        {
            Screenshake2DEntry currententry = null;

            if (!usechannel)
            {
                currententry = UnpoolEntry();
                Entries.Add(currententry);
            }
            else
            {
                foreach (var entry in Entries)
                {
                    if (entry.Description.ChannelID == channelid)
                    {
                        currententry = entry;
                        break;
                    }
                }

                if (currententry == null)
                {
                    currententry = UnpoolEntry();
                    Entries.Add(currententry);
                }
            }

            currententry.Shake(description, channelmode);
        }

        /// <summary> Stops the channel's shaking effect. </summary>
        /// <param name="id">The ID of the channel to stop.</param>
        public void StopChannel(string id)
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].Description.ChannelID == id)
                {
                    PoolEntry(Entries[i]);
                    Entries.RemoveAt(i);

                    return;
                }
            }
        }

        /// <summary> Stops all shaking effects. </summary>
        public void StopAll()
        {
            while (Entries.Count > 0)
            {
                PoolEntry(Entries[0]);
                Entries.RemoveAt(0);
            }
        }

        protected void TestShake()
        {
            if (Application.isEditor && Application.isPlaying)
            {
                if (!UseScriptableObject)
                {
                    Shake(TestingData);
                }
                else
                {
                    Shake(TestingSO);
                }
            }
            else
            {
                Debug.Log("Cannot run the test outside of play mode.");
            }
        }
    }
}
