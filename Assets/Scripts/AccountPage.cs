using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

public class AccountPage : MonoBehaviour
{
	public GameObject StoreEmpty;
	public Text userpagetitle;
	public Texture2D texture;
	public Image chosenImageDisplay;
	public GameObject paintingDisplay;
	public GameObject storescrollParent;

	public GameObject paintingOrderDisplay;
	public GameObject orderscrollParent;
	public int targetWidth, targetHeight;
	List<GameObject> paintingList = new List<GameObject>();
	List<GameObject> paintingOrderList = new List<GameObject>();
	public Queries queries;
	public UserInfo userinfo;
	public Texture2D PickedImage;
	public Image croppedImageHolder;
	public float minAspectRatio, maxAspectRatio;
	public string base64Encoded;
	public string serverUrl = "https://onlinepaintingstore.000webhostapp.com/PaintingPictures";
	public InputField titleInput, descriptionInput, priceInput, quantityInput, sizeWInput, sizeHInput;
	public TMP_InputField creationDateInput;
	public Dropdown styleDropDown;
	public string title, description, creationDate, price, quantity, sizeW, sizeH, style, size, artistfName, artistlName;

	string username;
	string oldpassword;
	string newpassword;
	string email;
	string firstname;
	string lastname;
	string birthdate;
	public InputField usernameInput;
	public InputField oldpasswordInput;
	public InputField newpasswordInput;
	public InputField emailInput;
	public InputField firstnameInput;
	public InputField lastnameInput;
	public TMP_InputField birthdateInput;
	List<string> usernamelist = new List<string>();
	bool emptyfieldserror, emailformaterror, usernameexistserror, passwordformaterror, usernameformaterror, passwordoldmatcherror;
	public GameObject emptyfieldsError, emailformatError, usernameexistsError, passwordformatError, usernameformatError, passwordoldmatchError;
	private void Update()
	{
		if (chosenImageDisplay.sprite == null)
		{
			Color temp = chosenImageDisplay.color;
			temp.a = 0;
			chosenImageDisplay.color = temp;
		}
		if (chosenImageDisplay.sprite != null)
		{
			Color temp = chosenImageDisplay.color;
			temp.a = 1;
			chosenImageDisplay.color = temp;
		}


		
		if (usernameInput.text == "" || firstnameInput.text == "" || lastnameInput.text == "" || emailInput.text == "" || birthdateInput.text == "")
		{
			emptyfieldserror = true;
		}
		else
		{
			emptyfieldserror = false;
		}

	}

    private void Start()
    {
	
	}


	public void OpenOrders()
    {
		foreach(GameObject painting in paintingOrderList)
        {
			Destroy(painting);
        }
		paintingOrderList.Clear();
		GetOrders(queries.WriteQuery_GETORDERS(userinfo.userID));
    }



	public static bool IsValidPassword(string password)
	{

		// Check if password is between 8 and 15 characters long and has no spaces
		if (password.Length < 8 || password.Length > 15 || password.Contains(" "))
			return false;

		// Check if password contains at least one capital letter, one normal letter, one number, and one special character
		string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,15}$";
		Regex regex = new Regex(pattern);

