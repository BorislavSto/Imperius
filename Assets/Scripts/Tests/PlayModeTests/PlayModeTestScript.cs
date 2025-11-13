using System.Collections;
using System.Collections.Generic;
using Combat;
using Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
using DG.Tweening;

// Tests follow an AAA Arrange_Act_Asset pattern
namespace PlayModeTests
{
    public class PlayModeBasicTests
    {
        [Test]
        public void SanityTestsGameRunning()
        {
            Assert.IsTrue(Application.isPlaying);
        }
    }

   public abstract class PlayModeTestBase
   {
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            // Clean up AFTER each test
            yield return CleanupEverything();
        }

        private IEnumerator CleanupEverything()
        {
            DOTween.KillAll();
            yield return null;

            yield return null;

            List<Scene> scenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
                scenes.Add(SceneManager.GetSceneAt(i));

            Scene activeScene = SceneManager.GetActiveScene();

            foreach (var scene in scenes)
            {
                if (scene.IsValid() && scene.isLoaded && scene != activeScene)
                {
                    var unload = SceneManager.UnloadSceneAsync(scene);
                    if (unload != null)
                        yield return unload;
                }
            }

            yield return null;

            var ddolScene = GetDontDestroyOnLoadScene();
            if (ddolScene.IsValid())
            {
                foreach (var go in ddolScene.GetRootGameObjects())
                {
                    if (!go.name.Contains("TestRunner") && !go.name.Contains("Debug Updater"))
                        Object.DestroyImmediate(go);
                }
            }

            yield return null;

            foreach (var go in activeScene.GetRootGameObjects())
            {
                if (!go.name.Contains("TestRunner") && !go.name.Contains("Code-based tests runner"))
                    Object.DestroyImmediate(go);
            }

            yield return new WaitForSeconds(0.05f);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        private Scene GetDontDestroyOnLoadScene()
        {
            GameObject temp = new GameObject("Temp");
            Object.DontDestroyOnLoad(temp);
            Scene ddolScene = temp.scene;
            Object.DestroyImmediate(temp);
            return ddolScene;
        }
    }
    
    public class PlayModeSceneTests : PlayModeTestBase
    {
        [UnityTest]
        public IEnumerator TestBootstrap()
        {
            var asyncOp = SceneManager.LoadSceneAsync(SceneNames.SceneBootstrap, LoadSceneMode.Single);
            asyncOp.allowSceneActivation = true; 
            yield return asyncOp;
            
            Scene scene = SceneManager.GetSceneByName("Bootstrap");
            Assert.IsTrue(scene.isLoaded, "Bootstrap scene should be loaded");
            
            var bootstrapManager = Object.FindAnyObjectByType<Bootstrapper>();
            Assert.IsNotNull(bootstrapManager, "BootstrapManager should exist in the scene");
        }        
        
        [UnityTest]
        public IEnumerator TestBootstrapTransition()
        {
            var asyncOp = SceneManager.LoadSceneAsync(SceneNames.SceneBootstrap, LoadSceneMode.Single);
            asyncOp.allowSceneActivation = true;
            yield return asyncOp;

            float timeout = 5f;
            float timer = 0f;
            while (!SceneManager.GetSceneByName(SceneNames.SceneMainMenu).isLoaded && timer < timeout)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            yield return null;
            
            Assert.IsTrue(SceneManager.GetSceneByName(SceneNames.SceneMainMenu).isLoaded, "Bootstrapper should have transitioned to MainMenu");

            var bootstrapScene = SceneManager.GetSceneByName(SceneNames.SceneBootstrap);
            Assert.IsFalse(bootstrapScene.isLoaded, "Bootstrap scene should be unloaded after transition");
        }
    }

    public class PlaymodeCombatTests : PlayModeTestBase
    {
        [UnityTest]
        public IEnumerator MeleeAttack_EnablesHitboxForCorrectDuration()
        {
            // Arrange
            var go = new GameObject();
            var attackHandler = go.AddComponent<AttackHandler>();
            var damageArea = go.AddComponent<BoxCollider>();
            var damageRelay = go.AddComponent<DamageRelay>();
           
            // Setup
            damageRelay.Init(damageArea);
            var meleeData = ScriptableObject.CreateInstance<MeleeAttackData>();
            attackHandler.SetMeleeConfig(damageArea, damageRelay);
            attackHandler.UpdateAttackSlots(new AttackData[]{meleeData});
            
            // Act
            attackHandler.Attack(new AttackContext());
            yield return new WaitForSeconds(meleeData.windup + 0.01f);
    
            // Assert
            Assert.IsTrue(damageArea.enabled);
    
            yield return new WaitForSeconds(0.2f);
    
            Assert.IsFalse(damageArea.enabled);
        }
        
        [UnityTest]
        public IEnumerator RangedAttack_SpawnsCorrectNumberOfProjectiles()
        {
            // Arrange
            var go = new GameObject();
            var attackHandler = go.AddComponent<AttackHandler>();
            
            // Setup
            attackHandler.SetRangedConfig(go.transform);
            var rangedData = ScriptableObject.CreateInstance<RangedAttackData>();
            var projectilePrefab = Resources.Load<GameObject>("TestContent/FlamingBall");
            rangedData.projectilePrefab = projectilePrefab;
            rangedData.projectileCount = 3;
            attackHandler.UpdateAttackSlots(new AttackData[]{rangedData});
            var ctx = new AttackContext
            {
                Attacker = go,
                TargetLocation = go.transform.position + Vector3.forward
            };
    
            // Act
            attackHandler.Attack(ctx);        
            yield return new WaitForSeconds(rangedData.windup + 0.01f);
    
            // Assert
            var projectiles = Object.FindObjectsByType<Projectile>(FindObjectsSortMode.None);
            Assert.AreEqual(3, projectiles.Length);
        }
        
        [UnityTest]
        public IEnumerator HealthTakesDamageAndDies()
        {
            GameObject go = new();
            Health health = go.AddComponent<Health>();
            health.Init(10);

            HitInfo hitInfo = new HitInfo { DamageAmount = 10 };

            health.TakeDamage(hitInfo);
            
            yield return null;
            Assert.IsTrue(health.currentHealth == 0);
        }
    }
}
