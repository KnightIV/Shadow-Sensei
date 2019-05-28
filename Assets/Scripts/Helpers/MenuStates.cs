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
    Recording,

    // Additional MainMenu Scene
    MainMenuLogin,
    MainMenuLoginResult,
    MainMenuRegister, 
    MainMenuRegisterResult,

    // Online Scene
    OnlineMenu,
    OnlineTechniquesMenu,
    OnlineUploadTechniqueMenu
}
