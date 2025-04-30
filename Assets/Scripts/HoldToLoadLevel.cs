using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class HoldToLoadLevel : MonoBehaviour
{
    public float holdTime = 1f; // Time in seconds to hold the button
    public Image fillCircle;

    private float holdTimer = 0f;
    private bool isHolding = false;

    public static event Action OnHoldComplete;
    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdTime;
            if(holdTimer >= holdTime)
            {
                //load level
                OnHoldComplete?.Invoke();
                ResetHold();
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context) {
        if (context.started)
        {
            isHolding = true;
        }
        else if (context.canceled)
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0f;
        fillCircle.fillAmount = 0f;
    }
}

