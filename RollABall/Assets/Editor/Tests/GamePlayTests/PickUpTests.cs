using System.Linq;
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
        public void TestPickupAllItems() {
            var pickupItems = _altDriver.FindObjectsWhichContain(By.NAME, "PickUp");
            var count = 0;

            foreach (var altObj in pickupItems.Where(altObj => !altObj.name.EndsWith("Parent"))) {
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
                    var moveFactor = new AltVector2(dirNorm.x, dirNorm.z) * dist;

                    Debug.Log($"Distance: {dist}, {altObj.name}; DirectionRaw: {dir.x}, {dir.y}, {dir.z}; DirectionNorm: {dirNorm.x}, {dirNorm.y}, {dirNorm.z} MoveFactor: {moveFactor.x}, {moveFactor.y}");
                    _altDriver.MoveMouse(moveFactor);

                    if (Mathf.Abs(dist) < 1f) {
                        break;
                    }
                }
            }
        }
    }
}