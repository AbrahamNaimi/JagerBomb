using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace My_Assets.Puzzles.Logbook
{
    public class LogbookPageCreator
    {
        private float _contentFontSize = 22.2f;
        private float _titleFontSize = 36f;
        private float _titlePageFontSize = 50f;

        /// <summary>
        /// Title string length should not exceed 80 characters after formatting. <br /> Content string length should not exceed 900 characters.
        /// </summary>
        /// <returns></returns>
        public GameObject BookPage(string name, Sprite background, float width, float height, [CanBeNull] string title,
            [CanBeNull] string content)
        {
            GameObject page = new GameObject(name);
            Image imageComponent = page.AddComponent<Image>();
            imageComponent.sprite = background;

            RectTransform rectTransform = page.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.anchoredPosition = new Vector2(0, 0);

            bool hasTitle = title != null;
            bool hasContent = content != null;

            if (hasTitle)
            {
                GameObject titleTM = CreateTitle(hasContent, title);
                titleTM.transform.SetParent(page.transform, false);

                RectTransform titleRectTransform = titleTM.GetComponent<RectTransform>();
                Vector2 anchorpoint = hasContent ? new Vector2(0, height * 0.8f) : new Vector2(0, height * 0.37f);
                titleRectTransform.anchoredPosition = anchorpoint;

                Vector2 sizeDelta = new Vector2(width * 0.7f, height * 0.2f);
                titleRectTransform.sizeDelta = sizeDelta;
            }

            if (hasContent)
            {
                GameObject contentObject = CreateContent(content);
                contentObject.transform.SetParent(page.transform, false);

                RectTransform contentRectTransform = contentObject.GetComponent<RectTransform>();
                Vector2 anchorpoint = hasTitle ? new Vector2(0, height * -0.065f) : new Vector2(0, 0);
                contentRectTransform.anchoredPosition = anchorpoint;

                Vector2 sizeDelta = new Vector2(width * 0.7f, height * 0.75f);
                contentRectTransform.sizeDelta = sizeDelta;
            }

            return page;
        }

        private GameObject CreateTitle(bool hasContent, string title)
        {
            GameObject titleObject = new GameObject("Title");
            TextMeshProUGUI textComponent = titleObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = title;
            textComponent.color = Color.black;
            textComponent.alignment = TextAlignmentOptions.Center;
            float textSize = hasContent ? _titleFontSize : _titlePageFontSize;
            textComponent.fontSize = textSize;

            return titleObject;
        }

        private GameObject CreateContent(string content)
        {
            GameObject contentObject = new GameObject("Content");
            TextMeshProUGUI textComponent = contentObject.AddComponent<TextMeshProUGUI>();
            textComponent.text = content;
            textComponent.color = Color.black;
            textComponent.alignment = TextAlignmentOptions.Center;
            textComponent.fontSize = _contentFontSize;

            return contentObject;
        }
    }
}