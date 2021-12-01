using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Deniverse.AddressableExercise.View
{
    public sealed class OnePieceView : MonoBehaviour
    {
        [SerializeField] List<Image> _images;
        [SerializeField] List<Text> _texts;

        readonly List<Sprite> _sprites = new(3);
        readonly List<string> _names = new(3);

        public void SetImages(IReadOnlyList<Sprite> sprites)
        {
            for (var i = 0; i < _sprites.Count ; i++)
            {
                _sprites[i] = sprites[i];
                _images[i].sprite = _sprites[i];
            }
        }

        public void SetName(IReadOnlyList<string> names)
        {
            for (var i = 0; i < _names.Count; i++)
            {
                _names[i] = names[i];
                _texts[i].text = _names[i];
            }
        }
    }
}