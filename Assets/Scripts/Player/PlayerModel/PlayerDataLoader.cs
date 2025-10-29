using UnityEngine;

public class PlayerDataLoader : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Mesh meshToLoad;
    
    private void Start()
    {
        if (skinnedMeshRenderer != null && meshToLoad != null)
            skinnedMeshRenderer.sharedMesh = meshToLoad;
        else
            Debug.LogWarning("SkinnedMeshRenderer or Mesh to load is not assigned.");
    }
}
