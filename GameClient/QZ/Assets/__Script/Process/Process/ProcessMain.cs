using UnityEngine.SceneManagement;
using UnityEngine;

class ProcessMain : ProcessBase
{
    public override void OnBegin()
    {
        SceneManager.LoadScene("mainGame");

      
       GameUnit gameUnit= GameUnitManager.Instance.CreateServerGameUnit(BattleProtocol.UUID, new GameUnitParam(),new GameUnitData());

        PlayerController.Instance.AddPlayerController(gameUnit);

        base.OnBegin();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }
}