		return regex.IsMatch(password);
	}

	public static bool IsValidUsername(string username)
	{

		// Check if username is between 8 and 15 characters long and has no spaces
		if (username.Length < 8 || username.Length > 15 || username.Contains(" "))
			return false;
		else
			return true;
	}
	public static string HashPassword(string password)
	{
		using (SHA256 sha256 = SHA256.Create())
		{
			byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < hashedBytes.Length; i++)
			{
				builder.Append(hashedBytes[i].ToString("x2"));
			}

			return builder.ToString();
		}
	}
	public void ShowOldUserInfo()
    {
		usernameInput.text = userinfo.username;
		emailInput.text = userinfo.email;
		firstnameInput.text = userinfo.fName;
		lastnameInput.text = userinfo.lName;
		birthdateInput.text = userinfo.birthDate;
		oldpasswordInput.text = "";
		newpasswordInput.text = "";

		ResetErrors();



	}
	void ResetErrors()
    {
		emptyfieldsError.SetActive(false);
		emailformatError.SetActive(false);
		usernameexistsError.SetActive(false);
		passwordformatError.SetActive(false);
		usernameformatError.SetActive(false);
		passwordoldmatchError.SetActive(false);

		emptyfieldserror = false;
		emailformaterror = false;
		usernameexistserror = false;
		passwordformaterror = false;
		usernameformaterror = false;
		passwordoldmatcherror = false;
	}
	public void EditUserInfo()
	{
		emptyfieldsError.SetActive(false);
		emailformatError.SetActive(false);
		usernameexistsError.SetActive(false);
		passwordformatError.SetActive(false);
		usernameformatError.SetActive(false);
		passwordoldmatchError.SetActive(false);

		emptyfieldserror = false;
		emailformaterror = false;
		usernameexistserror = false;
		passwordformaterror = false;
		usernameformaterror = false;
		passwordoldmatcherror = false;

		username = usernameInput.text;
		email = emailInput.text;
		firstname = firstnameInput.text;
		lastname = lastnameInput.text;
		birthdate = birthdateInput.text;
		oldpassword = oldpasswordInput.text;
		newpassword = newpasswordInput.text;

		if (IsValidEmail(email) == false)
		{
			emailformaterror = true;
		}

		if (IsValidUsername(username) == false)
		{
			usernameformaterror = true;
		}
		if (oldpasswordInput.text != "" || newpasswordInput.text != "")
        {
			if (HashPassword(oldpassword) != userinfo.password)
			{
				passwordoldmatcherror = true;
			}
			if (IsValidPassword(newpassword) == false)
			{
				passwordformaterror = true;
			}
		}
		if (oldpasswordInput.text == "" && newpasswordInput.text == "")
        {
			oldpassword = userinfo.password;
			newpassword = oldpassword;
        }


			CheckUsernames(queries.WriteQuery_GETALLUSERNAMES());
	


		
    }
	public void GetOrders(string query)
	{
		Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
		{

			DisplayOrders(queries.ExtractInfo(result));
		}));
	}
	public void GetUserInfo(string query)
	{
		Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
		{
			//Change so we get all userinfo we might need and put it in userinfo.
			userinfo.userID = queries.ExtractInfo(result)[0][0];
			userinfo.username = queries.ExtractInfo(result)[0][1];
			userinfo.fName = queries.ExtractInfo(result)[0][2];
			userinfo.lName = queries.ExtractInfo(result)[0][3];
			userinfo.birthDate = queries.ExtractInfo(result)[0][4];
			userinfo.email = queries.ExtractInfo(result)[0][5];
			userinfo.password = queries.ExtractInfo(result)[0][6];

		}));
	}
	public void EditUserResult()
    {
		if (emptyfieldserror == false && emailformaterror == false && usernameexistserror == false && passwordformaterror == false && usernameformaterror == false && passwordoldmatcherror == false)
        {
			EditUserQueryResult(queries.WriteQuery_UPDATEUSER(username, firstname, lastname, email, birthdate, HashPassword(newpassword), userinfo.userID));


		}
		if (emptyfieldserror)
		{ 
			emptyfieldsError.SetActive(true);		
		}
		if (usernameexistserror)
		{
			usernameexistsError.SetActive(true);
		}
		if (usernameformaterror)
		{
			usernameformatError.SetActive(true);
		}
		if (emailformaterror)
		{
			emailformatError.SetActive(true);
		}
		if (passwordformaterror)
        {
			passwordformatError.SetActive(true);
		}
		if (passwordoldmatcherror)
		{
			passwordoldmatchError.SetActive(true);
		}

	}
		

	
	public void CheckUsernames(string query)
	{
		Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
		{
			CompareUsernames(queries.ExtractInfo(result));
			EditUserResult();

		}));
	}
	void CompareUsernames(List<List<string>> usernameList)
	{

		for (int i = 0; i < usernameList.Count; i++)
		{
			usernamelist.Add(usernameList[i][0].Trim());
		}
		if (usernamelist.Contains(username) && username != userinfo.username)
		{
			usernameexistserror = true;
		}

	}
	public static bool IsValidEmail(string email)
	{


		// Regular expression pattern to validate email address format
		string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
		Regex regex = new Regex(pattern);

		return regex.IsMatch(email);
	}
	public void ResetAddPaintingInfo()
    {
		titleInput.text = "";
		descriptionInput.text = "";
		creationDateInput.text = "";
		priceInput.text = "";
		quantityInput.text = "";
		sizeWInput.text = "";
		sizeHInput.text = "";
		size = "";
		style = "";
		artistfName = "";
		artistlName = "";

		title = "";
		description = "";
		creationDate = "";
		price = "";
		quantity = "";
		sizeW = "";
		sizeH = "";
		base64Encoded = "";
		chosenImageDisplay.sprite = null;
	}
    public void ConfirmPaintingInfo()
	{
		title = titleInput.text;
		description = descriptionInput.text;
		creationDate = creationDateInput.text;
		price = priceInput.text;
		quantity = quantityInput.text;
		sizeW = sizeWInput.text;
		sizeH = sizeHInput.text;
		size = sizeW + "x" + sizeH;
		style = styleDropDown.options[styleDropDown.value].text;
		artistfName = userinfo.fName;
		artistlName = userinfo.lName;
		CallGetQueryResult(queries.WriteQuery_ADDPAINTING(userinfo.userID, title, description, style, price, quantity, artistfName, artistlName, size, base64Encoded, creationDate, "0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
		LoadStore();
	}

	public void CallGetQueryResult(string query)
	{
		Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
		{
			
		}));
	}
	public void EditUserQueryResult(string query)
	{
		Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
		{
			GetUserInfo(queries.WriteQuery_GETUSERINFO(username));
			ShowOldUserInfo();
		}));
	}

	public void PickImageFromGallery(int maxSize = 1024)
	{
		NativeGallery.GetImageFromGallery((path) =>
		{
			if (path != null)
			{
				PickedImage = NativeGallery.LoadImageAtPath(path, maxSize);
				//sprite = Sprite.Create(pickedImage, new Rect(0, 0, pickedImage.width, pickedImage.height), new Vector2(0.5f, 0.5f));
				//image.sprite = sprite;

				ShowCropper(PickedImage);

			}
		});
	}

	public void ShowCropper(Texture2D pickedImage)
	{
		pickedImage = PickedImage;
		
		ImageCropper.Instance.Show(pickedImage, (bool result, Texture originalImage, Texture2D croppedImage) =>
		{
			
			if (result)
			{
				// Assign cropped texture to the RawImage
				//croppedImageHolder.enabled = true;

				texture = new Texture2D(croppedImage.width, croppedImage.height, croppedImage.format, false);
			
				// Copy the pixel data from the source texture to the new texture
				byte[] sourcePixels = croppedImage.GetRawTextureData();
				texture.LoadRawTextureData(sourcePixels);

				// Apply the changes to the destination texture
				texture.Apply();
					
	
				Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, texture.format, false);
				
				for (int y = 0; y < targetHeight; y++)
				{
					for (int x = 0; x < targetWidth; x++)
					{
						Color color = texture.GetPixelBilinear((float)x / targetWidth, (float)y / targetHeight);
						resizedTexture.SetPixel(x, y, color);
					}
				}

				resizedTexture.Apply();
				byte[] textureData = resizedTexture.EncodeToPNG();

				// Convert the PNG byte array to a base64 string
				string base64Texture = CustomBase64Encode(textureData);
				base64Encoded = base64Texture;


				byte[] bytes = CustomBase64Decode(base64Encoded);
				Texture2D texturenew = new Texture2D(1, 1);
				texturenew.LoadImage(bytes);
				Sprite sprite = Sprite.Create(texturenew, new Rect(0, 0, texturenew.width, texturenew.height), Vector2.zero);


				//Sprite sprite = Sprite.Create(pickedImage, new Rect(0, 0, pickedImage.width, pickedImage.height), Vector2.zero);
				chosenImageDisplay.sprite = sprite;
				//chosenImageDisplay.sprite = sprite;



			}
			else
			{
				croppedImageHolder.enabled = false;

			}

			// Destroy the screenshot as we no longer need it in this case
			//Destroy(screenshot);
		},
			 settings: new ImageCropper.Settings()
			 {
				 ovalSelection = false,
				 autoZoomEnabled = false,
				 imageBackground = Color.clear, // transparent background
				 selectionMinAspectRatio = minAspectRatio,
				 selectionMaxAspectRatio = maxAspectRatio

			 },
			 croppedImageResizePolicy: (ref int width, ref int height) =>
			 {
				 // uncomment lines below to save cropped image at half resolution
				 //width = 540;
				 //height = 960;
			 });
	}

	public static byte[] CustomBase64Decode(string base64String)
	{
		const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		if (base64String == null || base64String.Length % 4 != 0)
			throw new ArgumentException("Invalid Base64 string");

		int padding = 0;
		if (base64String.EndsWith("=="))
			padding = 2;
		else if (base64String.EndsWith("="))
			padding = 1;

		int length = base64String.Length / 4 * 3 - padding;
		byte[] data = new byte[length];

		int i = 0;
		int j = 0;
		while (i < base64String.Length)
		{
			int b1 = base64Chars.IndexOf(base64String[i++]);
			int b2 = base64Chars.IndexOf(base64String[i++]);
			int b3 = base64Chars.IndexOf(base64String[i++]);
			int b4 = base64Chars.IndexOf(base64String[i++]);

			int c1 = (b1 << 2) | (b2 >> 4);
			int c2 = ((b2 & 0x0F) << 4) | (b3 >> 2);
			int c3 = ((b3 & 0x03) << 6) | b4;

			data[j++] = (byte)c1;
			if (j < length) data[j++] = (byte)c2;
			if (j < length) data[j++] = (byte)c3;
		}

		return data;
	}
	public static string CustomBase64Encode(byte[] data)
	{
		const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

		StringBuilder result = new StringBuilder();
		int i = 0;
		while (i < data.Length)
		{
			byte b1 = data[i++];
			byte b2 = i < data.Length ? data[i++] : (byte)0;
			byte b3 = i < data.Length ? data[i++] : (byte)0;

			byte c1 = (byte)(b1 >> 2);
			byte c2 = (byte)(((b1 & 0x03) << 4) | (b2 >> 4));
			byte c3 = (byte)(((b2 & 0x0F) << 2) | (b3 >> 6));
			byte c4 = (byte)(b3 & 0x3F);

			result.Append(base64Chars[c1]);
			result.Append(base64Chars[c2]);
			result.Append(base64Chars[c3]);
			result.Append(base64Chars[c4]);
		}

		switch (data.Length % 3)
		{
			case 1:
				result[result.Length - 2] = '=';
				result[result.Length - 1] = '=';
				break;
			case 2:
				result[result.Length - 1] = '=';
				break;
		}

		return result.ToString();
	}
	public void UploadFile()
	{
		string fileName = "testfilePICTURE.txt";
		string filePath = Application.persistentDataPath + "/" + fileName;

		// Create the text file
		StreamWriter writer = new StreamWriter(filePath);
		writer.Write(base64Encoded);
		writer.Close();

		// Upload the file to the server
		StartCoroutine(UploadFileCoroutine(filePath, serverUrl + fileName));
	}

	IEnumerator UploadFileCoroutine(string filePath, string serverUrl)
	{
		UnityWebRequest www = UnityWebRequest.Post(serverUrl, "");
		www.method = UnityWebRequest.kHttpVerbPUT;
		www.uploadHandler = new UploadHandlerFile(filePath);
		www.downloadHandler = new DownloadHandlerBuffer();

		yield return www.SendWebRequest();

		if (www.result == UnityWebRequest.Result.Success)
		{
			Debug.Log("File upload successful!");
		}
		else
		{
			Debug.LogError("File upload error: " + www.error);
		}
	}



	public void LoadStore()
    {
		userpagetitle.text = userinfo.fName + " " + userinfo.lName;
		foreach(GameObject painting in paintingList)
        {
			Destroy(painting);
        }
		paintingList.Clear();
		CallGetQueryResultGETPAINTINGSOFARTIST(queries.WriteQuery_GETPAINTINGSOFSELLER(userinfo.userID));

	}



	public void CallGetQueryResultGETPAINTINGSOFARTIST(string query)
	{
		Coroutine coroutine = StartCoroutine(queries.GetQueryResult(query, result =>
		{
			if (result == "0 results")
			{
				StoreEmpty.SetActive(true);
			}
			else
			{
				StoreEmpty.SetActive(false);
				DisplayPaintings(queries.ExtractInfo(result));
			}
			


		}));
	}



	void DisplayPaintings(List<List<string>> feedpaintings)
	{

		for (int i = 0; i < feedpaintings.Count; i++)
		{


			GameObject painting = Instantiate(paintingDisplay, transform.position, transform.rotation);
			painting.transform.parent = storescrollParent.transform;
			painting.GetComponent<SetPaintingDisplayInfo>().pID = feedpaintings[i][0];
			painting.GetComponent<SetPaintingDisplayInfo>().title = feedpaintings[i][1];
			painting.GetComponent<SetPaintingDisplayInfo>().price = feedpaintings[i][2];
			painting.GetComponent<SetPaintingDisplayInfo>().imageURL = feedpaintings[i][3];
			painting.GetComponent<SetPaintingDisplayInfo>().description = feedpaintings[i][4];
			painting.GetComponent<SetPaintingDisplayInfo>().rating = feedpaintings[i][5];
			painting.GetComponent<SetPaintingDisplayInfo>().size = feedpaintings[i][6];
			painting.GetComponent<SetPaintingDisplayInfo>().style = feedpaintings[i][7];
			painting.GetComponent<SetPaintingDisplayInfo>().artistFName = feedpaintings[i][8];
			painting.GetComponent<SetPaintingDisplayInfo>().artistLName = feedpaintings[i][9];

			painting.GetComponent<SetPaintingDisplayInfo>().creationDate = feedpaintings[i][10];
			painting.GetComponent<SetPaintingDisplayInfo>().uploadDate = feedpaintings[i][11];
			painting.GetComponent<SetPaintingDisplayInfo>().qt = feedpaintings[i][12];
			painting.GetComponent<SetPaintingDisplayInfo>().artistID = feedpaintings[i][13];
			paintingList.Add(painting);



		}




	}


	void DisplayOrders(List<List<string>> feedpaintings)
	{

		for (int i = 0; i < feedpaintings.Count; i++)
		{


			GameObject painting = Instantiate(paintingOrderDisplay, transform.position, transform.rotation);
			painting.transform.parent = orderscrollParent.transform;
			painting.GetComponent<OrderItem>().orderID = feedpaintings[i][0];
			painting.GetComponent<SetPaintingDisplayInfo>().pID= feedpaintings[i][1];
			painting.GetComponent<OrderItem>().orderDate = feedpaintings[i][2];
			painting.GetComponent<OrderItem>().orderStatus = feedpaintings[i][3];
			painting.GetComponent<OrderItem>().quantity = feedpaintings[i][4];
			painting.GetComponent<SetPaintingDisplayInfo>().title = feedpaintings[i][5];
			painting.GetComponent<SetPaintingDisplayInfo>().style = feedpaintings[i][6];
			painting.GetComponent<SetPaintingDisplayInfo>().price = feedpaintings[i][7];
			painting.GetComponent<SetPaintingDisplayInfo>().size = feedpaintings[i][8];

			painting.GetComponent<SetPaintingDisplayInfo>().imageURL = feedpaintings[i][9];
			painting.GetComponent<SetPaintingDisplayInfo>().description = feedpaintings[i][10];
			painting.GetComponent<SetPaintingDisplayInfo>().rating = feedpaintings[i][11];
			painting.GetComponent<SetPaintingDisplayInfo>().artistFName = feedpaintings[i][12];
			painting.GetComponent<SetPaintingDisplayInfo>().artistLName = feedpaintings[i][13];
			painting.GetComponent<SetPaintingDisplayInfo>().creationDate = feedpaintings[i][14];
			painting.GetComponent<SetPaintingDisplayInfo>().uploadDate = feedpaintings[i][15];
			painting.GetComponent<SetPaintingDisplayInfo>().qt = feedpaintings[i][16];
			painting.GetComponent<SetPaintingDisplayInfo>().artistID = feedpaintings[i][17];
			paintingOrderList.Add(painting);



		}




	}
}




