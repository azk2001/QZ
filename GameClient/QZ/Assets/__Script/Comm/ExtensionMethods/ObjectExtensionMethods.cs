using UnityEngine;
using System.Collections;

namespace MXEngine
{
    public static class ObjectExtensionMethods
    {
        public static int IndexOfInName(this UnityEngine.Object[] objs, string name)
        {
            if(objs == null)
                return -1;

            for(int i = 0, max = objs.Length; i < max; ++i)
            {
                if(string.Equals(objs[i].name, name))
                    return i;
            }
            return -1;
        }

        public static int LastIndexOfInName(this UnityEngine.Object[] objs, string name)
        {
            if(objs == null)
                return -1;

            for(int i = objs.Length - 1; i >= 0; --i)
            {
                if(string.Equals(objs[i].name, name))
                    return i;
            }
            return -1;
        }

        public static UnityEngine.Object FindObjectInName(this UnityEngine.Object[] objs, string name)
        {
            if(objs == null)
                return null;

            for(int i = 0, max = objs.Length; i < max; ++i)
            {
                if(string.Equals(objs[i].name, name))
                    return objs[i];
            }
            return null;
        }

        public static UnityEngine.Object LastFindObjectInName(this UnityEngine.Object[] objs, string name)
        {
            if(objs == null)
                return null;

            for(int i = objs.Length - 1; i >= 0; --i)
            {
                if(string.Equals(objs[i].name, name))
                    return objs[i];
            }
            return null;
        }

//		public static T CreateInstance<T>() where T: new()
//		{
//			System.Type type = typeof(T);
//			if (type.IsSubclassOf(typeof(MonoBehaviour))) 
//			{
//				GameObject gobj = new GameObject(type.ToString(), typeof(T));
//				return gobj.GetComponent<T>();
//			}
//			return new T();
//		}
    }
}