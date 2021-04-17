using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 
/// </summary>
static class Helper{

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    static public string CombineExePath(string path) {
        return Path.Combine(AssemblyExecuteableDirectory(), path);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    static public string AssemblyExecuteableDirectory() {
        string codeBase = Assembly.GetEntryAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="values"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    static public string FirstOrDefault(IEnumerable<string> values, string value) {

        if (values != null) {
            var it = values.GetEnumerator();
            if (it.MoveNext()) {
                return it.Current;
            }
        }

        return value;
    }

    /// <summary>
    /// 
    /// </summary>
    static public void RegisterAtWindowsStartup() {

        if (IsRegisteredAtWindowsStartup()) {
            return;
        }

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
            key.SetValue(Application.ProductName, "\"" + Application.ExecutablePath + "\"");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    static public void UnregisterAtWindowsStartup() {

        if (!IsRegisteredAtWindowsStartup()) {
            return;
        }

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
            key.DeleteValue(Application.ProductName, false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    static public bool IsRegisteredAtWindowsStartup() {
        bool registered = false;
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
            registered = key.GetValueNames().Contains(Application.ProductName);
        }

        return registered;
    }

    /// <summary>
    /// Returns <c>true</c> if the supplied collection <paramref name="readerNames"/> does not contain any reader.
    /// </summary>
    /// <param name="readerNames">Collection of smart-card reader names</param>
    /// <returns><c>true</c> if no reader found</returns>
    public static bool IsEmpty(ICollection<string> readerNames) {
        return readerNames == null || readerNames.Count < 1;
    }

    /// <summary>
    /// Returns <c>true</c> if the supplied collection <paramref name="readerNames"/> does not contain any reader.
    /// </summary>
    /// <param name="readerNames">Collection of smart-card reader names</param>
    /// <returns><c>true</c> if no reader found</returns>
    public static bool IsEmpty(IEnumerable<string> readerNames) {
        return readerNames == null || readerNames.Count() < 1;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
	public static IEnumerable<string> SplitByLength(this string str, int maxLength){
		int index = 0;
		while (true){
			if (index + maxLength >= str.Length)
			{
				yield return str.Substring(index);
				yield break;
			}
			yield return str.Substring(index, maxLength);
			index += maxLength;
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
	public static Byte[] Decode(this Byte[] bytes){
		if (bytes.Length == 0) return bytes;
		var i = bytes.Length - 1;
		while (bytes[i] == 0 || bytes[i] == 144){
			i--;
		}
		Byte[] copy = new Byte[i + 1];
		Array.Copy(bytes, copy, i + 1);
		return copy;
	}
	
    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
	public static string Base64Encode(string plainText){
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
		return System.Convert.ToBase64String(plainTextBytes);
	}
	
    /// <summary>
    /// 
    /// </summary>
    /// <param name="base64EncodedData"></param>
    /// <returns></returns>
	public static string Base64Decode(string base64EncodedData){
		var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
		return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
	}
}

