using System;
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

        string techniqueJson = JsonUtility.ToJson(t);
        string techniqueMetaJson = JsonUtility.ToJson((TechniqueMetaData) t);

        string techniquePath = $"{SaveFolder}/{t.TechniqueName}.ma";
        File.WriteAllText(techniquePath, techniqueJson);

        string metaDataPath = $"{MetaDataFolder}/{t.TechniqueName}.me";
        File.WriteAllText(metaDataPath, techniqueMetaJson);
    }

    public static void RegisterAttempt(Technique t, float attemptScore) {
        if (!Directory.Exists(SaveFolder) || !Directory.Exists(MetaDataFolder)) {
            throw new InvalidOperationException($"SaveFolder exists: {Directory.Exists(SaveFolder)} | MetaDataFolder exists: {Directory.Exists(MetaDataFolder)}");
        }

        TechniqueMetaData meta = GetMetaData(t.TechniqueName);
        if (t.LastAttemptedDateTime > meta.LastAttemptedDateTime) {
            meta.LastAttemptedTicks = t.LastAttemptedDateTime.Ticks;
        }

        meta.LastScore = attemptScore;
        meta.BestScore = Math.Max(meta.BestScore, attemptScore);

        string metaJson = JsonUtility.ToJson(meta);
        string metaPath = $"{MetaDataFolder}/{meta.TechniqueName}.me";

        File.WriteAllText(metaPath, metaJson);
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

        t.LastAttemptedDateTime = default(DateTime);
        t.Score = 0;
        t.UserAttemptFrames = null;

        return t;
    }

    public static IEnumerable<TechniqueMetaData> GetAllTechniquesMeta() {
        if (!Directory.Exists(MetaDataFolder)) {
            throw new InvalidOperationException("could not find meta directory");
        }

        string[] files = Directory.GetFiles(MetaDataFolder);

        List<TechniqueMetaData> metaData = new List<TechniqueMetaData>(files.Length);
        foreach (string file in files) {
            string metaJson = File.ReadAllText(file);
            metaData.Add(JsonUtility.FromJson<TechniqueMetaData>(metaJson));
        }

        return metaData;
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
}
