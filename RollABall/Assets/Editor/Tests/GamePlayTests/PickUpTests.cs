using System.Collections.Generic;
using Altom.AltDriver;
using NUnit.Framework;
using UnityEngine;

namespace Editor.Tests.GamePlayTests {
    public class PickUpTests {
        private AltDriver _altDriver;
        private Dictionary<int, AltVector2> _quads;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();

            _quads = new Dictionary<int, AltVector2>() {
                { 1, new AltVector2(1, 1) },
                { 2, new AltVector2(-1, 1) },
                { 3, new AltVector2(-1, -1) },
                { 4, new AltVector2(1, -1) }
            };

            _altDriver.LoadScene("MiniGame");
        }

        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
        }


        [Test]
        public void TestPickupAllItemsMouse() {
            var pickupItems = _altDriver.FindObjectsWhichContain(By.NAME, "PickUp");

            for (var i = 1; i < pickupItems.Count; i++) {
                while (true) {
                    try {
                        _altDriver.MoveMouse(GetMouseMoveAngle("Player", pickupItems[i].name, pickupItems[i].name[^2] == '0' ? 14f : 22.5f), 0.005f, false);
                    } catch {
                        Debug.Log($"Exception caught for {pickupItems[i].name}");
                        break;
                    }
                }
            }
        }

        [Test]
        public void TestPickupAllItemsKeyboard() {
            var pickupItems = _altDriver.FindObjectsWhichContain(By.NAME, "PickUp");

            for (var i = 1; i < pickupItems.Count; i++) {
                while (true) {
                    try {
                        var ballPos = _altDriver.FindObject(By.NAME, "Player").GetWorldPosition();
                        var targetPos = _altDriver.FindObject(By.NAME, pickupItems[i].name).GetWorldPosition();
                        var dir = Helpers.Normalized(Helpers.Subtraction(targetPos, ballPos));
                        Debug.Log($"Direction is ({dir.x}, {dir.z})");

                        if (Speed() < 1f) {
                            switch (dir.z) {
                                case > 0f and < 1f:
                                    if (Speed() < 1f) {
                                        _altDriver.PressKey(AltKeyCode.W, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.S, AltKeyCode.W);
                                    } else {
                                        _altDriver.PressKey(AltKeyCode.S, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.W, AltKeyCode.S);
                                    }

                                    break;
                                case > -1f and < 0f:
                                    if (Speed() < 1f) {
                                        _altDriver.PressKey(AltKeyCode.S, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.W, AltKeyCode.S);
                                    } else {
                                        _altDriver.PressKey(AltKeyCode.W, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.S, AltKeyCode.W);
                                    }

                                    break;
                            }

                            switch (dir.x) {
                                case > 0f and < 1f:
                                    if (Speed() < 1f) {
                                        _altDriver.PressKey(AltKeyCode.D, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.A, AltKeyCode.D);
                                    } else {
                                        _altDriver.PressKey(AltKeyCode.A, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.D, AltKeyCode.A);
                                    }

                                    break;
                                case > -1f and < 0f:
                                    if (Speed() < 1f) {
                                        _altDriver.PressKey(AltKeyCode.A, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.D, AltKeyCode.A);
                                    } else {
                                        _altDriver.PressKey(AltKeyCode.D, duration: 0.05f);
                                        // PerformKeysActions(AltKeyCode.A, AltKeyCode.D);
                                    }

                                    break;
                            }
                        }
                    } catch {
                        Debug.Log($"Exception caught for {pickupItems[i].name}");
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

        private float Speed() {
            var s = _altDriver.FindObject(By.NAME, "Player").GetComponentProperty<Vector3>("UnityEngine.Rigidbody", "velocity", "UnityEngine.PhysicsModule", 3).magnitude;
            Debug.Log($"Player Speed is {s}");

            return s;
        }

        private void PerformKeysActions(AltKeyCode keyToRelease, AltKeyCode keyToPress) {
            _altDriver.KeyUp(keyToRelease);
            _altDriver.KeyDown(keyToPress);
            Debug.Log($"Released Key: {keyToRelease}; Pressed key: {keyToPress}");
        }
    }
}