using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Core
{
    public class ParseLocalizationcCSV
    {

        [MenuItem("Urmobi/Parse localization CSV")]
        private static void NewMenuOption()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                EditorUtility.DisplayDialog("Warning", "Pls, load Start scene", "Ok");
            }

            TextAsset csvFile = Resources.Load("Localization/final") as TextAsset;
            var result = CSVReader.SplitCsvGrid(csvFile.text);
            var langCount = result.GetUpperBound(0);

            for (int i = 1; i <= result.GetUpperBound(0); i++)
            {
                string fileName = Application.dataPath + "/Resources/Localization/" + result[i, 0] + ".csv";

                var sr = File.CreateText(fileName);

                for (int k = 0; k < result.GetUpperBound(1); k++)
                {
                    sr.WriteLine(result[0, k] + "," +
                        (result[i, k]!= null && result[i, k] != "" && result[i, k].Contains(",") ? "\"" + result[i, k] + "\"" : result[i, k]));
                }
                sr.Close();
            }
        }
    }
}