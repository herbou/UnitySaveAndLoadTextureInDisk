using UnityEngine;
using System.IO;

public class TextureUtilities : MonoBehaviour
{
	static string savePath = Application.persistentDataPath + "/SavedTextures/";

	public static void Save (Texture2D texture, string key)
	{
		if (!Directory.Exists (savePath))
			Directory.CreateDirectory (savePath);
		
		byte[] bytes = texture.EncodeToPNG ();

		FileStream file = File.Create (GetFilePath (key));
		file.Write (bytes, 0, bytes.Length);

		file.Close ();
	}

	public static Texture2D Load (string key)
	{
		byte[] fileData = File.ReadAllBytes (GetFilePath (key));
	
		Texture2D texture = new Texture2D (2, 2);

		//..this will auto-resize the texture dimensions.
		texture.LoadImage (fileData); 

		return texture;
	}

	public static bool IsTextureSaved (string key)
	{
		return File.Exists (GetFilePath (key));
	}

	public static Sprite GetSpriteFromTexture (Texture2D texture)
	{
		return Sprite.Create (
			texture, 
			new Rect (0, 0, texture.width, texture.height), 
			new Vector2 (0.5f, 0.5f)
		);
	}


	public static Texture2D GetTextureCopy (Texture2D source)
	{
		//Create a RenderTexture
		RenderTexture rt = RenderTexture.GetTemporary (
			                   source.width,
			                   source.height,
			                   0,
			                   RenderTextureFormat.Default,
			                   RenderTextureReadWrite.Linear
		                   );

		//Copy source texture to the new render (RenderTexture) 
		Graphics.Blit (source, rt);

		//Store the active RenderTexture & activate new created one (rt)
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = rt;

		//Create new Texture2D and fill its pixels from rt and apply changes.
		Texture2D readableTexture = new Texture2D (source.width, source.height);
		readableTexture.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
		readableTexture.Apply ();

		//activate the (previous) RenderTexture and release texture created with (GetTemporary( ) ..)
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary (rt);

		return readableTexture;
	}

	static string GetFilePath (string filename)
	{
		return savePath + filename;
	}
}
