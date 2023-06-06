using Altom.AltDriver;
using UnityEngine;

namespace Editor {
    public static class Helpers {
        private static AltVector3 Normalized(AltVector3 v) {
            var norm = new Vector3(v.x, v.y, v.z).normalized;
            return new AltVector3(norm.x, norm.y, norm.z);
        }

        private static AltVector3 Subtraction(AltVector3 v1, AltVector3 v2) => new(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public static float Distance(AltVector3 v1, AltVector3 v2) {
            var v = Vector3.Distance(new Vector3(v1.x, v1.y, v1.z), new Vector3(v2.x, v2.y, v2.z));
            Debug.Log($"Distance = {v}");
            return v;
        }

        public static AltVector2 GetDirection2D(AltVector3 source, AltVector3 target) {
            var res = Normalized(Subtraction(target, source));
            Debug.Log($"Direction = ({res.x}, {res.z})");

            return new AltVector2(res.x, res.z);
        }

        public static float DirDotVel(AltVector2 v1, AltVector2 v2) {
            var f = Vector2.Dot(new Vector2(v1.x, v1.y), new Vector2(v2.x, v2.y));
            Debug.Log($"Dir Dot Vel = {f}");
            return f;
        }
    }
}