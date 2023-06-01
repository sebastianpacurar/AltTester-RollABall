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

            for (var i = 1; i < pickupItems.Count; i++) {
                while (true) {
                    try {
                        _altDriver.MoveMouse(GetMouseMoveAngle("Player", pickupItems[i].name, pickupItems[i].name[^2] == '0' ? 14f : 22.5f), 0.005f, false);
                    } catch {
                        break;
                    }
                }
            }
        }

        private AltVector2 GetMouseMoveAngle(string sourceName, string targetName, float moveFactor) {
            var targetPos = _altDriver.FindObject(By.NAME, targetName).GetWorldPosition();
            var sourcePos = _altDriver.FindObject(By.NAME, sourceName).GetWorldPosition();
            var dir = Helpers.Normalized(Helpers.Subtraction(targetPos, sourcePos));
            var res = new AltVector2(dir.x, dir.z) * moveFactor;
            Debug.Log($"Current Pickup: {targetName}; DirectionNorm: {dir.x}, {dir.y}, {dir.z} MoveFactor: {res.x}, {res.y}");

            return res;
        }
    }
}