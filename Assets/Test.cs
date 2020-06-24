using UnityEngine;

public class Test : MonoBehaviour
{
	//Your Sprite
	[SerializeField] SpriteRenderer imageSprite;
	//drawing color
	[SerializeField] Color drawColor;

	//A copy of your sprite's texture
	Texture2D imageTextureCopy;

	void Start ()
	{
		//Check if already saved a texture in disk with key "myTexture"
		if (TextureUtilities.IsTextureSaved ("myTexture")) {
			//Load saved texture
			imageTextureCopy = TextureUtilities.Load ("myTexture");

			//Assign loaded texture to your sprite
			imageSprite.sprite = TextureUtilities.GetSpriteFromTexture (imageTextureCopy);
		}

		//Get texture copy in order to manipulate its data and pixels.
		//you can't access texture data directly, it will throw an error : (Texture is not readable)
		imageTextureCopy = TextureUtilities.GetTextureCopy (imageSprite.sprite.texture);
	}

	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {
			//If mouse click : Draw a random horizontal line with color assigned in the inspector

			//Draw code
			int randomY = Random.Range (3, imageTextureCopy.height - 4);
			int y = randomY - 3;
			for (int i = 0; i < 6; i++) {
				for (int j = 0; j < imageTextureCopy.width; j++) {
					imageTextureCopy.SetPixel (j, y, drawColor);
				}
				y++;
			}

			//Apply texture data
			imageTextureCopy.Apply ();

			//Assign this imageTextureCopy to your sprite
			imageSprite.sprite = TextureUtilities.GetSpriteFromTexture (imageTextureCopy);
		}
	}

	void OnApplicationQuit ()
	{
		//Save texture whenever you quit the game.
		//Or hit Stop in unity Editor

		TextureUtilities.Save (imageTextureCopy, "myTexture");
	}
}
