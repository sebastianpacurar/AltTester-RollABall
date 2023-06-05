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
                        Debug.Log($"Exception caught for {_pickupItems[i].name}: Object Destroyed");
                        break;
                    }
                }
            }
        }

        [Test]
        public void TestPickupAllItemsKeyboard() {
            for (var i = 1; i < _pickupItems.Count; i++) {
                while (true) {
                    try {
                        var ballPos = _altDriver.FindObject(By.NAME, "Player");
                        var dir = Helpers.GetDirection2D(GetWorldPosition(ballPos.name), GetWorldPosition(_pickupItems[i].name));
                        var dist = Helpers.Distance(GetWorldPosition(ballPos.name), GetWorldPosition(_pickupItems[i].name));

                        switch (dir.y) {
                            case > 0f and < 1f:
                                if (dist > 0f) {
                                    PerformKeysActions(AltKeyCode.S, AltKeyCode.W);
                                    // _altDriver.KeyDown(AltKeyCode.W);
                                    // Debug.Log("KeyyyDown W");
                                } else {
                                    PerformKeysActions(AltKeyCode.W, AltKeyCode.S);
                                    // _altDriver.KeyUp(AltKeyCode.W);
                                    // Debug.Log("KeyyyUp W");
                                }

                                break;
                            case > -1f and < 0f:
                                if (dist > 0f) {
                                    PerformKeysActions(AltKeyCode.W, AltKeyCode.S);
                                    // _altDriver.KeyDown(AltKeyCode.S);
                                    // Debug.Log("KeyyyDown S");
                                } else {
                                    PerformKeysActions(AltKeyCode.S, AltKeyCode.W);
                                    // _altDriver.KeyUp(AltKeyCode.S);
                                    // Debug.Log("KeyyyUp S");
                                }

                                break;
                        }

                        switch (dir.x) {
                            case > 0f and < 1f:
                                if (dist > 0f) {
                                    PerformKeysActions(AltKeyCode.A, AltKeyCode.D);
                                    // _altDriver.KeyDown(AltKeyCode.D);
                                    // Debug.Log("KeyyyDown D");
                                } else {
                                    PerformKeysActions(AltKeyCode.D, AltKeyCode.A);
                                    // _altDriver.KeyUp(AltKeyCode.D);
                                    // Debug.Log("KeyyyUp D");
                                }

                                break;
                            case > -1f and < 0f:
                                if (dist > 0f) {
                                    PerformKeysActions(AltKeyCode.D, AltKeyCode.A);
                                    // _altDriver.KeyDown(AltKeyCode.A);
                                    // Debug.Log("KeyyyDown A");
                                } else {
                                    PerformKeysActions(AltKeyCode.A, AltKeyCode.D);
                                    // _altDriver.KeyUp(AltKeyCode.A);
                                    // Debug.Log("KeyyyUp A");
                                }

                                break;
                        }
                    } catch {
                        Debug.Log($"Exception caught for {_pickupItems[i].name}: Object Destroyed");
                        break;
                    }
                }
            }
        }

        private AltVector2 GetMouseMoveAngle(string sourceName, string targetName, float moveFactor) {
            return Helpers.GetDirection2D(GetWorldPosition(sourceName), GetWorldPosition(targetName)) * moveFactor;
        }

        private float Speed() {
            var s = _altDriver.FindObject(By.NAME, "Player").GetComponentProperty<Vector3>("UnityEngine.Rigidbody", "velocity", "UnityEngine.PhysicsModule", 3).magnitude;
            Debug.Log($"Player Speed is {s}");

            return s;
        }


        private AltVector3 GetWorldPosition(string objName) => _altDriver.FindObject(By.NAME, objName).GetWorldPosition();

        private void PerformKeysActions(AltKeyCode keyToRelease, AltKeyCode keyToPress) {
            if (Speed() > 2.5f) {
                _altDriver.KeyUp(keyToRelease);
                _altDriver.KeyDown(keyToPress);
            } else {
                _altDriver.KeysDown(new[] { keyToRelease, keyToPress });
            }


            Debug.Log($"Released Key: {keyToRelease}; Pressed key: {keyToPress}");
        }
    }
}