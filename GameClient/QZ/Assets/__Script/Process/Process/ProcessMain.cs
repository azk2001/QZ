using UnityEngine.SceneManagement;

class ProcessMain : ProcessBase
{
    public override void OnBegin()
    {
        SceneManager.LoadScene("mainGame", LoadSceneMode.Single);

        base.OnBegin();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }
}