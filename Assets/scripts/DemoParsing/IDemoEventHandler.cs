public interface IDemoEventHandler {
    void OnHeaderParsed(object sender, object ea);
    void OnRoundStart(object sender, object ea);
    void OnRoundEnd(object sender, object ea);
    void OnTickDone(object sender, object ea);
    void OnWeaponFired(object sender, object ea);
    void OnPlayerHurt(object sender, object ea);
    void OnPlayerTeam(object sender, object ea);
    void OnSmokeNadeStarted(object sender, object ea);
}
