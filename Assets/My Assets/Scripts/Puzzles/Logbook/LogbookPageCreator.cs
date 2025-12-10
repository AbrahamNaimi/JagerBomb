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
        public GameObject BookPage(string name, GameObject page, Sprite background, float width, float height,
            [CanBeNull] string title,
            [CanBeNull] string content)
        {
            SetPageBase(name, page, background, width, height);

            bool hasTitle = title != null;
            bool hasContent = content != null;

            if (hasTitle)
            {
                GameObject titleGameobject = page.transform.Find("Title").gameObject;
                SetTitle(titleGameobject.GetComponent<TextMeshProUGUI>(), hasContent, title);
                SetTitleLayout(titleGameobject.GetComponent<RectTransform>(), width, height, hasContent);
            }

            if (hasContent)
            {
                GameObject contentGameobject = page.transform.Find("Content").gameObject;
                SetContent(contentGameobject.GetComponent<TextMeshProUGUI>(), content);
                SetContentLayout(contentGameobject.GetComponent<RectTransform>(), width, height, hasTitle);
            }

            return page;
        }

        private void SetPageBase(string name, GameObject page, Sprite background, float width, float height)
        {
            page.name = name;
            Image imageComponent = page.GetComponent<Image>();
            RectTransform rectTransform = page.GetComponent<RectTransform>();
            imageComponent.sprite = background;
            rectTransform.sizeDelta = new Vector2(width, height);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }

        private void SetTitle(TextMeshProUGUI textMesh, bool hasContent, string title)
        {
            textMesh.text = title;
            textMesh.color = Color.black;
            textMesh.alignment = TextAlignmentOptions.Center;
            float textSize = hasContent ? _titleFontSize : _titlePageFontSize;
            textMesh.fontSize = textSize;
        }

        private void SetTitleLayout(RectTransform titleRectTransform, float width, float height, bool hasContent)
        {
            Vector2 anchorpoint = hasContent ? new Vector2(0, height * 0.8f) : new Vector2(0, height * 0.37f);
            titleRectTransform.anchoredPosition = anchorpoint;

            Vector2 sizeDelta = new Vector2(width * 0.7f, height * 0.2f);
            titleRectTransform.sizeDelta = sizeDelta;
        }

        private void SetContent(TextMeshProUGUI textMesh, string content)
        {
            textMesh.text = content;
            textMesh.color = Color.black;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.fontSize = _contentFontSize;
        }

        private void SetContentLayout(RectTransform contentRectTransform, float width, float height, bool hasTitle)
        {
            Vector2 anchorpoint = hasTitle ? new Vector2(0, height * -0.065f) : new Vector2(0, 0);
            contentRectTransform.anchoredPosition = anchorpoint;

            Vector2 sizeDelta = new Vector2(width * 0.7f, height * 0.75f);
            contentRectTransform.sizeDelta = sizeDelta;
        }
    }
}