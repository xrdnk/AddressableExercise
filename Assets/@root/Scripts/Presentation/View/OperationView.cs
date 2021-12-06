using System;
using UniRx;
using UnityEngine;
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
        [SerializeField] InputField _inputFieldUnloadKey;
        [SerializeField] Button _buttonUnloadAsset;

        readonly Subject<Unit> _initializeSubject = new();
        public IObservable<Unit> OnInitializeAsObservable() => _initializeSubject;

        readonly Subject<Unit> _loadSpritesAsyncSubject = new();
        public IObservable<Unit> OnLoadSpritesAsyncAsObservable() => _loadSpritesAsyncSubject;

        readonly Subject<Unit> _loadNamesAsyncSubject = new();
        public IObservable<Unit> OnLoadNamesAsyncAsObservable() => _loadNamesAsyncSubject;

        readonly Subject<Unit> _loadModalAsyncSubject = new();
        public IObservable<Unit> OnLoadModalAsyncAsObservable() => _loadModalAsyncSubject;

        readonly Subject<Unit> _unloadModalAsyncSubject = new();
        public IObservable<Unit> OnUnloadModalAsyncAsObservable() => _unloadModalAsyncSubject;

        readonly Subject<string> unloadAssetAsObservable = new();
        public IObservable<string> OnUnloadAssetAsObservable() => unloadAssetAsObservable;

        void Awake()
        {
            _buttonInitialize.OnClickAsObservable()
                .Subscribe(_ => _initializeSubject.OnNext(Unit.Default))
                .AddTo(this);

            // NOTE: 連打するとリリースできない
            _buttonLoadSpritesAsync.OnClickAsObservable()
                .Subscribe(_ => _loadSpritesAsyncSubject.OnNext(Unit.Default))
                .AddTo(this);

            // NOTE: 連打するとリリースできない
            _buttonLoadNamesAsync.OnClickAsObservable()
                .Subscribe(_ => _loadNamesAsyncSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonLoadModalAsync.OnClickAsObservable()
                .Subscribe(_ => _loadModalAsyncSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonUnloadModalAsync.OnClickAsObservable()
                .Subscribe(_ => _unloadModalAsyncSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonUnloadAsset.OnClickAsObservable()
                .Where(_ => !string.IsNullOrEmpty(_inputFieldUnloadKey.text))
                .Subscribe(_ => unloadAssetAsObservable.OnNext(_inputFieldUnloadKey.text))
                .AddTo(this);
        }
    }
}