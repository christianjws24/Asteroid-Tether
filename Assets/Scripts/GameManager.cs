using System;

// Renombrado a GameStateManager para evitar conflictos con nombres nativos de Unity
public static class GameStateManager
{
    private static bool isGameOver;
    public static event Action<bool> OnGameOverChanged;

    public static bool IsGameOver => isGameOver;

    public static void SetGameOver(bool value)
    {
        if (isGameOver == value) return;
        isGameOver = value;
        OnGameOverChanged?.Invoke(isGameOver);
    }

    public static void Reset()
    {
        SetGameOver(false);
    }
}
