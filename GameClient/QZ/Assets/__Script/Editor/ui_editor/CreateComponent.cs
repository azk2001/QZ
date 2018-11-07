using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CreateComponent : Editor
{
    [MenuItem("GameObject/UI/Image _#&_s")]
    public static void CreateNewImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Img_");
                go.AddComponent<UIImage>().raycastTarget = false;
                SetParent(go);
                Undo.RegisterCreatedObjectUndo(go, "createImg" + go.name);
            }
        }
    }

    private static void SetParent(GameObject go)
    {
        if (go != null)
        {
            go.transform.SetParent(Selection.activeTransform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;
            Selection.activeGameObject = go;
        }
    }

}
