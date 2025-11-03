using NUnit.Framework;
using UI;

namespace EditorModeTests
{
    public class ModelTests
    {
        private class TestModel : Model
        {
            public int Data { get; private set; }
            public bool LoadCalled { get; private set; }
            public bool SaveCalled { get; private set; }

            public override void Load()
            {
                LoadCalled = true;
                Data = 42;
            }

            public override void Save()
            {
                SaveCalled = true;
            }
        }

        [Test]
        public void Model_Load_SetsData()
        {
            var model = new TestModel();
            model.Load();

            Assert.IsTrue(model.LoadCalled);
            Assert.AreEqual(42, model.Data);
        }

        [Test]
        public void Model_Save_IsCalled()
        {
            var model = new TestModel();
            model.Save();

            Assert.IsTrue(model.SaveCalled);
        }
    }

    public class ViewModelLogicTests
    {
        private class TestViewModel : ViewModel
        {
            public bool EscapeTriggered { get; private set; }

            protected override void OnEscapeTriggered()
            {
                EscapeTriggered = true;
            }

            // Expose for testing
            public void TriggerEscape() => OnEscapeTriggered();
        }

        [Test]
        public void ViewModel_EscapeCallback_CanBeTriggered()
        {
            var vm = new TestViewModel();

            Assert.IsFalse(vm.EscapeTriggered);

            vm.TriggerEscape();

            Assert.IsTrue(vm.EscapeTriggered);
        }
    }
}
