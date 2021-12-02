using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Deniverse.AddressableExercise.Domain.ResourceContent;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace Deniverse.AddressableExercise.Domain.Infra.ResourceContent
{
    public sealed class AddressablesLoader : MonoBehaviour, IResourceLoader
    {
        public enum LoadAPIType
        {
            Address,
            Label,
            AssetReference
        }

        [SerializeField] LoadAPIType _loadAPIType;
        [SerializeField] AssetReference _luffyAssetReference;
        [SerializeField] AssetReference _zoroAssetReference;
        [SerializeField] AssetReference _sanjiAssetReference;
        [SerializeField] AssetReference _dataAssetReference;

        IResourceLocator _resourceLocator;
        List<Sprite> _sprites = new();

        const string OnePieceLabel = "onepiece";

        const string LuffyAddress = "Luffy";
        const string ZoroAddress = "Zoro";
        const string SanjiAddress = "Sanji";
        const string DataAddress = "OnePieceData";

        const string LuffyLabel = "luffy";
        const string ZoroLabel = "zoro";
        const string SanjiLabel = "sanji";
        const string DataLabel = "data";

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask InitializeAsync()
        {
            // 初期化処理前に色々設定したい場合はここら辺に記述する

            // Addressable の手動初期化処理
            // (通常は自動的に初期化処理が行われるが，Addressable の初期設定に関して動的にカスタマイズしたい場合は宣言する)
            _resourceLocator = await Addressables.InitializeAsync();

            if (_resourceLocator == null)
            {
                Debug.Log("Initialize Failed.");
                return;
            }

            // バイト単位でアセットの容量を見ることが出来る (リモート読込限定)
            // 一度読み込んだことがある(キャッシュがある)場合は「0」が返ってくることに注意する
            var size = await Addressables.GetDownloadSizeAsync(OnePieceLabel);
            Debug.Log($"Asset Byte Size : {size} B");
        }

        /// <summary>
        /// スプライトの読込
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<List<Sprite>> LoadSpritesAsync(CancellationToken token) => await InternalLoadSpritesAsync(_loadAPIType, token);

        /// <summary>
        /// 名前データの読込
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<List<string>> LoadNamesAsync(CancellationToken token) => await InternalLoadNamesAsync(_loadAPIType, token);

        /// <summary>
        /// アセットのアンロード
        /// </summary>
        /// <param name="key"></param>
        public void Unload(string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                _luffyAssetReference.ReleaseAsset();
                _zoroAssetReference.ReleaseAsset();
                _sanjiAssetReference.ReleaseAsset();
            }
            Addressables.Release(key);
        }

        async UniTask<List<Sprite>> InternalLoadSpritesAsync(LoadAPIType loadAPIType, CancellationToken token)
        {
            Sprite luffy, zoro, sanji;
            switch (loadAPIType)
            {
                // Address を用いたアセットロード
                case LoadAPIType.Address:
                    luffy = await Addressables.LoadAssetAsync<Sprite>(LuffyAddress)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Luffy: {x * 100}")), cancellationToken: token);
                    zoro = await Addressables.LoadAssetAsync<Sprite>(ZoroAddress)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Zoro: {x * 100}")), cancellationToken: token);
                    sanji = await Addressables.LoadAssetAsync<Sprite>(SanjiAddress)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Sanji: {x * 100}")), cancellationToken: token);
                    _sprites.Add(luffy);
                    _sprites.Add(zoro);
                    _sprites.Add(sanji);
                    break;

                // Label を用いたアセットロード
                case LoadAPIType.Label:
                    luffy = await Addressables.LoadAssetAsync<Sprite>(LuffyLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Luffy: {x * 100}")), cancellationToken: token);
                    zoro = await Addressables.LoadAssetAsync<Sprite>(ZoroLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Zoro: {x * 100}")), cancellationToken: token);
                    sanji = await Addressables.LoadAssetAsync<Sprite>(SanjiLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Sanji: {x * 100}")), cancellationToken: token);
                    _sprites.Add(luffy);
                    _sprites.Add(zoro);
                    _sprites.Add(sanji);
                    break;

                // AssetReference を用いたアセットロード
                case LoadAPIType.AssetReference:
                    luffy = await _luffyAssetReference.LoadAssetAsync<Sprite>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Luffy: {x * 100}")), cancellationToken: token);
                    zoro = await _zoroAssetReference.LoadAssetAsync<Sprite>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Zoro: {x * 100}")), cancellationToken: token);
                    sanji = await _sanjiAssetReference.LoadAssetAsync<Sprite>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Sanji: {x * 100}")), cancellationToken: token);
                    _sprites.Add(luffy);
                    _sprites.Add(zoro);
                    _sprites.Add(sanji);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(loadAPIType), loadAPIType, null);
            }

            return _sprites;
        }

        async UniTask<List<string>> InternalLoadNamesAsync(LoadAPIType loadAPIType, CancellationToken token)
        {
            var names = new List<string>();
            OnePieceData data;
            switch (loadAPIType)
            {
                // Address を用いたアセットロード
                case LoadAPIType.Address:
                    data = await Addressables.LoadAssetAsync<OnePieceData>(DataAddress)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Data: {x * 100}")), cancellationToken: token);
                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    break;

                // Label を用いたアセットロード
                case LoadAPIType.Label:
                    data = await Addressables.LoadAssetAsync<OnePieceData>(DataLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Data: {x * 100}")), cancellationToken: token);
                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    break;

                // AssetReference を用いたアセットロード
                case LoadAPIType.AssetReference:
                    data = await _dataAssetReference.LoadAssetAsync<OnePieceData>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Data: {x * 100}")), cancellationToken: token);
                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(loadAPIType), loadAPIType, null);
            }

            return names;
        }


    }
}