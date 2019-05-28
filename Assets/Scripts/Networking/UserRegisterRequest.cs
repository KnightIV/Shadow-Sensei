using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserRegisterRequest {

    public string name;
    public string username;
    public string password;
    public string email;

    public UserRegisterRequest () { }

    public UserRegisterRequest(string name, string username, string password, string email) {
        this.name = name;
        this.username = username;
        this.password = password;
        this.email = email;
    }
}
