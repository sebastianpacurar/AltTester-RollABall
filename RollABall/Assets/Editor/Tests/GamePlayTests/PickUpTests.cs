using System;
using System.Threading;
using Altom.AltDriver;
using NUnit.Framework;
using UnityEngine;

namespace Editor.Tests.GamePlayTests {
    public class PickUpTests {
        private AltDriver _altDriver;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("MiniGame");
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        [Test]
        public void GetComponents() { }

        [Test]
        public void TestPickupAllItems() {
            var pickupItems = _altDriver.FindObjectsWhichContain(By.NAME, "PickUp");
            var count = 0;


            for (var i = 1; i < pickupItems.Count; i++) {
                var altObj = pickupItems[i];

                var targetPos = altObj.GetWorldPosition();
                Debug.Log($"Counter: {count++}, {altObj.name}");


                while (true) {
                    var currBallPos = _altDriver.FindObject(By.NAME, "Player").GetWorldPosition();

                    //TODO: find velocity of rb to product it with the movefactor and get the exact direction based on current movement
                    var ball = _altDriver.FindObject(By.NAME, "Player");
                    // 

                    var dir = Helpers.Subtraction(targetPos, currBallPos);
                    var dirNorm = Helpers.Normalized(dir);
                    var dist = Helpers.Distance(targetPos, currBallPos);
                    var moveFactor = new AltVector2(dirNorm.x, dirNorm.z) * (dist * 5);

                    Debug.Log($"Distance: {dist}, {altObj.name}; DirectionRaw: {dir.x}, {dir.y}, {dir.z}; DirectionNorm: {dirNorm.x}, {dirNorm.y}, {dirNorm.z} MoveFactor: {moveFactor.x}, {moveFactor.y}");

                    _altDriver.MoveMouse(moveFactor, duration: 0.5f);

                    // broken
                    // NOTE: mouse gets stuck in that position (due to cursor locked state?) 
                    if (Math.Abs(dist) < 0.75f) {
                        if (Velocity().magnitude > 0.5f) {
                            var altV = ConvertToAltVector(Velocity());
                            _altDriver.MoveMouse(new AltVector2(altV.x, altV.z));
                        }

                        break;
                    }
                }
            }
        }

        private Vector3 Velocity() => _altDriver.FindObject(By.NAME, "Player").GetComponentProperty<Vector3>("UnityEngine.Rigidbody", "velocity", "UnityEngine.PhysicsModule", 3);
        private AltVector3 ConvertToAltVector(Vector3 v) => new(v.x, v.y, v.z);
        private AltVector3 ReverseVector(AltVector3 v) => new(-v.x, -v.y, -v.z);
    }
}