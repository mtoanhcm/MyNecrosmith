using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using Newtonsoft.Json;

namespace SaveLoad
{
    /// <summary>
    /// Enum defining types of files that can be saved
    /// </summary>
    public enum FileType
    {
        BuildingPosition
    }

    /// <summary>
    /// Static class to manage game data saving and loading
    /// </summary>
    public static class SaveLoadManager
    {
        // Encryption key - in a real environment, you should store this key more securely
        private static readonly string EncryptionKey = "YourStrongEncryptionKey123!@#";
        private static readonly string SaveDirectory = "SaveData";

        /// <summary>
        /// Save object data to file with encryption
        /// </summary>
        /// <typeparam name="T">Type of object to save. Must be marked with [Serializable] attribute and contain only serializable fields.</typeparam>
        /// <param name="fileType">File type from FileType enum</param>
        /// <param name="data">Data object to save</param>
        /// <returns>True if saved successfully, False if failed</returns>
        /// <remarks>
        /// The type T must:
        /// - Be marked with the [Serializable] attribute
        /// - Contain only fields that NewtonJson can serialize
        /// - Public fields will be serialized by default
        /// - Private fields need [SerializeField] attribute to be included
        /// - Dictionaries and certain complex types are not supported by NewtonJson
        /// </remarks>
        public static bool Save<T>(FileType fileType, T data)
        {
            try
            {
                // Verify the type is serializable
                if (!typeof(T).IsSerializable && typeof(T) != typeof(string))
                {
                    Debug.LogError($"Type {typeof(T).Name} is not marked as [Serializable]. Data cannot be saved.");
                    return false;
                }

                // Convert data to JSON string
                string jsonData = JsonConvert.SerializeObject(data);

                // Check if serialization produced empty result for non-primitive type
                if (jsonData == "{}" && !IsPrimitiveOrString(typeof(T)))
                {
                    Debug.LogWarning($"Serialization of {typeof(T).Name} resulted in empty JSON. Check if the type can be properly serialized by NewtonJson.");
                }

                // Create filename from enum
                string fileName = fileType.ToString();

                // Encrypt the content
                string encryptedContent = EncryptData(jsonData);

                // Ensure directory exists
                string directoryPath = Path.Combine(Application.persistentDataPath, SaveDirectory);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create full path to the file
                string filePath = Path.Combine(directoryPath, fileName);

                // Write encrypted content to file
                File.WriteAllText(filePath, encryptedContent);

                Debug.Log($"Saved data to {fileName} successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Load and deserialize object from file
        /// </summary>
        /// <typeparam name="T">Type of object to load. Must be marked with [Serializable] attribute and have a parameterless constructor.</typeparam>
        /// <param name="fileType">File type from FileType enum</param>
        /// <returns>Loaded object or default if error</returns>
        /// <remarks>
        /// The type T must:
        /// - Be marked with the [Serializable] attribute
        /// - Have a parameterless constructor
        /// - Contain only fields that NewtonJson can deserialize
        /// </remarks>
        public static T Load<T>(FileType fileType) where T : new()
        {
            try
            {
                // Verify the type is serializable
                if (!typeof(T).IsSerializable && typeof(T) != typeof(string))
                {
                    Debug.LogError($"Type {typeof(T).Name} is not marked as [Serializable]. Data cannot be loaded properly.");
                    return new T();
                }

                // Create filename from enum
                string fileName = fileType.ToString();

                // Create full path to the file
                string filePath = Path.Combine(Application.persistentDataPath, SaveDirectory, fileName);

                // Check if file exists
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"Save file {fileName} does not exist.");
                    return new T();
                }

                // Read encrypted content from file
                string encryptedContent = File.ReadAllText(filePath);

                // Decrypt the content
                string jsonData = DecryptData(encryptedContent);

                // Deserialize JSON to object
                T loadedData = JsonConvert.DeserializeObject<T>(jsonData);

                Debug.Log($"Loaded data from {fileName} successfully.");
                return loadedData;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading data: {ex.Message}");
                return new T();
            }
        }

        /// <summary>
        /// Save raw string content to file with encryption
        /// </summary>
        /// <param name="fileType">File type from FileType enum</param>
        /// <param name="content">String content to save</param>
        /// <returns>True if saved successfully, False if failed</returns>
        public static bool SaveRaw(FileType fileType, string content)
        {
            try
            {
                // Create filename from enum
                string fileName = fileType.ToString();

                // Encrypt the content
                string encryptedContent = EncryptData(content);

                // Ensure directory exists
                string directoryPath = Path.Combine(Application.persistentDataPath, SaveDirectory);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create full path to the file
                string filePath = Path.Combine(directoryPath, fileName);

                // Write encrypted content to file
                File.WriteAllText(filePath, encryptedContent);

                Debug.Log($"Saved raw data to {fileName} successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving raw data: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Load raw string content from file
        /// </summary>
        /// <param name="fileType">File type from FileType enum</param>
        /// <returns>Decrypted content or null if error</returns>
        public static string LoadRaw(FileType fileType)
        {
            try
            {
                // Create filename from enum
                string fileName = fileType.ToString();

                // Create full path to the file
                string filePath = Path.Combine(Application.persistentDataPath, SaveDirectory, fileName);

                // Check if file exists
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"Save file {fileName} does not exist.");
                    return null;
                }

                // Read encrypted content from file
                string encryptedContent = File.ReadAllText(filePath);

                // Decrypt the content
                string decryptedContent = DecryptData(encryptedContent);

                Debug.Log($"Loaded raw data from {fileName} successfully.");
                return decryptedContent;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading raw data: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="fileType">File type to check</param>
        /// <returns>True if file exists, False if not</returns>
        public static bool FileExists(FileType fileType)
        {
            string filePath = Path.Combine(Application.persistentDataPath, SaveDirectory, fileType.ToString());
            return File.Exists(filePath);
        }

        /// <summary>
        /// Delete save file
        /// </summary>
        /// <param name="fileType">File type to delete</param>
        /// <returns>True if deleted successfully, False if failed</returns>
        public static bool DeleteSaveFile(FileType fileType)
        {
            try
            {
                string filePath = Path.Combine(Application.persistentDataPath, SaveDirectory, fileType.ToString());
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log($"Deleted save file {fileType} successfully.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error deleting save file: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if a type is a primitive or string
        /// </summary>
        private static bool IsPrimitiveOrString(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        /// <summary>
        /// Encrypt data using AES
        /// </summary>
        private static string EncryptData(string data)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(data);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypt data encrypted with AES
        /// </summary>
        private static string DecryptData(string encryptedData)
        {
            encryptedData = encryptedData.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(encryptedData);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }
    }
}
