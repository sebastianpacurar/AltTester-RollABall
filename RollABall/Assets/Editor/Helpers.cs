using Altom.AltDriver;
using UnityEngine;

namespace Editor {
    public static class Helpers {
        public static AltVector3 Normalized(AltVector3 v) {
            var norm = new Vector3(v.x, v.y, v.z).normalized;
            return new AltVector3(norm.x, norm.y, norm.z);
        }
        public static AltVector3 Subtraction(AltVector3 v1, AltVector3 v2) => new(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        public static float Distance(AltVector3 v1, AltVector3 v2) => Vector3.Distance(new Vector3(v1.x, v1.y, v1.z), new Vector3(v2.x, v2.y, v2.z));
    }
}