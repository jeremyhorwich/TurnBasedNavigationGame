using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance { get; private set; }
    public int CurrentTurnNumber { get; private set; }
    public int TurnsRemaining { get; private set; }
    public Enums.TurnPhase CurrentPhase { get; private set; }

    public event Action OnTurnBegin;
    public event Action OnExitCheck;
    public event Action OnProjectileTurn;
    public event Action OnEnemyTurn;
    public event Action OnNPCTurn;

    [SerializeField] private float turnDisplayDelay = 0.3f;

    private void Awake()
    {
        //Protect Singleton
        if (instance != null && instance != this) Destroy(this);
        else instance = this;

        //Initialize turn counts
        CurrentTurnNumber = 1;
        TurnsRemaining = 500;

        //Start in enemy phase
        CurrentPhase = Enums.TurnPhase.enemyPhase;
    }

    private void Start()
    {
        StartRoom();
    }

    private void StartRoom()
    {
        CalculateEnemyPhase();
        CommandManager.instance.ClearCommandHistory();
        EnablePlayerPhase();
    }

    private void LoopThroughNonPlayerActions()
    {
        if (CalculateExit())
        {
            StartRoom();
            return;
        }

        CalculateNPCPhase();
        CalculateEnemyPhase();
        CalculateEnvironmentPhase();
        //Invoke("CalculateEnvironmentPhase", turnDisplayDelay);

        CommandManager.instance.CacheTurnCommands();

        EnablePlayerPhase();
    }

    private void EnablePlayerPhase()
    {
        CurrentPhase = Enums.TurnPhase.playerPhase;
        OnTurnBegin?.Invoke();
    }

    public void EndPlayerPhase()
    {
        CurrentTurnNumber += 1;
        TurnsRemaining -= 1;
        CurrentPhase = Enums.TurnPhase.environmentPhase;
        Invoke("LoopThroughNonPlayerActions", turnDisplayDelay);
    }

    private void CalculateNPCPhase() => OnNPCTurn?.Invoke();

    private void CalculateEnvironmentPhase()
    {
        //Loop through terrain actions
        OnProjectileTurn?.Invoke();
        //Loop through item actions
    }
    private void CalculateEnemyPhase()
    {
        OnEnemyTurn?.Invoke();
        CurrentPhase = Enums.TurnPhase.environmentPhase;
    }

    private bool CalculateExit()
    {
        OnExitCheck?.Invoke();

        //We clear the command history when we exit, so we can check if an exit has been found from that
        return (CommandManager.instance.turnCommands.Count == 0);
    }

    public void RewindTurn()
    {
        CurrentTurnNumber -= 1;
    }

    IEnumerator DelayForTime(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);
        method();
    }
}
