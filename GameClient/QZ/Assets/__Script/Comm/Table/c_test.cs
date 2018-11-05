using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace MXEngine
{
    public class cs_mapnpc
    {
        public int key { get; private set; }
        public int name;
        public Vector3 desc1;
        public float[] desc2;
        public Color desc3;
        public Vector2[] descList;

        public static bool ContainsThis(int key)
        {
            return infoDic.ContainsKey(key);
        }

        public static cs_mapnpc TryGetThis(int key)
        {
            if (infoDic.ContainsKey(key))
                return infoDic[key];

            Debuger.LogError("cs_mapnpc table found no key: " + key);
            return null;
        }

        public static bool TryAddThis(int key, cs_mapnpc p)
        {
            if (infoDic.ContainsKey(key))
            {
                Debuger.LogError("cs_mapnpc table key repeat, key: " + key);
                return false;
            }
            p.key = key;
            infoDic.Add(key, p);
            return true;
        }

        public static bool TryRemoveThis(int key)
        {
            if (!infoDic.ContainsKey(key))
            {
                Debuger.LogError("cs_mapnpc table found no key, key: " + key);
                return false;
            }
            infoDic.Remove(key);
            return true;
        }

        public static Dictionary<int, cs_mapnpc> infoDic { private set; get; }

        static cs_mapnpc()
        { infoDic = new Dictionary<int, cs_mapnpc>(); }

        public static void Serialize(Table table)
        {
            int keyIndex = table.TryGetColIndex("key");
            int nameIndex = table.TryGetColIndex("name");
            int desc1Index = table.TryGetColIndex("desc1");
            int desc2Index = table.TryGetColIndex("desc2");
            int desc3Index = table.TryGetColIndex("desc3");
            int descListIndex = table.TryGetColIndex("descList");
            for (int i = 1, rowHeight = table.rowHeight; i < rowHeight; ++i)
            {
                cs_mapnpc p = new cs_mapnpc();
                p.key = table.TryGetInt(i, keyIndex);
                p.name = table.TryGetInt(i, nameIndex);
                p.desc1 = table.TryGetVector3(i, desc1Index);
                p.desc2 = table.TryGetFloatArray(i, desc2Index);
                p.desc3 = table.TryGetColor(i, desc3Index);
                p.descList = table.TryGetVector2Array(i, descListIndex);
                if (infoDic.ContainsKey(p.key))
                    Debuger.LogError("cs_mapnpc table key repeat, p.key: " + p.key);
                else
                    infoDic.Add(p.key, p);
            }
        }

        public override string ToString()
        { return string.Concat(Table.Parse(key), '\t', Table.Parse(name), '\t', Table.Parse(desc1), '\t', Table.Parse(desc2), '\t', Table.Parse(desc3), '\t', Table.Parse(descList)); }

        public static string Deserialize()
        { return Table.Deserialize(infoDic, "key\tname\tdesc1\tdesc2\tdesc3\tdescList"); }
    }
}
