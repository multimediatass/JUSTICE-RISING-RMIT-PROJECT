using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputFieldTabNavigation : MonoBehaviour
{
    public TMP_InputField[] inputFields; // Array of your input fields

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NavigateThroughInputs();
        }
    }

    void NavigateThroughInputs()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current != null && current.GetComponent<TMP_InputField>() != null)
        {
            for (int i = 0; i < inputFields.Length; i++)
            {
                if (current == inputFields[i].gameObject)
                {
                    int nextIndex = (i + 1) % inputFields.Length;
                    EventSystem.current.SetSelectedGameObject(inputFields[nextIndex].gameObject, null);
                    inputFields[nextIndex].OnPointerClick(new PointerEventData(EventSystem.current));
                    break;
                }
            }
        }
    }
}
