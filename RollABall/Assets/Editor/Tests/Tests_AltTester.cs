using System.Threading;
using Altom.AltDriver;
using NUnit.Framework;
using UnityEngine;

namespace Editor.Tests {
    public class TestsAltTester
    {
        private AltDriver _altDriver;
        //Before any test it connects with the socket
        [OneTimeSetUp]
        public void SetUp()
        {
            _altDriver = new AltDriver();
        }

        //At the end of the test closes the connection with the socket
        [OneTimeTearDown]
        public void TearDown()
        {
            _altDriver.Stop();
        }

        [Test]
        public void TestMoveBallWithMoveMouse()
        {
            _altDriver.LoadScene("MiniGame");
            var ball = _altDriver.FindObject(By.NAME, "Player");
            var initialPosition = ball.GetWorldPosition();
            _altDriver.MoveMouse(new AltVector2(0, 0), 3f);
            _altDriver.MoveMouse(new AltVector2(100, 100), 3f);
            _altDriver.MoveMouse(new AltVector2(-200, -200), 3f);
            ball = _altDriver.FindObject(By.NAME, "Player");
            var finalPosition = ball.GetWorldPosition();
            Assert.AreNotEqual(initialPosition.x, finalPosition.x);
        }

        [Test]
        public void TestScrollOnScrollbar()
        {
            _altDriver.LoadScene("MiniGame");
            var scrollbar = _altDriver.FindObject(By.NAME, "Scrollbar Vertical");
            var scrollbarPosition = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
            _altDriver.MoveMouse(_altDriver.FindObject(By.NAME, "Scroll View").GetScreenPosition(), 1);
            _altDriver.Scroll(new AltVector2(-3000, -3000), 1, true);
            var scrollbarPositionFinal = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
            Assert.AreNotEqual(scrollbarPosition, scrollbarPositionFinal);
        }

        [Test]
        public void TestMoveMouseOnScrollbar()
        {
            _altDriver.LoadScene("MiniGame");
            var objects = _altDriver.GetAllElementsLight();
            var scrollbar = _altDriver.WaitForObject(By.NAME, "Handle");
            AltVector2 scrollbarInitialPosition = scrollbar.GetScreenPosition(); // use screen coordinates instead of world coordinates        
            _altDriver.MoveMouse(scrollbar.GetScreenPosition()); // move mouse in area where scroll reacts
            _altDriver.Scroll(-200, 0.1f);
            scrollbar = _altDriver.WaitForObject(By.NAME, "Handle");
            AltVector2 scrollbarFinalPosition = scrollbar.GetScreenPosition();
            Assert.AreNotEqual(scrollbarInitialPosition.y, scrollbarFinalPosition.y);//compare y as there is no equality comparer on AltVector2. and we expect only y to change
        }

        [Test]
        public void TestSwipeOnScrollbar()
        {
            _altDriver.LoadScene("MiniGame");
            var scrollbar = _altDriver.WaitForObject(By.NAME, "Handle");
            AltVector2 scrollbarInitialPosition = new AltVector2(scrollbar.worldX, scrollbar.worldY);
            _altDriver.Swipe(new AltVector2(scrollbar.x, scrollbar.y), new AltVector2(scrollbar.x, scrollbar.y - 200), 3);
            scrollbar = _altDriver.WaitForObject(By.NAME, "Handle");
            AltVector2 scrollbarFinalPosition = new AltVector2(scrollbar.worldX, scrollbar.worldY);
            Assert.AreNotEqual(scrollbarInitialPosition.y,scrollbarFinalPosition.y);
        }

        [Test]
        public void TestClickNearScrollBarMovesScrollBar()
        {
            _altDriver.LoadScene("MiniGame");

            var scrollbar = _altDriver.WaitForObject(By.NAME, "Handle");
            var scrollbarInitialPosition = scrollbar.GetScreenPosition();
        
            var scrollBarMoved = new AltVector2(scrollbar.x, scrollbar.y - 100);
            _altDriver.MoveMouse(scrollBarMoved, 1);

            _altDriver.Click(new AltVector2(scrollbar.x, scrollbar.y - 100));

            scrollbar = _altDriver.WaitForObject(By.NAME, "Handle");
            var scrollbarFinalPosition = scrollbar.GetScreenPosition();

            Assert.AreNotEqual(scrollbarInitialPosition.y, scrollbarFinalPosition.y);
        }

