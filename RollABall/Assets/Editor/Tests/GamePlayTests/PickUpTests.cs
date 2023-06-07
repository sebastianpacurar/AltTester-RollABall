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
        [Ignore("Issue with complexity of Force Impulse vs timing the duration of a press. On Hold")]
        public void TestPickupAllItemsKeyboard() {
            _altDriver.PressKey(AltKeyCode.W);
            _altDriver.PressKey(AltKeyCode.S);

            for (var i = 1; i < _pickupItems.Count; i++) {
                while (true) {
                    // GetForwardVector2();
                    _altDriver.PressKey(AltKeyCode.D, duration: 0.001f, wait: false);

                    try {
                        var dir = Helpers.GetDirection2D(GetWorldPosition("Player"), GetWorldPosition(_pickupItems[i].name));
                        var product = Helpers.DirDotVel(RbVel(), dir);
                        var dist = Helpers.Distance(GetWorldPosition("Player"), GetWorldPosition(_pickupItems[i].name));
                        
                        

                        if (i == 1) {
                            _altDriver.PressKey(AltKeyCode.D, duration: 0.05f, wait: true);
                        } else {
                            // if north
                            if (dir.y > 0f && dir.y < 1f) {
                                _altDriver.PressKey(AltKeyCode.W, duration: 0.05f, wait: true);

                                // north-west
                                if (dir.x > -1f && dir.x < 0f) {
                                    _altDriver.PressKey(AltKeyCode.A, duration: 0.05f, wait: true);
                                }

                                // north-east
                                else if (dir.x > 0f && dir.x < 1f) {
                                    _altDriver.PressKey(AltKeyCode.D, duration: 0.05f, wait: true);
                                }
                            }

                            // if south
                            else if (dir.y > -1f && dir.y < 0f) {
                                _altDriver.PressKey(AltKeyCode.S, duration: 0.05f, wait: true);

                                // south-west
                                if (dir.x > -1f && dir.x < 0f) {
                                    _altDriver.PressKey(AltKeyCode.A, duration: 0.05f, wait: true);
                                }

                                // south-east
                                else if (dir.x > 0f && dir.x < 1f) {
                                    _altDriver.PressKey(AltKeyCode.D, duration: 0.05f, wait: true);
                                }
                            }
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

        private float Speed() {
            return _altDriver.FindObject(By.NAME, "Player").GetComponentProperty<Vector3>("UnityEngine.Rigidbody", "velocity", "UnityEngine.PhysicsModule", 3).magnitude;
        }

        private AltVector2 RbVel() {
            var v = _altDriver.FindObject(By.NAME, "Player").GetComponentProperty<Vector3>("UnityEngine.Rigidbody", "velocity", "UnityEngine.PhysicsModule", 3).normalized;
            return new AltVector2(v.x, v.z);
        }

        private AltVector3 GetWorldPosition(string objName) => _altDriver.FindObject(By.NAME, objName).GetWorldPosition();
    }
}