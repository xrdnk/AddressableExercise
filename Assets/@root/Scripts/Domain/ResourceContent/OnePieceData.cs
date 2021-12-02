using System;
using UnityEngine;

namespace Deniverse.AddressableExercise.Domain.ResourceContent
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(OnePieceData), menuName = nameof(OnePieceData), order = 0)]
    public class OnePieceData : ScriptableObject
    {
        [SerializeField] string _luffyName = "ルフィ";
        [SerializeField] string _zoroName = "ゾロ";
        [SerializeField] string _sanjiName = "サンジ";

        public string LuffyName => _luffyName;
        public string ZoroName => _zoroName;
        public string SanjiName => _sanjiName;
    }
}