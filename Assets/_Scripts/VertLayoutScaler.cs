using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class VertLayoutScaler : MonoBehaviour
{
    private void Awake()
    {
        _verticalLayout = GetComponent<VerticalLayoutGroup>();
        _rect = GetComponent<RectTransform>();
        OnTransformChildrenChanged();
    }

    // [Unity doc] This function is called when the list of children of the transform of the GameObject has changed. 
    private void OnTransformChildrenChanged()
    {
        if (transform.childCount > 0)
        {
            _childRect = transform.GetChild(0).GetComponent<RectTransform>();
            ScaleRectTransform_TightenGridBounds();
            return;
        }
        _childRect = null;
    }

    private void ScaleRectTransform_TightenGridBounds()
    {
        UnityEngine.Assertions.Assert.IsNotNull(_verticalLayout);
        if (transform.childCount == 0)
            return;
        var myFirstChild = transform.GetChild(0);
        if (myFirstChild == null)
            return;

        var childHeight = _childRect.rect.height * _childRect.localScale.y;
        float numRows = transform.childCount;
        float totalHeight = (childHeight + _verticalLayout.spacing) * numRows;
        _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }

    private VerticalLayoutGroup _verticalLayout;
    private RectTransform _rect;
    private RectTransform _childRect;
}