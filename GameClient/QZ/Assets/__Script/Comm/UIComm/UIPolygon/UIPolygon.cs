
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPolygon : MaskableGraphic
{
    [SerializeField]
    Texture m_Texture;
    public bool fill = true;
    public float thickness = 5;
    [Range(3, 360)]
    public int sides = 5;
    [Range(0, 360)]
    public float rotation = 0;
    [Range(0f, 1)]
    public float[] VerticesDistances = new float[3];
    private float size = 0;
    //圆心向外的偏移量，
    private float offsetCenter = 0.2f;
    protected override void Awake()
    {
        base.Awake();
        CalculateSize();
    }
    private void Update()
    {
        if (!Application.isPlaying)
            CalculateSize();
    }
    public override Texture mainTexture
    {
        get
        {
            return m_Texture == null ? s_WhiteTexture : m_Texture;
        }
    }
    public Texture texture
    {
        get
        {
            return m_Texture;
        }
        set
        {
            if (m_Texture == value) return;
            m_Texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }
    //public void DrawPolygon(int _sides)
    //{
    //    sides = _sides;
    //    VerticesDistances = new float[_sides + 1];
    //    for (int i = 0; i < _sides; i++) VerticesDistances[i] = 1; ;
    //    rotation = 0;
    //    SetVerticesDirty();
    //}
    //public void DrawPolygon(int _sides, float[] _VerticesDistances)
    //{
    //    sides = _sides;
    //    VerticesDistances = _VerticesDistances;
    //    rotation = 0;
    //    SetVerticesDirty();
    //}
    //public void DrawPolygon(int _sides, float[] _VerticesDistances, float _rotation)
    //{
    //    sides = _sides;
    //    VerticesDistances = _VerticesDistances;
    //    rotation = _rotation;
    //    SetVerticesDirty();
    //}

    //public void DrawStateBar(float[] attRatio)
    //{
    //    VerticesDistances = attRatio;
    //    SetVerticesDirty();
    //}

    public void DrawStateBar(int[] attRatio)
    {
        //VerticesDistances = new float[attRatio.Length + 1];
        //确保5变形，
        VerticesDistances = new float[sides + 1];
        int index = 0;
        for (; index < attRatio.Length; index++)
        {
            //万分比
            VerticesDistances[index] = (float)attRatio[index] / 10000;
        }
        for (; index < sides; index++)
        {
            VerticesDistances[index] = 0;
        }
        VerticesDistances[index] = (float)attRatio[0] / 10000;
        SetVerticesDirty();
    }


    private void CalculateSize()
    {
        size = rectTransform.rect.width;
        if (rectTransform.rect.width > rectTransform.rect.height)
            size = rectTransform.rect.height;
        else
            size = rectTransform.rect.width;
        thickness = (float)Mathf.Clamp(thickness, 0, size / 2);
    }
    protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
    {
        UIVertex[] vbo = new UIVertex[4];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = vertices[i];
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        Vector2 prevX = Vector2.zero;
        Vector2 prevY = Vector2.zero;
        Vector2 uv0 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(0, 1);
        Vector2 uv2 = new Vector2(1, 1);
        Vector2 uv3 = new Vector2(1, 0);
        Vector2 pos0;
        Vector2 pos1;
        Vector2 pos2;
        Vector2 pos3;
        float degrees = 360f / sides;
        int vertices = sides + 1;
        if (VerticesDistances.Length != vertices)
        {
            VerticesDistances = new float[vertices];
            for (int i = 0; i < vertices - 1; i++) VerticesDistances[i] = 1;
        }
        // last vertex is also the first!
        VerticesDistances[vertices - 1] = VerticesDistances[0];
        for (int i = 0; i < vertices; i++)
        {
            float outer = -rectTransform.pivot.x * size * (offsetCenter + (1 - offsetCenter) * VerticesDistances[i]);
            float inner = -rectTransform.pivot.x * size * (offsetCenter + (1 - offsetCenter) * VerticesDistances[i]) + thickness;
            float rad = Mathf.Deg2Rad * (i * degrees + rotation);
            float c = Mathf.Cos(rad);
            float s = Mathf.Sin(rad);
            uv0 = new Vector2(0, 1);
            uv1 = new Vector2(1, 1);
            uv2 = new Vector2(1, 0);
            uv3 = new Vector2(0, 0);
            pos0 = prevX;
            pos1 = new Vector2(outer * c, outer * s);
            if (fill)
            {
                pos2 = Vector2.zero;
                pos3 = Vector2.zero;
            }
            else
            {
                pos2 = new Vector2(inner * c, inner * s);
                pos3 = prevY;
            }
            prevX = pos1;
            prevY = pos2;
            vh.AddUIVertexQuad(SetVbo(new[] { pos0, pos1, pos2, pos3 }, new[] { uv0, uv1, uv2, uv3 }));
        }
    }
}

