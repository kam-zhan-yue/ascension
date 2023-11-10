using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;

public class MyClass
{
	public string username;
	public string score;
	public string test;
}

public class leaderBoard : MonoBehaviour
{
	public TextMeshProUGUI Line1;
	public TextMeshProUGUI Line2;
	public TextMeshProUGUI Line3;
	public TextMeshProUGUI Line4;
	public TextMeshProUGUI Line5;
	public TextMeshProUGUI Score1;
	public TextMeshProUGUI Score2;
	public TextMeshProUGUI Score3;
	public TextMeshProUGUI Score4;
	public TextMeshProUGUI Score5;
	public static PubNub pubnub;

	public Button SubmitButton;
	public InputField FieldUsername;
	public InputField FieldScore;
	//public Object[] tiles = {}
	// Use this for initialization
	void Start()
	{
		//Button btn = SubmitButton.GetComponent<Button>();
		//btn.onClick.AddListener(TaskOnClick);

		// Use this for initialization
		PNConfiguration pnConfiguration = new PNConfiguration();
		pnConfiguration.PublishKey = "pub-c-a3e4617f-8425-4eaa-b403-80125e209564";
		pnConfiguration.SubscribeKey = "sub-c-760a86b6-4330-11eb-9d3f-7e8713e36938";

		pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
		pnConfiguration.UUID = Random.Range(0f, 999999f).ToString();

		pubnub = new PubNub(pnConfiguration);
		Debug.Log(pnConfiguration.UUID);


		MyClass myFireObject = new MyClass();
		myFireObject.test = "new user";
		string fireobject = JsonUtility.ToJson(myFireObject);
		pubnub.Fire()
			.Channel("my_channel")
			.Message(fireobject)
			.Async((result, status) => {
				if (status.Error)
				{
					Debug.Log(status.Error);
					Debug.Log(status.ErrorData.Info);
				}
				else
				{
					Debug.Log(string.Format("Fire Timetoken: {0}", result.Timetoken));
				}
			});

		pubnub.SusbcribeCallback += (sender, e) => {
			SusbcribeEventEventArgs mea = e as SusbcribeEventEventArgs;
			if (mea.Status != null)
			{
			}
			if (mea.MessageResult != null)
			{
				Dictionary<string, object> msg = mea.MessageResult.Payload as Dictionary<string, object>;

				string[] strArr = msg["username"] as string[];
				string[] strScores = msg["score"] as string[];

				int usernamevar = 1;
				foreach (string username in strArr)
				{
					string usernameobject = "Line" + usernamevar;
					GameObject.Find(usernameobject).GetComponent<TextMeshProUGUI>().text = username.ToString();
					usernamevar++;
					Debug.Log(username);
				}

				int scorevar = 1;
				foreach (string score in strScores)
				{
					string scoreobject = "Score" + scorevar;
					GameObject.Find(scoreobject).GetComponent<TextMeshProUGUI>().text = score.ToString();
					scorevar++;
					Debug.Log(score);
				}
			}
			if (mea.PresenceEventResult != null)
			{
				Debug.Log("In Example, SusbcribeCallback in presence" + mea.PresenceEventResult.Channel + mea.PresenceEventResult.Occupancy + mea.PresenceEventResult.Event);
			}
		};
		pubnub.Subscribe()
			.Channels(new List<string>() {
				"my_channel2"
			})
			.WithPresence()
			.Execute();
	}

	public static void AddLeaderBoard(int score, string name)
	{
		var usernametext = name;
		var scoretext = score.ToString();
		MyClass myObject = new MyClass();
		myObject.username = name;
		myObject.score = score.ToString();
		string json = JsonUtility.ToJson(myObject);

		pubnub.Publish()
			.Channel("my_channel")
			.Message(json)
			.Async((result, status) => {
				if (!status.Error)
				{
					Debug.Log(string.Format("Publish Timetoken: {0}", result.Timetoken));
				}
				else
				{
					Debug.Log(status.Error);
					Debug.Log(status.ErrorData.Info);
				}
			});
		//Output this to console when the Button is clicked
		Debug.Log("You have clicked the button!");
	}

}
