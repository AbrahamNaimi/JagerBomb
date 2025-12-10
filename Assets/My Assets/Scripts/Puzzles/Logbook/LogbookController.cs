using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace My_Assets.Puzzles.Logbook
{
    public class LogBookPage
    {
        public string Name { get; }
        [CanBeNull] public string Title { get; }
        [CanBeNull] public string Content { get; }
        
        public LogBookPage(string name, string title, string content)
        {
            Name = name;
            Title = title;
            Content = content;
        }
    }
    
    public class LogbookController : MonoBehaviour
    {
        public Sprite transparent;
        public Sprite notebookPage;
        public Sprite notebookPageFlipped;
        public Sprite frontCover;
        public Sprite backCover;
        public GameObject bookPage;
        public GameObject leftPage;
        public GameObject rightPage;
        
        private float _height;
        private float _width;
        private List<GameObject> _pages = new ();
        private GameObject _transparentPage;
        private GameObject _emptyRightPage;
        private LogbookPageCreator _logbookPageCreator = new ();
        private int _currentPage = 0;
        

        void Start()
        {
            GetHeightAndWidth();
            GeneratePages();
            SetPages(_transparentPage, _pages[_currentPage]);
        }

        private void GetHeightAndWidth()
        {
            RectTransform pageRect = leftPage.GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            pageRect.GetWorldCorners(corners);
            
            _width = Vector3.Distance(corners[0], corners[1]);
            _height = Vector3.Distance(corners[0], corners[3]);
        }

        private void GeneratePages()
        {
            _transparentPage = _logbookPageCreator.BookPage("Transparent page", Instantiate(bookPage), transparent, _width, _height, null, null);
            _emptyRightPage = _logbookPageCreator.BookPage("Empty page", Instantiate(bookPage), notebookPage, _width, _height, null, null);
            _pages.Add(_logbookPageCreator.BookPage("Front cover", Instantiate(bookPage), frontCover, _width, _height, "<color=#ffffff>JagerLog</color>", null));
            _pages.Add(_logbookPageCreator.BookPage("Back cover", Instantiate(bookPage), backCover, _width, _height, null, null));
        }
        
        public void AddPage(LogBookPage page)
        {
            Sprite background = _pages.Count % 2 != 0 ? notebookPage : notebookPageFlipped;
            _pages.Insert(_pages.Count - 1, _logbookPageCreator.BookPage(page.Name, Instantiate(bookPage), background, _width, _height, page.Title, page.Content));
        }

        private void SetPages(GameObject lPage, GameObject rPage)
        {
            leftPage.transform.DetachChildren();
            rightPage.transform.DetachChildren();
            
            lPage.transform.SetParent(leftPage.transform, false); 
            lPage.transform.localPosition = Vector3.zero;
            lPage.transform.localScale = Vector3.one;
            
            rPage.transform.SetParent(rightPage.transform, false);
            rPage.transform.localPosition = Vector3.zero;
            rPage.transform.localScale = Vector3.one;
            
            RectTransform leftRectTransform = lPage.GetComponent<RectTransform>();
            leftRectTransform.anchorMin = Vector2.zero;
            leftRectTransform.anchorMax = Vector2.one;
            leftRectTransform.offsetMin = Vector2.zero;
            leftRectTransform.offsetMax = Vector2.zero;
            
            RectTransform rightRectTransform = rPage.GetComponent<RectTransform>();
            rightRectTransform.anchorMin = Vector2.zero;
            rightRectTransform.anchorMax = Vector2.one;
            rightRectTransform.offsetMin = Vector2.zero;
            rightRectTransform.offsetMax = Vector2.zero;
        }
        
        public void NextPage()
        {
            GameObject[] pagesToShow = PagesToShow(_currentPage + 2);
            if (pagesToShow == null) return;
            _currentPage += 2;
            SetPages(pagesToShow[0], pagesToShow[1]);
        }

        public void PreviousPage()
        {
            GameObject[] pagesToShow = PagesToShow(_currentPage - 2);
            if (pagesToShow == null) return;
            _currentPage -= 2;
            SetPages(pagesToShow[0], pagesToShow[1]);
        }

        [CanBeNull]
        private GameObject[] PagesToShow(int pageNumber)
        {
            if (pageNumber < 0) return null;
            
            if (pageNumber == 0)
            {
                return new []{_transparentPage, _pages[pageNumber]};
            }
            
            if (_pages.Count % 2 != 0 && _pages.Count + 1 == pageNumber )
            {
                return new []{_pages[pageNumber - 2], _transparentPage};
            }
            
            if (pageNumber == _pages.Count )
            {
                return new []{_pages[pageNumber - 1], _transparentPage};
            }

            if (pageNumber + 1 == _pages.Count && _pages.Count % 2 != 0)
            {
                return new []{_pages[pageNumber - 1], _emptyRightPage};
            }

            if (pageNumber + 1 >= _pages.Count)
            {
                return null;
            }
            
            return new []{_pages[pageNumber - 1], _pages[pageNumber]};
        }
    }
}