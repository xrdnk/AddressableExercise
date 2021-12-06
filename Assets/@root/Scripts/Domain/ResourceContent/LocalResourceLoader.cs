using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Deniverse.AddressableExercise.Domain.ResourceContent
{
    public sealed class LocalResourceLoader : MonoBehaviour, IResourceLoader
    {
        public UniTask InitializeAsync()
        {
            return default;
        }

        public UniTask<List<Sprite>> LoadSpritesAsync(CancellationToken token)
        {
            return default;
        }

        public UniTask<List<string>> LoadNamesAsync(CancellationToken token)
        {
            return default;
        }

        public UniTask LoadModalAsync(CancellationToken token)
        {
            return default;
        }

        public void UnloadModal()
        {
        }

        public void Unload(string key = null)
        {
        }
    }
}