using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Deniverse.AddressableExercise.View
{
    public sealed class OperationView : MonoBehaviour
    {
        [SerializeField] Button _buttonInitialize;
        [SerializeField] Button _buttonLoadSpritesAsync;
        [SerializeField] Button _buttonLoadNamesAsync;
        // [SerializeField] InputField _inputFieldUnloadKey;
        // [SerializeField] Button _buttonUnload;

        readonly Subject<Unit> _initializeSubject = new();
        public IObservable<Unit> OnInitializeAsObservable() => _initializeSubject;

        readonly Subject<Unit> _loadSpritesAsyncSubject = new();
        public IObservable<Unit> OnLoadSpritesAsyncAsObservable() => _loadSpritesAsyncSubject;

        readonly Subject<Unit> _loadNamesAsyncSubject = new();
        public IObservable<Unit> OnLoadNamesAsyncAsObservable() => _loadNamesAsyncSubject;

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
        }
    }
}