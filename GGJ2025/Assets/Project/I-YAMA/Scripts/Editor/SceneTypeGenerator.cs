using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace GGJ.Common
{
    public class SceneTypeGenerator : MonoBehaviour
    {
        /// <summary> ESceneType生成 </summary>
        [InitializeOnLoadMethod]
        static void SceneTypeGenerate()
        {
            EditorBuildSettings.sceneListChanged += () =>
            {
                // コード生成
                var writeCodes = new List<string>();
                writeCodes.Add("// SceneTypeGenerator.csで生成\n");
                writeCodes.Add("namespace GGJ.Common");
                writeCodes.Add("{");
                writeCodes.Add("\t/// <summary> シーンタイプ </summary>");
                writeCodes.Add("\t[System.Serializable]");
                writeCodes.Add("\tpublic enum SceneType");
                writeCodes.Add("\t{");
                writeCodes.Add("\t\tNone = -1,");

                // シーン一覧からシーン名と状態を取得
                writeCodes.AddRange(from scene in EditorBuildSettings.scenes
                    select scene.path
                    into sceneName
                    select sceneName.Remove(0, sceneName.LastIndexOf("/", StringComparison.Ordinal) + 1)
                        .Replace(".unity", "")
                        .Replace(" ", "_")
                    into sceneName
                    select "\t\t" + sceneName + ",");

                writeCodes.Add("\t\tMax,");
                writeCodes.Add("\t}");
                writeCodes.Add("}");


                // 生成
                var filePath = Application.dataPath + "/Project/Scripts/SceneManager/SceneType.cs";
                File.Delete(filePath);
                var stream = new FileStream(filePath, FileMode.OpenOrCreate);
                var sw = new StreamWriter(stream, Encoding.UTF8);
                sw.Write("");
                foreach (var code in writeCodes)
                {
                    sw.WriteLine(code);
                }

                sw.Close();

                AssetDatabase.Refresh();
                Debug.Log("シーンリスト更新終了");
            };
        }

    }
}