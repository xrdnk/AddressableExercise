using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Deniverse.AddressableExercise.View
{
    public sealed class OnePieceView : MonoBehaviour
    {
        [SerializeField] List<Image> _images;
        [SerializeField] List<Text> _texts;

        public void SetImages(IReadOnlyList<Sprite> sprites)
        {
            for (var i = 0; i < sprites.Count ; i++)
            {
                _images[i].sprite = sprites[i];
            }
        }

        public void SetNames(IReadOnlyList<string> names)
        {
            for (var i = 0; i < names.Count; i++)
            {
                _texts[i].text = names[i];
            }
        }
    }
}