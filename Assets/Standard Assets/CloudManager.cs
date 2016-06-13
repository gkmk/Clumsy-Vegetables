using UnityEngine;
using System.Collections;
using GooglePlayGames;

public class CloudManager : GooglePlayGames.BasicApi.OnStateLoadedListener
{

		// cloud save callbacks
		private GooglePlayGames.BasicApi.OnStateLoadedListener mAppStateListener;
		private bool LoadedFromCloud = false;
	
		private static string GetPlayerStats ()
		{
				return "v1:" + PlayerPrefs.GetInt ("First Flight", 0) + ":" + PlayerPrefs.GetInt ("OveralPoeni")
						+ ":" + PlayerPrefs.GetInt ("OveralCoinsEarned") + ":" + PlayerPrefs.GetInt ("OveralCoinsSpent")
				+ ":" + PlayerPrefs.GetInt ("OveralRockets") + ":" + PlayerPrefs.GetInt ("OveralSaves")+":"+PlayerPrefs.GetInt("FirstRockets");
		}
	
		private static string FromBytes (byte[] b)
		{
				return System.Text.ASCIIEncoding.Default.GetString (b);
		}
	
		private byte[] ProgressToBytes ()
		{
				return System.Text.ASCIIEncoding.Default.GetBytes (GetPlayerStats ());
		}
	
		public void LoadFromCloud ()
		{
				if (LoadedFromCloud)
						return;
				if (Social.localUser.authenticated) {
						// Cloud save is not in ISocialPlatform, it's a Play Games extension,
						// so we have to break the abstraction and use PlayGamesPlatform:
						Debug.Log ("Loading game progress from the cloud.");
						((PlayGamesPlatform)Social.Active).LoadState (0, this);
				}
		}
	
		public void SaveToCloud ()
		{
				if (Social.localUser.authenticated) {
						// Cloud save is not in ISocialPlatform, it's a Play Games extension,
						// so we have to break the abstraction and use PlayGamesPlatform:
						Debug.Log ("Saving progress to the cloud..."+GetPlayerStats ());
						((PlayGamesPlatform)Social.Active).UpdateState (0, ProgressToBytes (), this);
				}
		}
	
		private bool ProcessCloudData (byte[] cloudData)
		{
				if (cloudData == null) {
						Debug.Log ("No data saved to the cloud yet...");
						return false;
				}
				Debug.Log ("Decoding cloud data from bytes.");
				string progress = FromBytes (cloudData);
				string[] p = progress.Split (new char[] { ':' });
				if (!p [0].Equals ("v1")) {
						Debug.LogError ("Failed to parse game progress from: " + progress);
						return false;
				}
				Debug.Log ("Merging with existing game progress.");
				try {
						PlayerPrefs.SetInt ("First Flight", int.Parse (p [1]));
						PlayerPrefs.SetInt ("OveralPoeni", int.Parse (p [2]));
						PlayerPrefs.SetInt ("OveralCoinsEarned", int.Parse (p [3]));
						PlayerPrefs.SetInt ("OveralCoinsSpent", int.Parse (p [4]));
						PlayerPrefs.SetInt ("OveralRockets", int.Parse (p [5]));
						PlayerPrefs.SetInt ("OveralSaves", int.Parse (p [6]));
						PlayerPrefs.SetInt("FirstRockets", int.Parse (p [7]));
				} catch (UnityException e) {
				}
				Debug.Log ("Load from cloud complete.");
				return true;
		}
		// Data was successfully loaded from the cloud
		public void OnStateLoaded (bool success, int slot, byte[] data)
		{
				Debug.Log ("Cloud load callback, success=" + success);
				if (success) {
						LoadedFromCloud = ProcessCloudData (data);
				} else {
						Debug.LogWarning ("Failed to load from cloud. Network problems?");
				}
		
				// regardless of success, this is the end of the auth process
				//mWaitingForAuth = false;
		
				// report any progress we have to report
				//ReportAllProgress();
		}
	
		// Conflict in cloud data occurred
		public byte[] OnStateConflict (int slot, byte[] local, byte[] server)
		{
				Debug.Log ("Conflict callback called. Resolving conflict.");
			string []localS = FromBytes (local).Split(':');
		string []serverS = FromBytes (server).Split(':');
		
		PlayerPrefs.SetInt ("First Flight",  int.Parse(localS[1])>int.Parse(serverS[1])? int.Parse (localS[1]):int.Parse(serverS[1]));
		PlayerPrefs.SetInt ("OveralPoeni",  int.Parse(localS[2])>int.Parse(serverS[2])? int.Parse (localS[2]):int.Parse(serverS[2]));
		PlayerPrefs.SetInt ("OveralCoinsEarned",  int.Parse(localS[3])>int.Parse(serverS[3])? int.Parse (localS[3]):int.Parse(serverS[3]));
		PlayerPrefs.SetInt ("OveralCoinsSpent",  int.Parse(localS[4])>int.Parse(serverS[4])? int.Parse (localS[4]):int.Parse(serverS[4]));
		PlayerPrefs.SetInt ("OveralRockets",  int.Parse(localS[5])>int.Parse(serverS[5])? int.Parse (localS[5]):int.Parse(serverS[5]));
		PlayerPrefs.SetInt ("OveralSaves",  int.Parse(localS[6])>int.Parse(serverS[6])? int.Parse (localS[6]):int.Parse(serverS[6]));
		PlayerPrefs.SetInt("FirstRockets",  int.Parse(localS[7])>int.Parse(serverS[7])? int.Parse (localS[7]):int.Parse(serverS[7]));
		
				/*// decode byte arrays into game progress and merge them
		GameProgress localProgress = local == null ?
			new GameProgress() : GameProgress.FromBytes(local);
		GameProgress serverProgress = server == null ?
			new GameProgress() : GameProgress.FromBytes(server);
		localProgress.MergeWith(serverProgress);
		
		// resolve conflict
		return localProgress.ToBytes();*/
				return null;
		}
	
		public void OnStateSaved (bool success, int slot)
		{
				if (!success) {
						Debug.LogWarning ("Failed to save state to the cloud.");
			
						// try to save later:
						//mProgress.Dirty = true;
				} else
						Debug.Log ("Saved to cloud slot: " + slot.ToString ());
		}
}
