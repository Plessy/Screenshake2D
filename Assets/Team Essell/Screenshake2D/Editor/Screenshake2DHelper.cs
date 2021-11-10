using UnityEditor;
using UnityEngine;

namespace TeamEssell.Screenshake2D
{
    internal static class Screenshake2DHelper //Internal so undocumented
    {
        public static bool ValidateCurve(SerializedProperty curveproperty)
        {
            var curve = curveproperty.animationCurveValue;

            if (curve.keys.Length == 0)
            {
                curve.keys = GetDefaultCurveKeys();
                curveproperty.animationCurveValue = curve;

                return true;
            }

            return false;
        }

        private static Keyframe[] GetDefaultCurveKeys()
        {
            var keys = new Keyframe[3];

            keys[0] = new Keyframe(-1, 1, 0, 0, 0, 0.3333333f);
            keys[1] = new Keyframe(0, 0, 0, 0, 0.3333333f, 0.3333333f);
            keys[2] = new Keyframe(1, 1, 0, 0, 0.3333333f, 0);

            return keys;
        }
    }
}