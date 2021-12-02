using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Deniverse.AddressableExercise.Presentation.View
{
    public sealed class OperationView : MonoBehaviour
    {
        [SerializeField] Button _buttonInitialize;
        [SerializeField] Button _buttonLoadSpritesAsync;
        [SerializeField] Button _buttonLoadNamesAsync;
        [SerializeField] Button _buttonLoadModalAsync;
        [SerializeField] Button _buttonUnloadModalAsync;
        // [SerializeField] InputField _inputFieldUnloadKey;
        // [SerializeField] Button _buttonUnload;

        readonly Subject<Unit> _initializeSubject = new();
        public IObservable<Unit> OnInitializeAsObservable() => _initializeSubject;

        readonly Subject<Unit> _loadSpritesAsyncSubject = new();
        public IObservable<Unit> OnLoadSpritesAsyncAsObservable() => _loadSpritesAsyncSubject;

        readonly Subject<Unit> _loadNamesAsyncSubject = new();
        public IObservable<Unit> OnLoadNamesAsyncAsObservable() => _loadNamesAsyncSubject;

        const string ModalAddress = "Modal";
        SceneInstance _sceneInstance;

        void Awake()
        {
            _buttonInitialize.OnClickAsObservable()
                .Subscribe(_ => _initializeSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonLoadSpritesAsync.OnClickAsObservable()
                .Subscribe(_ => _loadSpritesAsyncSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonLoadNamesAsync.OnClickAsObservable()
                .Subscribe(_ => _loadNamesAsyncSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonLoadModalAsync.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    UniTask.Void(async () =>
                    {
                        _sceneInstance = await Addressables.LoadSceneAsync(ModalAddress, LoadSceneMode.Additive);
                    });
                })
                .AddTo(this);

            _buttonUnloadModalAsync.OnClickAsObservable()
                .Subscribe(_ => Addressables.UnloadSceneAsync(_sceneInstance))
                .AddTo(this);
        }
    }
}