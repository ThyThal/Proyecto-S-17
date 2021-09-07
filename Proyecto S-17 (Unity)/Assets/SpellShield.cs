using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShield : MonoBehaviour
{
    ThirdPersonMovement controller;

    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private Animator _spellAnimator;
    [SerializeField] public ParticleSystem _particlesRings;
    [SerializeField] private KeyCode _spellKeybind = KeyCode.Alpha1;
    [SerializeField] private KeyCode _spellCancel = KeyCode.Space;
    [SerializeField] private Transform _intersectionArea;
    [SerializeField] private GameObject _explosionPrefab;

    [SerializeField] private bool isProtected = false;
    [SerializeField] [Range(0,10)] private int absorbed = 0;

    private void Start()
    {
        controller = GetComponent<ThirdPersonMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_spellKeybind))
        {
            CreateShield();
            controller.OnDisable();
        }

        if (Input.GetKeyDown(_spellCancel))
        {
            ExplodeShield();
            controller.OnEnable();
        }

        if (isProtected)
        {
            ExpandShield();

            if (_intersectionArea.transform.localScale.x > 14 && absorbed == 10)
            {
                ExplodeShield();
            }
        }
    }

    private void CreateShield()
    {
        _intersectionArea.transform.localScale = Vector3.one * 4f;

        _characterAnimator.SetTrigger("ActivateShield");
        _spellAnimator.SetTrigger("ActivateShield");
        _particlesRings.Play();
        isProtected = true;
    }
    private void ExpandShield()
    {
        if (_intersectionArea.transform.localScale.x < 4 + absorbed)
        {
            _intersectionArea.transform.localScale += Vector3.one * 1f * Time.deltaTime;
        }
    }

    private void ExplodeShield()
    {
        if (isProtected)
        {
            var a =Instantiate(_explosionPrefab);
            Destroy(a, 2f);
            _characterAnimator.SetTrigger("ExplodeShield");
            _spellAnimator.SetTrigger("DeactivateShield");
            Debug.Log("EPLOTAR!!!");
            isProtected = false;
            absorbed = 0;
        }
    }

    private void Absorb()
    {
        absorbed += 1;
    }
}