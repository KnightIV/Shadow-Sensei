﻿using System;

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

    // PlaybackEdit Scene
    EditingMenu,
    TextDataInput,  
    ExportFinished,
    QuitEdit,

    // Recording Scene
    Recording
}
