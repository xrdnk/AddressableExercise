using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Deniverse.AddressableExercise.Domain.ResourceContent
{
    public interface IResourceLoader
    {
        UniTask InitializeAsync();
        UniTask<List<Sprite>> LoadSpritesAsync(CancellationToken token);
        UniTask<List<string>> LoadNamesAsync(CancellationToken token);
        UniTask LoadModalAsync(CancellationToken token);
        void UnloadModal();
        void Unload(string key = null);
    }

    public enum LoaderType
    {
        ResourcesLoad,
        AddressablesLoad
    }
}