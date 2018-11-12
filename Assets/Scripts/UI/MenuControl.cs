using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct StateLayout {

    public MenuStates State;
    public GameObject Layout;
}

public class MenuControl : MonoBehaviour {

    public MenuStates CurrentState;
    public StateLayout[] StatesToLayout;

    private int curStateIndex;
    private Stack<int> prevStateIndexes;
    private List<int> curActiveStatesIndexes;

    void Start() {
        if (CurrentState == MenuStates.None) {
            CurrentState = StatesToLayout[0].State;
        }

        foreach (StateLayout stateLayout in StatesToLayout) {
            bool active = stateLayout.State == CurrentState;
            stateLayout.Layout.SetActive(active);
        }

        prevStateIndexes = new Stack<int>();
        curActiveStatesIndexes = new List<int> {
            IndexOf(CurrentState)
        };
    }

    [EnumAction(typeof(MenuStates))]
    public void OnStateChanged(int nextState) {
        OnStateChanged((MenuStates) nextState);
    }

    [EnumAction(typeof(MenuStates))]
    public void OnStateChangedVisible(int nextState) {
        OnStateChanged((MenuStates) nextState, false);
    }

    public void OnStateChanged(MenuStates nextState, bool shouldTurnInvisible = true) {
        int nextIndex = IndexOf(nextState);
        StatesToLayout[nextIndex].Layout.SetActive(true);

        if (shouldTurnInvisible) {
            foreach (int activeIndex in curActiveStatesIndexes.Where(i => i != nextIndex)) {
                StatesToLayout[activeIndex].Layout.SetActive(false);
            }

            curActiveStatesIndexes.Clear();
        }

        prevStateIndexes.Push(curStateIndex);

        CurrentState = nextState;
        curStateIndex = IndexOf(CurrentState);
        curActiveStatesIndexes.Add(curStateIndex);
    }

    public void GoToPreviousState() {
        if (prevStateIndexes.Any()) {
            OnStateChanged(StatesToLayout[prevStateIndexes.Pop()].State);
        }
    }

    private int IndexOf(MenuStates state) {
        for (int i = 0; i < StatesToLayout.Length; i++) {
            if (StatesToLayout[i].State == state) {
                return i;
            }
        }

        throw new InvalidOperationException("state not found in options");
    }
}
