using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static string GetFilePath(string dataFileName)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, "data");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        return Path.Combine(directoryPath, dataFileName.ToLower() + ".txt");
    }


    //Save Data (Asynchronous with backup and error handling)
    public static async Task<bool> SaveDataAsync<T>(T dataToSave, string dataFileName)
    {
        string filePath = GetFilePath(dataFileName);
        string backupFilePath = filePath + ".bak";

        // Convert to Json then to Bytes
        string jsonData = JsonUtility.ToJson(dataToSave, true);

        Debug.Log("Himanshu Save Data to: " + jsonData);

        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        try
        {
            byte[] encryptedData = EncryptionUtility.Encrypt(jsonBytes);

            // Backup the old file before overwriting
            if (File.Exists(filePath))
            {
                File.Copy(filePath, backupFilePath, true);
            }

            await File.WriteAllBytesAsync(filePath, encryptedData);

            Debug.Log("Himanshu Save Data to: " + filePath.Replace("/", "\\"));

            // Remove backup after successful save
            if (File.Exists(backupFilePath))
            {
                File.Delete(backupFilePath);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Himanshu Failed to save data to: " + filePath.Replace("/", "\\"));
            Debug.LogError("Himanshu Error: " + e.Message);

            // Restore from backup if save fails
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, filePath, true);
                File.Delete(backupFilePath);
                Debug.LogWarning("Himanshu Restored data from backup.");
            }

            return false;
        }
    }

    //Load Data (Asynchronous with detailed error handling)
    public static async Task<T> LoadDataAsync<T>(string dataFileName)
    {
        string filePath = GetFilePath(dataFileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Himanshu File does not exist: " + filePath.Replace("/", "\\"));
            return default(T);
        }
        try
        {
            byte[] encryptedData = await File.ReadAllBytesAsync(filePath);
            byte[] decryptedData = EncryptionUtility.Decrypt(encryptedData);

            // Validate file integrity (optional checksum validation can be added here)
            string jsonData = Encoding.UTF8.GetString(decryptedData);
    
            object resultValue = JsonUtility.FromJson<T>(jsonData);

            Debug.Log("Himanshu Loaded Data from: " + filePath.Replace("/", "\\") + resultValue);
            return (T)Convert.ChangeType(resultValue, typeof(T));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Himanshu Failed to load data from: " + filePath.Replace("/", "\\"));
            Debug.LogError("Himanshu Error: " + e.Message);
            return default(T);
        }
    }


    public static async Task<bool> UpdateDataAsync<T>(Func<T, T> updateFunc, string dataFileName)
    {
        string filePath = GetFilePath(dataFileName);
        string backupFilePath = filePath + ".bak";

        try
        {
            // Check if the file exists before updating
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("Himanshu Update Failed: File not found: " + filePath.Replace("/", "\\"));
                return false;
            }

            // Read the existing encrypted data
            byte[] encryptedData = await File.ReadAllBytesAsync(filePath);

            // Decrypt the data
            byte[] jsonBytes = EncryptionUtility.Decrypt(encryptedData);

            // Convert the JSON data back to the object
            string jsonData = Encoding.UTF8.GetString(jsonBytes);
            T existingData = JsonUtility.FromJson<T>(jsonData);

            // Apply the update function
            T updatedData = updateFunc(existingData);

            // Convert updated data back to JSON and then bytes
            string updatedJsonData = JsonUtility.ToJson(updatedData, true);
            byte[] updatedJsonBytes = Encoding.UTF8.GetBytes(updatedJsonData);

            // Backup the old file before overwriting
            if (File.Exists(filePath))
            {
                File.Copy(filePath, backupFilePath, true);
            }

            // Encrypt the updated data
            byte[] updatedEncryptedData = EncryptionUtility.Encrypt(updatedJsonBytes);

            // Write the updated data to the file
            await File.WriteAllBytesAsync(filePath, updatedEncryptedData);

            Debug.Log("Himanshu Updated Data in: " + filePath.Replace("/", "\\"));

            // Remove the backup after a successful update
            if (File.Exists(backupFilePath))
            {
                File.Delete(backupFilePath);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Himanshu Failed to update data in: " + filePath.Replace("/", "\\"));
            Debug.LogError("Himanshu Error: " + e.Message);

            // Restore from backup if the update fails
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, filePath, true);
                File.Delete(backupFilePath);
                Debug.LogWarning("Himanshu Restored data from backup.");
            }

            return false;
        }
    }


    //Delete Data (With custom exceptions)
    public static bool DeleteData(string dataFileName)
    {
        string filePath = GetFilePath(dataFileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Himanshu File does not exist: " + filePath.Replace("/", "\\"));
            return false;
        }

        try
        {
            File.Delete(filePath);
            Debug.Log("Himanshu Data deleted from: " + filePath.Replace("/", "\\"));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Himanshu Failed to delete data: " + e.Message);
            throw new DataDeleteException("Failed to delete data.", e);
        }
    }

    public static bool ValidateFileIntegrity(string dataFileName)
    {
        string filePath = GetFilePath(dataFileName);
        if (!File.Exists(filePath))
        {
            return false;
        }

        try
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            byte[] decryptedData = EncryptionUtility.Decrypt(encryptedData);

            // Perform additional checks here, such as comparing checksums
            return decryptedData != null && decryptedData.Length > 0;
        }
        catch
        {
            return false;
        }
    }
}

[Serializable]
public class PlayerInformation
{
    // public bool privacyPolicy;
    // public int currentDrone;
    public int money;
   // public float highScore = 0;
   // public bool[] dronesUnlocked;
}

// Custom exception for data deletion errors
public class DataDeleteException : Exception
{
    public DataDeleteException() { }
    public DataDeleteException(string message) : base(message) { }
    public DataDeleteException(string message, Exception inner) : base(message, inner) { }
}
