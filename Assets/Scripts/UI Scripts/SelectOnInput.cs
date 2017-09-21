using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

    private bool buttondSelected;
    private Selectable next;
    // Use this for initialization
    void Start () {
	}

    private void OnEnable()
    {
        eventSystem = EventSystem.current;
        // Disable selection on menu window swithces
        // otherwise bugs on tracking selection from previous window
        eventSystem.SetSelectedGameObject(null);
    }

    // Update is called once per frame
    void Update () {

        next = null;
        // TODO: Vertical inclues 'W' and 'S' keys for input
        //       this means if using input fields our navigation won't work correctly
        //       if the user is in an input box and hits up/down they should move as expected.
        if (buttondSelected == false && Input.GetAxisRaw("Vertical") != 0)
        {
            SelectDefaultButton();
        }
        // TODO: Figure out Shift + Tab handling :(
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            try
            {
                // Shift is bottom -> top
                next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            }
            catch (System.NullReferenceException)
            {
                SelectDefaultButton();
            }
        }
        // Add tabbing through menu functionality - currently requires something to be selected first
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            try
            {
                // Tab is top -> bottom
                next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            }
            catch (System.NullReferenceException)
            {
                SelectDefaultButton();
            }
        }
        if (next != null)
        {
            // Handle being able to tab from an active component to next or previous component
            InputField inputfield = next.GetComponent<InputField>();
            if (inputfield != null)
                inputfield.OnPointerClick(new PointerEventData(eventSystem));  //if it's an input field, also set the text caret

            eventSystem.SetSelectedGameObject(next.gameObject, new BaseEventData(eventSystem));
        }
    }

    private void SelectDefaultButton()
    {
        eventSystem.SetSelectedGameObject(selectedObject);
        buttondSelected = true;
    }

    private void OnDisable()
    {
        buttondSelected = false;
    }
}
