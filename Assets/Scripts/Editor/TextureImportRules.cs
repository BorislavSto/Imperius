using UnityEditor;

namespace Editor
{
    public class TextureImportRules : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            TextureImporter importer = (TextureImporter)assetImporter;

            importer.maxTextureSize = 256;
            importer.textureType = TextureImporterType.Default;

            importer.isReadable = false;
        }
    }
}
