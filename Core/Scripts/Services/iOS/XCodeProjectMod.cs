//Developed by Pavel Kravtsov.
#if UNITY_EDITOR && UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections;

namespace Core
{
    public class XCodeProjectMod : MonoBehaviour
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                string projPath = PBXProject.GetPBXProjectPath(path);
                PBXProject project = new PBXProject();
                project.ReadFromString(File.ReadAllText(projPath));
                string target = project.TargetGuidByName("Unity-iPhone");
                project.UpdateBuildProperty(target, "OTHER_CFLAGS", new string[] { "-fdeclspec" }, new string[] { });

                project.AddFrameworkToProject(target, "StoreKit.framework", false);
                project.AddFrameworkToProject(target, "GameKit.framework", false);
                File.WriteAllText(projPath, project.WriteToString());
            }
        }
    }
}
#endif