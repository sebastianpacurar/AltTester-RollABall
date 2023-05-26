using System;
using Altom.AltDriver;
using NUnit.Framework;

namespace Editor.Tests.CanvasTests {
    public class TogglePauseTests {
        private AltDriver _altDriver;

        [OneTimeSetUp]
        public void SetUp() {
            _altDriver = new AltDriver();
        }

        //At the end of the test closes the connection with the socket
        [OneTimeTearDown]
        public void TearDown() {
            _altDriver.Stop();
            _altDriver.LoadScene("MiniGame");
        }


        [Test, Order(1)]
        public void AddItem() {
            _altDriver.PressKey(AltKeyCode.P, duration: 0.1f, wait: true);
            var addBtn = _altDriver.FindObject(By.PATH, "//Canvas/InteractivePanel/Scroll Container/Scroll Section/Add1");
            addBtn.Click();

            var itemsCountTxt = _altDriver.FindObject(By.PATH, "//Canvas/InteractivePanel/Scroll Container/Scroll Section/ItemIndicator/Value/Txt");
            var btnLabel = _altDriver.FindObject(By.PATH, "//Canvas/InteractivePanel/Scroll Container/Scroll Section/Scroll View/Viewport/Content/1/Button/BtnTxt");

            Assert.AreEqual(btnLabel.GetText(), "Btn 1");
            Assert.AreEqual(itemsCountTxt.GetText(), "1");
        }

        [Test, Order(2)]
        public void RemoveItem() {
            var itemsCountTxt = _altDriver.FindObject(By.PATH, "//Canvas/InteractivePanel/Scroll Container/Scroll Section/ItemIndicator/Value/Txt");
            var createdBtn = _altDriver.FindObject(By.NAME, "1");
            createdBtn.Click();

            _altDriver.WaitForObjectNotBePresent(By.NAME, "1");
            Assert.AreEqual(itemsCountTxt.GetText(), "0");

            _altDriver.PressKey(AltKeyCode.P, duration: 0.1f, wait: true);
        }

        [Test, Order(3)]
        public void Add10Items() {
            _altDriver.PressKey(AltKeyCode.P, duration: 0.1f, wait: true);
            var itemsInitialCountTxt = GetCurrentItemsCount();

            var addBtn = _altDriver.FindObject(By.PATH, "//Canvas/InteractivePanel/Scroll Container/Scroll Section/Add10");
            addBtn.Click();
            var itemsAfterCountTxt = GetCurrentItemsCount();

            for (int i = 1; i <= 10; i++) {
                var btnLabel = _altDriver.FindObject(By.PATH, $"//Canvas/InteractivePanel/Scroll Container/Scroll Section/Scroll View/Viewport/Content/{i}/Button/BtnTxt");
                Assert.AreEqual(btnLabel.GetText(), $"Btn {i}");
            }

            Assert.AreEqual(itemsAfterCountTxt, itemsInitialCountTxt + 10);
        }

        [Test, Order(4)]
        public void RemoveMultipleItems() {
            var items = _altDriver.FindObjectsWhichContain(By.TEXT, "Btn ");

            foreach (var item in items) {
                var itemsInitialCountTxt = GetCurrentItemsCount();
                item.Click();
                var itemsAfterCountTxt = GetCurrentItemsCount();
                Assert.AreEqual(itemsAfterCountTxt, itemsInitialCountTxt - 1);
            }

            _altDriver.PressKey(AltKeyCode.P, duration: 0.1f, wait: true);
        }

        private int GetCurrentItemsCount() {
            return int.Parse(_altDriver.FindObject(By.PATH, "//Canvas/InteractivePanel/Scroll Container/Scroll Section/ItemIndicator/Value/Txt").GetText());
        }
    }
}