        [Test]
        public void TestBeginMoveEndTouchMovesScrollbar()
        {
            _altDriver.LoadScene("MiniGame");
            var scrollbar = _altDriver.FindObject(By.NAME, "Handle");
            var scrollbarPosition = scrollbar.GetScreenPosition();
            int fingerId = _altDriver.BeginTouch(scrollbar.GetScreenPosition());
            _altDriver.MoveTouch(fingerId, scrollbarPosition);
            AltVector2 newPosition = new AltVector2(scrollbar.x, scrollbar.y - 500);
            _altDriver.MoveTouch(fingerId, newPosition);
            _altDriver.EndTouch(fingerId);
            scrollbar = _altDriver.FindObject(By.NAME, "Handle");
            var scrollbarPositionFinal = scrollbar.GetScreenPosition();

            Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
        }

        [Test]
        public void TestPressKeyNearScrollBarMovesScrollBar()
        {
            _altDriver.LoadScene("MiniGame");

            var scrollbar = _altDriver.FindObject(By.NAME, "Handle");
            var scrollbarPosition = scrollbar.GetScreenPosition();
            var scrollBarMoved = new AltVector2(scrollbar.x, scrollbar.y - 100);
            _altDriver.MoveMouse(scrollBarMoved, 1);
            _altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);
            scrollbar = _altDriver.FindObject(By.NAME, "Handle");
            var scrollbarPositionFinal = scrollbar.GetScreenPosition();
            Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
        }

        [Test]
        public void TestKeyDownAndKeyUpMouse0MovesScrollBar()
        {
            _altDriver.LoadScene("MiniGame");

            var scrollbar = _altDriver.FindObject(By.NAME, "Scrollbar Vertical");
            var handle = _altDriver.FindObject(By.NAME, "Handle");
            var scrollbarPosition = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
            var scrollBarMoved = new AltVector2(handle.x, handle.y - 100);
            _altDriver.MoveMouse(scrollBarMoved, 1);
            _altDriver.KeyDown(AltKeyCode.Mouse0);
            _altDriver.KeyUp(AltKeyCode.Mouse0);
            scrollbar = _altDriver.FindObject(By.NAME, "Scrollbar Vertical");
            var scrollbarPositionFinal = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
            Assert.AreNotEqual(scrollbarPosition, scrollbarPositionFinal);
        }

        [Test]
        public void TestBallMovesOnPressKeys()
        {
            _altDriver.LoadScene("MiniGame");

            var ball = _altDriver.FindObject(By.NAME, "Player");
            _altDriver.PressKey(AltKeyCode.S, 1f, 1f);
            var newBall = _altDriver.FindObject(By.NAME, "Player");
            Debug.Log("Ball moved backward");
            Assert.AreNotEqual(ball.GetWorldPosition().z, newBall.GetWorldPosition().z);
            Thread.Sleep(1000);

            ball = _altDriver.FindObject(By.NAME, "Player");
            _altDriver.PressKey(AltKeyCode.W, 1f, 2f);
            newBall = _altDriver.FindObject(By.NAME, "Player");
            Debug.Log("Ball moved forward");
            Assert.AreNotEqual(ball.GetWorldPosition().z, newBall.GetWorldPosition().z);
            Thread.Sleep(1000);

            ball = _altDriver.FindObject(By.NAME, "Player");
            _altDriver.PressKey(AltKeyCode.A, 1f, 2f);
            newBall = _altDriver.FindObject(By.NAME, "Player");
            Debug.Log("Ball moved to the left");
            Assert.AreNotEqual(ball.GetWorldPosition().x, newBall.GetWorldPosition().x);
            Thread.Sleep(1000);

            ball = _altDriver.FindObject(By.NAME, "Player");
            _altDriver.PressKey(AltKeyCode.D, 1f, 2f);
            newBall = _altDriver.FindObject(By.NAME, "Player");
            Debug.Log("Ball moved to the right");
            Assert.AreNotEqual(ball.GetWorldPosition().x, newBall.GetWorldPosition().x);
            Thread.Sleep(2000);
        }

        [Test]
        public void TestTiltBall()
        {
            _altDriver.LoadScene("MiniGame");
            var ball = _altDriver.FindObject(By.NAME, "Player");
            var initialPosition = ball.GetWorldPosition();
            _altDriver.Tilt(new AltVector3(1000, 1000, 1), 3f);
            Assert.AreNotEqual(initialPosition.x, _altDriver.FindObject(By.NAME, "Player").GetWorldPosition().x);
        }

        [Test]
        public void TestDoubleClick()
        {
            _altDriver.LoadScene("MiniGame");
            var button = _altDriver.FindObject(By.NAME, "SpecialButton").Click();
            Thread.Sleep(1000);
            button.Click();
            var text = _altDriver.FindObject(By.PATH,"//ScrollCanvas/SpecialButton/Text (TMP)").GetText();
            Assert.AreEqual("2",text);
        }
    }
}