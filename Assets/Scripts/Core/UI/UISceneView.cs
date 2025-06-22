using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public abstract class UISceneView : MonoBehaviour
    {
        private List<UIView> views;

        private void Awake()
        {
            Initialized();
        }

        private void Start()
        {
            
        }

        protected virtual void Initialized() {
            CollectUIView();
        }

        private void CollectUIView() { 
            var allViews = GetComponentsInChildren<UIView>(true);
            views = new List<UIView>(allViews.Length);
            foreach (var view in allViews)
            {
                if (!views.Contains(view))
                {
                    views.Add(view);
                }
            }
        }

        public T ShowView<T>(bool isHideOtherView = true) where T : UIView
        {
            T targetView = null;
            foreach (var view in views)
            {
                if (view is T tView)
                {
                    targetView = tView;
                    tView.Show();
                    continue;
                }

                if (isHideOtherView)
                {
                    view.Hide();
                }
            }

            return targetView;
        }

        public void HideView<T>() where T : UIView
        {
            foreach (var view in views)
            {
                if (view is T tView)
                {
                    tView.Hide();
                    return;
                }
            }
        }

        public void HideAllViews()
        {
            foreach (var view in views)
            {
                view.Hide();
            }
        }
    }
}
