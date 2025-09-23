using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace EditorModeTests
{
    public class EditorModeTestsScript
    {
        [Test]
        public void PlayModeTestScriptSimplePasses()
        {
        }

        [UnityTest]
        public IEnumerator PlayModeTestScriptWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
