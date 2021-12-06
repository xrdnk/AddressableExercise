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


            // Addressable の手動初期化処理
            // (通常は自動的に初期化処理が行われるが，Addressable の初期設定に関して動的にカスタマイズしたい場合は宣言する)


            if (_resourceLocator == null)
            {
                Debug.Log("Initialize Failed.");
                return;
            }

            // バイト単位でアセットの容量を見ることが出来る (リモート読込限定)
            // 一度読み込んだことがある(キャッシュがある)場合は「0」が返ってくることに注意する

        }

        /// <summary>
        /// スプライトの読込
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<List<Sprite>> LoadSpritesAsync(CancellationToken token)
        {
            return default;
        }

        /// <summary>
        /// 名前データの読込
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTask<List<string>> LoadNamesAsync(CancellationToken token)
        {
            return default;
        }

        /// <summary>
        /// モーダルシーンの読込
        /// </summary>
        /// <param name="token"></param>
        public async UniTask LoadModalAsync(CancellationToken token)
        {
            return;
        }

        /// <summary>
        /// モーダルシーンの破棄
        /// </summary>
        public void UnloadModal()
        {
            return;
        }

        /// <summary>
        /// アセットのアンロード
        /// </summary>
        /// <param name="key"></param>
        public void Unload(string key)
        {
            return;
        }

        async UniTask<List<Sprite>> InternalLoadSpritesAsync(LoadAPIType loadAPIType, CancellationToken token)
        {
            var sprites = new List<Sprite>();
            Sprite luffy = null, zoro = null, sanji = null;
            switch (loadAPIType)
            {
                // Address を用いたアセットロード
                case LoadAPIType.Address:

                    sprites.Add(luffy);
                    sprites.Add(zoro);
                    sprites.Add(sanji);
                    InternalAddAsset(LuffyAddress, luffy);
                    InternalAddAsset(ZoroAddress, zoro);
                    InternalAddAsset(SanjiAddress, sanji);
                    break;

                // Label を用いたアセットロード
                case LoadAPIType.Label:

                    sprites.Add(luffy);
                    sprites.Add(zoro);
                    sprites.Add(sanji);
                    InternalAddAsset(LuffyLabel, luffy);
                    InternalAddAsset(ZoroLabel, zoro);
                    InternalAddAsset(SanjiLabel, sanji);
                    break;

                // AssetReference を用いたアセットロード
                case LoadAPIType.AssetReference:

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
            OnePieceData data = null;
            switch (loadAPIType)
            {
                // Address を用いたアセットロード
                case LoadAPIType.Address:

                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    InternalAddAsset(DataAddress, data);
                    break;

                // Label を用いたアセットロード
                case LoadAPIType.Label:

                    names.Add(data.LuffyName);
                    names.Add(data.ZoroName);
                    names.Add(data.SanjiName);
                    InternalAddAsset(DataLabel, data);
                    break;

                // AssetReference を用いたアセットロード
                case LoadAPIType.AssetReference:

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
                _assets.Remove(key);
                Debug.Log($"Removed -> {key} : {asset}");
            }
        }
    }
}