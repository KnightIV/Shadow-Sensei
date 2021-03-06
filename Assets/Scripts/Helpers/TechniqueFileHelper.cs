﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class TechniqueFileHelper {

    public static readonly string SaveFolder = $"{Application.persistentDataPath}/Techniques";
    public static readonly string MetaDataFolder = $"{Application.persistentDataPath}/TechniquesMeta";

    public static void Save(Technique t) {
        if (!Directory.Exists(SaveFolder)) {
            Directory.CreateDirectory(SaveFolder);
        }

        if (!Directory.Exists(MetaDataFolder)) {
            Directory.CreateDirectory(MetaDataFolder);
        }

        t.ownerArtistId = VariableHolder.User.UserID;

        string techniqueJson = JsonUtility.ToJson(t);
        string techniqueMetaJson = JsonUtility.ToJson((TechniqueMetaData) t);

        string techniquePath = $"{SaveFolder}/{t.TechniqueName}.ma";
        File.WriteAllText(techniquePath, techniqueJson);

        string metaDataPath = $"{MetaDataFolder}/{t.TechniqueName}.me";
        File.WriteAllText(metaDataPath, techniqueMetaJson);
    }

    /// <summary>
    /// Updates the technique in the user's file system
    /// </summary>
    /// <param name="t">The technique attempted</param>
    /// <param name="attemptScore">The user's attempt's score</param>
    /// <returns>Whether or not the new attempt was a new record</returns>
    public static bool RegisterAttempt(Technique t, float attemptScore) {
        if (!Directory.Exists(SaveFolder) || !Directory.Exists(MetaDataFolder)) {
            throw new InvalidOperationException($"SaveFolder exists: {Directory.Exists(SaveFolder)} | MetaDataFolder exists: {Directory.Exists(MetaDataFolder)}");
        }

        t.artistId = VariableHolder.User.UserID;
        bool newRecord = false;

        TechniqueMetaData meta = GetMetaData(t.TechniqueName);
        if (t.LastAttemptedDateTime > meta.LastAttemptedDateTime) {
            meta.LastAttemptedTicks = t.LastAttemptedDateTime.Ticks;
        }

        meta.LastScore = attemptScore;
        if (Mathf.RoundToInt(attemptScore.ToPercent()) > Mathf.RoundToInt(meta.BestScorePercent)) {
            meta.BestScore = attemptScore;
            newRecord = true;
        }

        string metaJson = JsonUtility.ToJson(meta);
        string metaPath = $"{MetaDataFolder}/{meta.TechniqueName}.me";

        File.WriteAllText(metaPath, metaJson);

        string techniqueJson = JsonUtility.ToJson(t);
        string techniquePath = $"{SaveFolder}/{t.TechniqueName}.ma";
        File.WriteAllText(techniquePath, techniqueJson);

        return newRecord;
    }

    public static Technique Load(string name) {
        if (!Directory.Exists(SaveFolder)) {
            throw new InvalidOperationException("could not find save directory");
        }

        string path = $"{SaveFolder}/{name}.ma";
        string techniqueJson = File.ReadAllText(path);
        Technique t = JsonUtility.FromJson<Technique>(techniqueJson);

        return t;
    }

    public static Technique LoadClean(string name) {
        Technique t = Load(name);

        t.LastAttemptedDateTime = default;
        t.Score = 0;
        t.UserAttemptFrames = null;

        return t;
    }

    public static IEnumerable<TechniqueMetaData> GetAllTechniquesMeta() {
        if (!Directory.Exists(MetaDataFolder)) {
            return new List<TechniqueMetaData>();
        }

        string[] files = Directory.GetFiles(MetaDataFolder);

        List<TechniqueMetaData> metaData = new List<TechniqueMetaData>(files.Length);
        foreach (string file in files) {
            string metaJson = File.ReadAllText(file);
            metaData.Add(JsonUtility.FromJson<TechniqueMetaData>(metaJson));
        }

        return metaData;
    }

    public static IEnumerable<TechniqueMetaData> GetAllAttemptedTechniquesMeta() {
        return GetAllTechniquesMeta().Where(m => m.HasBeenAttempted);
    }

    public static IEnumerable<TechniqueMetaData> GetCleanTechniquesMeta() {
        return GetAllTechniquesMeta().Select(m => {
            m.LastAttemptedTicks = default(DateTime).Ticks;
            return m;
        });
    }

    public static TechniqueMetaData GetMetaData(string techniqueName) {
        if (!Directory.Exists(MetaDataFolder)) {
            throw new InvalidOperationException("could not find meta directory");
        }

        string techniquePath = $"{MetaDataFolder}/{techniqueName}.me";
        if (!File.Exists(techniquePath)) {
            throw new InvalidOperationException($"{techniquePath} does not exist");
        }

        return JsonUtility.FromJson<TechniqueMetaData>(File.ReadAllText(techniquePath));
    }

    public static void DeleteTechnique(string techniqueName) {
        if (!Directory.Exists(MetaDataFolder) || !Directory.Exists(SaveFolder)) {
            throw new InvalidOperationException();
        }

        string savedTech = $"{SaveFolder}/{techniqueName}.ma";
        string meta = $"{MetaDataFolder}/{techniqueName}.me";

        if (!File.Exists(savedTech) || !File.Exists(meta)) {
            throw new InvalidOperationException("technique does not exist on this computer");
        }

        File.Delete(savedTech);
        File.Delete(meta);
    }
}
