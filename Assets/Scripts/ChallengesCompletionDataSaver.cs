using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//todo: make static saving helper class, not core logic, that supposed to be  in game mode class
//todo: save only in one file, make persistent challenge state class
//todo: maybe save only bool[] completion of challenges state
public class ChallengesCompletionDataSaver: MonoBehaviour
{
   [Header("Directory & File Names")]
   [SerializeField] private string saveDirectoryName = "saves";
   [SerializeField] private string saveFileName = "monthsChallenges";
   private string savePath;

   [Header("Objects To Persist")]
   [SerializeField] private MonthChallengeSet[] monthChallengeSets;
   
   private void SaveGame()
   {
      for (int i = 0; i < monthChallengeSets.Length; i++)
      {
        SaveToFile(saveFileName+i,monthChallengeSets[i]);
      }
   }

   private void SaveToFile(string filename, ScriptableObject objectToPersist)
   {
      FileStream fileStream = File.Create(Path.Combine(savePath,filename));
      BinaryFormatter binaryFormatter = new BinaryFormatter();

      var json = JsonUtility.ToJson(objectToPersist);
      binaryFormatter.Serialize(fileStream,json);
      fileStream.Close();
   }

   private void LoadGame()
   {
      for (int i = 0; i < monthChallengeSets.Length; i++)
      {
         print(LoadFile(saveFileName+i, monthChallengeSets[i]) ? "Load successful!" : "Load failed!");
      }
   }

   private bool LoadFile(string filename, ScriptableObject objectToLoad)
   {
      string filePath = Path.Combine(savePath, filename);
      if (File.Exists(filePath))
      {
         var fileStream = File.Open(filePath, FileMode.Open);
         BinaryFormatter binaryFormatter = new BinaryFormatter();

         JsonUtility.FromJsonOverwrite((string) binaryFormatter.Deserialize(fileStream), objectToLoad);
         fileStream.Close();

         return true;
      }

      return false;
   }

   public void ClearGameSaves()
   {
      for (int i = 0; i < monthChallengeSets.Length; i++)
         ClearFile(saveFileName+i);
   }
   
   private void ClearFile(string filename)
   {
      if(savePath == null)
         savePath = Path.Combine(Application.persistentDataPath, saveDirectoryName);
      string filePath = Path.Combine(savePath, filename);
      if (File.Exists(filePath))
         File.Delete(filePath);
   }

   private void Awake()
   {
      savePath = Path.Combine(Application.persistentDataPath, saveDirectoryName);
      if (!Directory.Exists(savePath))
         Directory.CreateDirectory(savePath);
      
      LoadGame();
   }

   private void OnApplicationPause(bool pauseStatus)
   {
      if(pauseStatus)
         SaveGame();
   }

   private void OnApplicationQuit()
   {
      SaveGame();
   }
}
