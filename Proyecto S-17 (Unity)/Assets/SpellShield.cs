using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShield : MonoBehaviour
{
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private Animator _spellAnimator;
    [SerializeField] private KeyCode _speelKeybind = KeyCode.Alpha1;

    private void Update()
    {
        if (Input.GetKeyDown(_speelKeybind))
        {
            _characterAnimator.SetTrigger("ActivateShield");
            _spellAnimator.SetTrigger("ActivateShield");
        }
    }

}
