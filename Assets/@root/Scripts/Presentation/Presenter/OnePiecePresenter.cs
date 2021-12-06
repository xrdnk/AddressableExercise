using System.Threading;
using Cysharp.Threading.Tasks;
using Deniverse.AddressableExercise.Domain.ResourceContent;
using Deniverse.AddressableExercise.Presentation.View;
using UniRx;
using Zenject;

namespace Deniverse.AddressableExercise.Presentation.Presenter
{
    public sealed class OnePiecePresenter : IInitializable
    {
        readonly IResourceLoader _resourceLoader;
        readonly OnePieceView _onePieceView;
        readonly OperationView _operationView;
        readonly CompositeDisposable _disposable;

        public OnePiecePresenter
        (
            IResourceLoader resourceLoader,
            OnePieceView onePieceView,
            OperationView operationView
        )
        {
            _resourceLoader = resourceLoader;
            _onePieceView = onePieceView;
            _operationView = operationView;
            _disposable = new CompositeDisposable();
        }

        void IInitializable.Initialize()
        {
            _operationView.OnInitializeAsObservable()
                .Subscribe(_ => _resourceLoader.InitializeAsync())
                .AddTo(_disposable);

            _operationView.OnLoadSpritesAsyncAsObservable()
                .Subscribe( _ =>
                {
                    UniTask.Void(async () =>
                    {
                        var cts = new CancellationTokenSource();
                        var sprites = await _resourceLoader.LoadSpritesAsync(cts.Token);
                        _onePieceView.SetImages(sprites);
                    });
                })
                .AddTo(_disposable);

            _operationView.OnLoadNamesAsyncAsObservable()
                .Subscribe(_ =>
                {
                    UniTask.Void(async () =>
                    {
                        var cts = new CancellationTokenSource();
                        var names = await _resourceLoader.LoadNamesAsync(cts.Token);
                        _onePieceView.SetNames(names);
                    });
                })
                .AddTo(_disposable);

            _operationView.OnLoadModalAsyncAsObservable()
                .Subscribe(_ =>
                {
                    UniTask.Void(async () =>
                    {
                        var cts = new CancellationTokenSource();
                        await _resourceLoader.LoadModalAsync(cts.Token);
                    });
                })
                .AddTo(_disposable);

            _operationView.OnUnloadModalAsyncAsObservable()
                .Subscribe(_ => _resourceLoader.UnloadModal())
                .AddTo(_disposable);

            _operationView.OnUnloadAssetAsObservable()
                .Subscribe(key => _resourceLoader.Unload(key))
                .AddTo(_disposable);
        }
    }
}