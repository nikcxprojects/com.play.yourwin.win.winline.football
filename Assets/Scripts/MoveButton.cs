using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TypeButton _type;
    private bool _isPressed;

    private void Update()
    {
        if (!_isPressed)
            return;

        if (_type == TypeButton.Right)
            GameManager.instance.goalkeeper.SetDirectionButton(Vector2.right);
        else if (_type == TypeButton.Left)
            GameManager.instance.goalkeeper.SetDirectionButton(Vector2.left);
       
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        GameManager.instance.goalkeeper.SetDirectionButton(Vector2.zero);
    }

    [System.Serializable]
    private enum TypeButton
    {
        Left,
        Right,
    }
}
