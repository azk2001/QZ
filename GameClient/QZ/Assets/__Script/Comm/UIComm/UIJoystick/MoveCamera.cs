using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine;

public class MoveCamera : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        MovePlayer(eventData.delta);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MovePlayer(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MovePlayer(Vector2.zero);
    }

    private CameraLookPlayer lookPlayer = null;
    void Start () {
		if(lookPlayer == null)
        {
            lookPlayer = CameraLookPlayer.Instance;
        }
	}
	
	private void MovePlayer (Vector2 offset)
    {
        if (lookPlayer == null)
            return;

        lookPlayer.eulerAngles += offset.x*0.5f; 
        lookPlayer.curHight += offset.y*0.02f;

        lookPlayer.SetPlayerForward();
    }
}
