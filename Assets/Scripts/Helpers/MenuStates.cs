using System;

[Flags]
public enum MenuStates {

    // General
    None,
    LoadingScene,

    // MainMenu Scene
    MainMenu,
    MainMenuExit,

    // Training Scene
    LoadList,
    TrainingPreview,
    Training,
    TrainingFinished,
    QuitTraining,
    DeleteTechnique,
    TechniqueDeleted,

    // PlaybackEdit Scene
    EditingMenu,
    TextDataInput,  
    ExportFinished,
    QuitEdit,
    ErrorEdit,

    // Recording Scene
    Recording
}
