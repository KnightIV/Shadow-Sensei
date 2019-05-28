using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class APIHelper {

    private const string BASE_URL = "http://localhost:8080";

    public static UnityWebRequest AttemptLogIn(string username, string password) {
        return MakeGetRequest("user", username, password);
    }

    public static UnityWebRequest Register(UserRegisterRequest userRegister) {
        string artistJson = JsonUtility.ToJson(userRegister);
        artistJson = artistJson[0] + "\"@type\": \"MartialArtist\"," + artistJson.Substring(1, artistJson.Length - 1);

        return MakePostRequest("user", artistJson);
    }

    public static UnityWebRequest CreateTechnique(Technique t) {
        return MakePostRequest("technique", JsonUtility.ToJson(t), VariableHolder.User.Username, VariableHolder.User.Password);
    }

    public static UnityWebRequest SendAttempt(Technique t) {
        return MakePostRequest("technique/attempt", JsonUtility.ToJson(t), VariableHolder.User.Username, VariableHolder.User.Password);
    }

    private static UnityWebRequest MakeGetRequest(string path, string username = null, string password = null) {
        return MakeRequest(path, "GET", username, password);
    }

    private static UnityWebRequest MakePostRequest(string path, string bodyJson = null, string username = null, string password = null) {
        return MakeRequest(path, "POST", username, password, bodyJson);
    }

    private static UnityWebRequest MakeRequest(string path, string verb, string username = null, string password = null, string bodyJson = null) {
        string finalPath = $"{BASE_URL}/{path}";
        UnityWebRequest request = new UnityWebRequest(finalPath, verb) {
            downloadHandler = new DownloadHandlerBuffer()
        };

        if (username != null && password != null) {
            request.SetRequestHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
        }
        if (!string.IsNullOrWhiteSpace(bodyJson)) {
            byte[] jsonBytes = new UTF8Encoding().GetBytes(bodyJson);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.SetRequestHeader("Content-Type", "application/json");
        }

        return request;
    }
}
