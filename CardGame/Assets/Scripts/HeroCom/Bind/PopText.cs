using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class PopText : SightEffect
    {
        public TextMeshPro TextUI;
        
        public void InitText(string textValue)
        {
            TextUI.text = textValue;
            transform.localPosition = Vector3.zero;
            PlayAnim();
        }

        private void PlayAnim()
        {
            GetComponent<RectTransform>().DOAnchorPosY(2, Duration).SetEase(Ease.Linear);
        }
    }
}