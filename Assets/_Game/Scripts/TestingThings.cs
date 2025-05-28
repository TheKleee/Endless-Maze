using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using EM_Hashing;

public class TestingThings : MonoBehaviour
{

    [SerializeField] string password = "";
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            Test();
    }

    public void Test()
    {
        Debug.Log($"Password: {password}\nHash:" + password.Hash());
    }

}
