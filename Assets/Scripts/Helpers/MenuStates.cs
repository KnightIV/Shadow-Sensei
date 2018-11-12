using System;

[Flags]
public enum MenuStates {

    None,

    // MainMenu Scene
    MainMenu,
    MainMenuExit,

    // Training Scene
    LoadList,
    TrainingPreview,
    Training,
    TrainingFinished,
    QuitTraining,

    // PlaybackEdit Scene
    EditingMenu,
    TextDataInput,  
    ExportFinished,
    QuitEdit
}
