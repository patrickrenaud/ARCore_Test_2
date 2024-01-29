using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
 
/// Store this code as "MakeFolders.cs" in the Assets\Editor directory (create it, if it does not exist)
/// Creates a number of directories for storage of various content types.
/// Modify as you see fit.
/// Directories are created in the Assets dir.
/// Not tested on a Mac.
 
 
public class MakeFolders : ScriptableObject
{
 
    [MenuItem ("Assets/(ALTKEY) Make Project Default Folders")]
    static void MenuMakeFolders()
    {
		CreateFolders();
	}
 
	static void CreateFolders()
	{
		string f = Application.dataPath + "/";

        Directory.CreateDirectory(f + "Editor");
        Directory.CreateDirectory(f + "Editor Default Resources");
        Directory.CreateDirectory(f + "Extensions");
        Directory.CreateDirectory(f + "Extensions/3rd-Party");
        Directory.CreateDirectory(f + "Plugins");
        Directory.CreateDirectory(f + "Standard Assets");
        Directory.CreateDirectory(f + "StreamingAssets");
        Directory.CreateDirectory(f + "WebPlayerTemplate");
        Directory.CreateDirectory(f + "_ALTKEY");
        Directory.CreateDirectory(f + "_ALTKEY/Animations");
        Directory.CreateDirectory(f + "_ALTKEY/Animators");
        Directory.CreateDirectory(f + "_ALTKEY/Audio");
        Directory.CreateDirectory(f + "_ALTKEY/Audio/Music");
        Directory.CreateDirectory(f + "_ALTKEY/Audio/SFX");
		Directory.CreateDirectory(f + "_ALTKEY/Audio/Mixer");
        Directory.CreateDirectory(f + "_ALTKEY/Fonts");
        Directory.CreateDirectory(f + "_ALTKEY/Gizmos");
        Directory.CreateDirectory(f + "_ALTKEY/Materials");
        Directory.CreateDirectory(f + "_ALTKEY/Models");
        Directory.CreateDirectory(f + "_ALTKEY/Prefabs");
        Directory.CreateDirectory(f + "_ALTKEY/Resources");
        Directory.CreateDirectory(f + "_ALTKEY/Scenes");
        Directory.CreateDirectory(f + "_ALTKEY/Scripts");
        Directory.CreateDirectory(f + "_ALTKEY/Editor");
        Directory.CreateDirectory(f + "_ALTKEY/Shaders");
        Directory.CreateDirectory(f + "_ALTKEY/Textures");

        Debug.Log("Created directories");
	}
}