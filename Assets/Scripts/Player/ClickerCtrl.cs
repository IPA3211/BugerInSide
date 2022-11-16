using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerCtrl : MonoBehaviour
{
    public ClickTrigger _otherTrigger;
    private void OnTriggerEnter2D(Collider2D other)
    {
        _otherTrigger = other.gameObject.GetComponent<ClickTrigger>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _otherTrigger = other.gameObject.GetComponent<ClickTrigger>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _otherTrigger = null;
    }
}
