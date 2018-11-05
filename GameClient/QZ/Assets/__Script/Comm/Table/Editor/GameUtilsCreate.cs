using UnityEngine;
using System.Collections;
using UnityEditor;

namespace MXEngine
{
    public static class GameUtilsCreate
    {
        //配置表生成-------------------------------------------------------------------------------------------
        [MenuItem("GameUtils/Create/tabledic.cs")]
        [MenuItem("Assets/GameUtils/Create/tabledic.cs")]
        static void TableToDicCS()
        {
            if (MXEngine.BuilderTableEditor.TableToCS(true, true, false))
                Debug.Log("配置表类创建完毕");
        }
//        [MenuItem("GameUtils/Create/tabledic.cs", true)]
//        [MenuItem("Assets/GameUtils/Create/tabledic.cs", true)]
//        static public bool TableToDicCSBool()
//        {
//            return GameUtils.CfgEditor.BuilderTableEditor.TableToCSBool();
//        }
        [MenuItem("GameUtils/Create/tablelist.cs")]
        [MenuItem("Assets/GameUtils/Create/tablelist.cs")]
        static void TableToListCS()
        {
            if (MXEngine.BuilderTableEditor.TableToCS(true, false, false))
                Debug.Log("配置表类创建完毕");
        }

		//配置表生成-------------------------------------------------------------------------------------------
		[MenuItem("GameUtils/Create/tabledic(正式).cs")]
		[MenuItem("Assets/GameUtils/Create/tabledic(正式).cs")]
		static void TableToDicCS2()
		{
			if (MXEngine.BuilderTableEditor.TableToCS(false, true, false))
				Debug.Log("配置表类创建完毕");
		}
		//        [MenuItem("GameUtils/Create/tabledic.cs", true)]
		//        [MenuItem("Assets/GameUtils/Create/tabledic.cs", true)]
		//        static public bool TableToDicCSBool()
		//        {
		//            return GameUtils.CfgEditor.BuilderTableEditor.TableToCSBool();
		//        }
		[MenuItem("GameUtils/Create/tablelist(正式).cs")]
		[MenuItem("Assets/GameUtils/Create/tablelist(正式).cs")]
		static void TableToListCS2()
		{
			if (MXEngine.BuilderTableEditor.TableToCS(false, false, false))
				Debug.Log("配置表类创建完毕");
		}

		//配置表生成-------------------------------------------------------------------------------------------
		[MenuItem("GameUtils/Create/tabledic(编辑).cs")]
		[MenuItem("Assets/GameUtils/Create/tabledic(编辑).cs")]
		static void TableToDicCSEditor()
		{
			if (MXEngine.BuilderTableEditor.TableToCS(false, true, true))
				Debug.Log("配置表类创建完毕");
		}
		//        [MenuItem("GameUtils/Create/tabledic.cs", true)]
		//        [MenuItem("Assets/GameUtils/Create/tabledic.cs", true)]
		//        static public bool TableToDicCSBool()
		//        {
		//            return GameUtils.CfgEditor.BuilderTableEditor.TableToCSBool();
		//        }
		[MenuItem("GameUtils/Create/tablelist(编辑).cs")]
		[MenuItem("Assets/GameUtils/Create/tablelist(编辑).cs")]
		static void TableToListCSEditor()
		{
			if (MXEngine.BuilderTableEditor.TableToCS(false, false, true))
				Debug.Log("配置表类创建完毕");
		}

		//配置表生成-------------------------------------------------------------------------------------------
        [MenuItem("GameUtils/Create/tabledic.cs", true)]
        [MenuItem("Assets/GameUtils/Create/tabledic.cs", true)]
        [MenuItem("GameUtils/Create/tablelist.cs", true)]
        [MenuItem("Assets/GameUtils/Create/tablelist.cs", true)]
		[MenuItem("GameUtils/Create/tabledic(正式).cs", true)]
		[MenuItem("Assets/GameUtils/Create/tablelist(正式).cs", true)]
		[MenuItem("GameUtils/Create/tabledic(编辑).cs", true)]
		[MenuItem("Assets/GameUtils/Create/tablelist(编辑).cs", true)]
        static public bool OnCheckTxt()
        {
            return GameUtilsEditor.CheckPathNameExtension(".txt", Selection.objects);
        }
    }
}


