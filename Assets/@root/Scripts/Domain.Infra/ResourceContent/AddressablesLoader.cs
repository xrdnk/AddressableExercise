using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Deniverse.AddressableExercise.Domain.ResourceContent;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

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
        SceneInstance _sceneInstance;
        readonly Dictionary<string, object> _assets = new();

        const string OnePieceLabel = "onepiece";

        const string LuffyAddress = "Luffy";
        const string ZoroAddress = "Zoro";
        const string SanjiAddress = "Sanji";
        const string DataAddress = "OnePieceData";

        const string LuffyLabel = "luffy";
        const string ZoroLabel = "zoro";
        const string SanjiLabel = "sanji";
        const string DataLabel = "data";

        const string ModalAddress = "Modal";
        const int PercentageRate = 100;

        /// <summary>
        /// 初期化
        /// </summary>
        public async UniTask InitializeAsync()
        {
            // 初期化処理前に色々設定したい場合はここら辺に記述する
            // NOTE: デモのため，初期化前に残っているキャッシュを消す
            Caching.ClearCache();

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
        /// モーダルシーンの読込
        /// </summary>
        /// <param name="token"></param>
        public async UniTask LoadModalAsync(CancellationToken token) =>
            _sceneInstance = await Addressables.LoadSceneAsync(ModalAddress, LoadSceneMode.Additive)
                .ToUniTask(Progress.Create<float>(x => Debug.Log($"Modal: {x * PercentageRate}")), cancellationToken: token);

        /// <summary>
        /// モーダルシーンの破棄
        /// </summary>
        public void UnloadModal() => _ = Addressables.UnloadSceneAsync(_sceneInstance);

        /// <summary>
        /// アセットのアンロード
        /// </summary>
        /// <param name="key"></param>
        public void Unload(string key) => InternalRemoveAsset(key);

        async UniTask<List<Sprite>> InternalLoadSpritesAsync(LoadAPIType loadAPIType, CancellationToken token)
        {
            var sprites = new List<Sprite>();
            Sprite luffy, zoro, sanji;
            switch (loadAPIType)
            {
                // Address を用いたアセットロード
                case LoadAPIType.Address:
                    luffy = await Addressables.LoadAssetAsync<Sprite>(LuffyAddress)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Luffy: {x * PercentageRate}")), cancellationToken: token);
                    zoro = await Addressables.LoadAssetAsync<Sprite>(ZoroAddress)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Zoro: {x * PercentageRate}")), cancellationToken: token);
                    sanji = await Addressables.LoadAssetAsync<Sprite>(SanjiAddress)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Sanji: {x * PercentageRate}")), cancellationToken: token);
                    sprites.Add(luffy);
                    sprites.Add(zoro);
                    sprites.Add(sanji);
                    InternalAddAsset(LuffyAddress, luffy);
                    InternalAddAsset(ZoroAddress, zoro);
                    InternalAddAsset(SanjiAddress, sanji);
                    break;

                // Label を用いたアセットロード
                case LoadAPIType.Label:
                    luffy = await Addressables.LoadAssetAsync<Sprite>(LuffyLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Luffy: {x * PercentageRate}")), cancellationToken: token);
                    zoro = await Addressables.LoadAssetAsync<Sprite>(ZoroLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Zoro: {x * PercentageRate}")), cancellationToken: token);
                    sanji = await Addressables.LoadAssetAsync<Sprite>(SanjiLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Sanji: {x * PercentageRate}")), cancellationToken: token);
                    sprites.Add(luffy);
                    sprites.Add(zoro);
                    sprites.Add(sanji);
                    InternalAddAsset(LuffyLabel, luffy);
                    InternalAddAsset(ZoroLabel, zoro);
                    InternalAddAsset(SanjiLabel, sanji);
                    break;

                // AssetReference を用いたアセットロード
                case LoadAPIType.AssetReference:
                    luffy = await _luffyAssetReference.LoadAssetAsync<Sprite>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Luffy: {x * PercentageRate}")), cancellationToken: token);
                    zoro = await _zoroAssetReference.LoadAssetAsync<Sprite>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Zoro: {x * PercentageRate}")), cancellationToken: token);
                    sanji = await _sanjiAssetReference.LoadAssetAsync<Sprite>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Sanji: {x * PercentageRate}")), cancellationToken: token);
                    sprites.Add(luffy);
                    sprites.Add(zoro);
                    sprites.Add(sanji);
                    InternalAddAsset(_luffyAssetReference.RuntimeKey.ToString(), luffy);
                    InternalAddAsset(_zoroAssetReference.RuntimeKey.ToString(), zoro);
                    InternalAddAsset(_sanjiAssetReference.RuntimeKey.ToString(), sanji);
                    Debug.Log($"{_luffyAssetReference.RuntimeKey}");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(loadAPIType), loadAPIType, null);
            }

            return sprites;
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
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Data: {x * PercentageRate}")), cancellationToken: token);
                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    InternalAddAsset(DataAddress, data);
                    break;

                // Label を用いたアセットロード
                case LoadAPIType.Label:
                    data = await Addressables.LoadAssetAsync<OnePieceData>(DataLabel)
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Data: {x * PercentageRate}")), cancellationToken: token);
                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    InternalAddAsset(DataLabel, data);
                    break;

                // AssetReference を用いたアセットロード
                case LoadAPIType.AssetReference:
                    data = await _dataAssetReference.LoadAssetAsync<OnePieceData>()
                        .ToUniTask(Progress.Create<float>(x => Debug.Log($"Data: {x * PercentageRate}")), cancellationToken: token);
                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    InternalAddAsset(_dataAssetReference.RuntimeKey.ToString(), data);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(loadAPIType), loadAPIType, null);
            }

            return names;
        }

        void InternalAddAsset(string key, object asset)
        {
            if (!_assets.ContainsKey(key))
            {
                _assets.Add(key, asset);
                Debug.Log($"Added -> {key} : {asset}");
            }
        }

        void InternalRemoveAsset(string key)
        {
            if (_assets.TryGetValue(key, out var asset))
            {
                Addressables.Release(asset);
                _assets.Remove(key);
                Debug.Log($"Removed -> {key} : {asset}");
            }
        }
    }
}