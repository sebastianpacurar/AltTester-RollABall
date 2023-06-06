using System.Collections.Generic;
using Altom.AltDriver;
using NUnit.Framework;
using UnityEngine;

namespace Editor.Tests.GamePlayTests {
    public class PickUpTests {
        private AltDriver _altDriver;
        private List<AltObject> _pickupItems;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
            _altDriver.LoadScene("MiniGame");
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }

        [SetUp]
        public void GrabCollectables() {
            _pickupItems = _altDriver.FindObjectsWhichContain(By.NAME, "PickUp");
        }


        [Test]
        public void TestPickupAllItemsMouse() {
            for (var i = 1; i < _pickupItems.Count; i++) {
                while (true) {
                    try {
                        _altDriver.MoveMouse(GetMouseMoveAngle("Player", _pickupItems[i].name, i == 1 ? 5f : 20f + i), 0.0075f, false);
                    } catch {
                        Debug.Log($"Exception caught for {_pickupItems[i].name}: Object Disabled");
                        break;
                    }
                }
            }
        }

        [Test]
        public void TestPickupAllItemsKeyboard() {
            for (var i = 1; i < _pickupItems.Count; i++) {
                while (true) {
                    // GetForwardVector2();

                    try {
                        var dir = Helpers.GetDirection2D(GetWorldPosition("Player"), GetWorldPosition(_pickupItems[i].name));
                        var product = Helpers.DirDotVel(RbVel(), dir);

                        switch (product) { }


                        switch (dir.y) {
                            case > 0f and < 1f:
                                _altDriver.PressKey(AltKeyCode.W, duration: 0.1f, wait: false);
                                break;
                            case > -1 and < 0:
                                _altDriver.PressKey(AltKeyCode.S, duration: 0.1f, wait: false);

                                break;
                        }

                        switch (dir.x) {
                            case > 0f and < 1f:
                                _altDriver.PressKey(AltKeyCode.D, duration: 0.1f, wait: false);
                                break;
                            case > -1 and < 0:
                                _altDriver.PressKey(AltKeyCode.A, duration: 0.1f, wait: false);
                                break;
                        }
                    } catch {
                        Debug.Log($"Exception caught for {_pickupItems[i].name}: Object Disabled");
                        break;
                    }
                }
            }
        }

        private AltVector2 GetMouseMoveAngle(string sourceName, string targetName, float moveFactor) {
            return Helpers.GetDirection2D(GetWorldPosition(sourceName), GetWorldPosition(targetName)) * moveFactor;
        }

        private AltVector2 RbVel() {
            var v = _altDriver.FindObject(By.NAME, "Player").GetComponentProperty<Vector3>("UnityEngine.Rigidbody", "velocity", "UnityEngine.PhysicsModule", 3).normalized;
            return new AltVector2(v.x, v.z);
        }

        private AltVector3 GetWorldPosition(string objName) => _altDriver.FindObject(By.NAME, objName).GetWorldPosition();
    }
}