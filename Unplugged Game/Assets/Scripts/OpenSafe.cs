using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenSafe : MonoBehaviour
{
    private Animator animator;

    private bool IsAtSafe = false;  

    [SerializeField] private TextMeshProUGUI codeText;
    string code = ""; 
    public string correctCode;
    public GameObject codePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        codeText.text = code;

        if (code == correctCode)
        {
            animator.SetTrigger("OpenSafe");
            codePanel.SetActive(false);
        }

        if (code.Length >= 4)
        {
            code = "";
        }

        if (Input.GetKeyDown(KeyCode.E) && IsAtSafe == true)
        {
            codePanel.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsAtSafe = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
            IsAtSafe = false;
        codePanel.SetActive(false);
    }

    public void AddDigit(string digit)
    {
        if (IsAtSafe)
        {
            code += digit;
        }
    }
}
