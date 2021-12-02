using System;
using Deniverse.AddressableExercise.Domain.Infra.ResourceContent;
using Deniverse.AddressableExercise.Domain.ResourceContent;
using Deniverse.AddressableExercise.Presentation.Presenter;
using Deniverse.AddressableExercise.Presentation.View;
using UnityEngine;
using Zenject;

namespace Deniverse.AddressableExercise.DIInstaller
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] LoaderType _loaderType = LoaderType.ResourcesLoad;

        public override void InstallBindings()
        {
            switch (_loaderType)
            {
                case LoaderType.ResourcesLoad:
                    Container
                        .Bind<IResourceLoader>()
                        .To<LocalResourceLoader>()
                        .FromComponentsInHierarchy()
                        .AsSingle();
                    break;
                case LoaderType.AddressablesLoad:
                    Container
                        .Bind<IResourceLoader>()
                        .To<AddressablesLoader>()
                        .FromComponentsInHierarchy()
                        .AsSingle();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Container.BindInterfacesAndSelfTo<OnePiecePresenter>().AsSingle();

            Container.Bind<OnePieceView>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<OperationView>().FromComponentsInHierarchy().AsSingle();
        }
    }
}