using System.Collections;
using Combat;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace PlayModeTests
{
    public class PlayModeTestScript
    {
        [Test]
        public void SanityTestsGameRunning()
        {
            Assert.IsTrue(Application.isPlaying);
        }

        [UnityTest]
        public IEnumerator HealthTakesDamageAndDies()
        {
            GameObject go = new();
            Health health = go.AddComponent<Health>();
            health.Init(10);

            HitInfo hitInfo = new HitInfo();
            hitInfo.DamageAmount = 10;
            
            health.TakeDamage(hitInfo);
            
            yield return null;
            Assert.IsTrue(health.CurrentHealth == 0);
        }
    }
}
