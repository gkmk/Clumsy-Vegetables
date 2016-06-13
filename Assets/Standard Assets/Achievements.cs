using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace GK {
public class Achievements : MonoBehaviour {

		private static List<string> ulockedAchNew = new List<string>();

	public static void giveAchievement(string ID, string pref, int value)
	{
		if (PlayerPrefs.GetInt ("AutoLogin") == 0) return;

		try{
			Social.ReportProgress (ID, 100.0f, (bool success) => {
				if (success) {
					PlayerPrefs.SetInt (pref, value);
							ulockedAchNew.Add(value + " " + pref);
				}
			});
		} catch (UnityException e) {}
	}

		public static bool hasNewAch() { return ulockedAchNew.Count>0; }
		
		public static string getNextAch() { 
			if (!hasNewAch()) return "";
			string tmp = ulockedAchNew[ulockedAchNew.Count-1];
			ulockedAchNew.RemoveAt(ulockedAchNew.Count-1);
			return tmp;
		}
		public static string getCurrentAch() {
			return ulockedAchNew[ulockedAchNew.Count-1];
}

	public static bool updateAchievements()
	{
		if (PlayerPrefs.GetInt ("AutoLogin") == 0) return false;

		//	Unlock Achievement "FIRST FLIGHT"
		if (PlayerPrefs.GetInt ("First Flight", 0) == 0) {
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQAg", "First Flight", 1);

		}

		//	ACIVEMENTY BARIERI
		if (PlayerPrefs.GetInt ("OveralPoeni")>=10 && PlayerPrefs.GetInt ("Barriers Passed") < 10)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQAw", "Barriers Passed", 10); //10 Barriers Passed	
		if (PlayerPrefs.GetInt ("OveralPoeni")>=25 && PlayerPrefs.GetInt ("Barriers Passed") < 20)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQBw", "Barriers Passed", 20); //25 Barriers Passed	
		if (PlayerPrefs.GetInt ("OveralPoeni")>=60 && PlayerPrefs.GetInt ("Barriers Passed") < 60)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQCg", "Barriers Passed", 60); //60 Barriers Passed
		if (PlayerPrefs.GetInt ("OveralPoeni")>=100 && PlayerPrefs.GetInt ("Barriers Passed") < 100)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQCw", "Barriers Passed", 100); //100 Barriers Passed
		
			
		//	COINS EARNED
		if (PlayerPrefs.GetInt ("OveralCoinsEarned")>=10 && PlayerPrefs.GetInt ("Coins Earned") < 10)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQBA", "Coins Earned", 10); //10 Coins Earned - CgkIzZ7Nn4gBEAIQBA	
		if (PlayerPrefs.GetInt ("OveralCoinsEarned")>=50 && PlayerPrefs.GetInt ("Coins Earned") < 50)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQCQ", "Coins Earned", 50); //50 Coins Earned
		if (PlayerPrefs.GetInt ("OveralCoinsEarned")>=100 && PlayerPrefs.GetInt ("Coins Earned") < 100)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQDA", "Coins Earned", 100); //100 Coins Earned
		if (PlayerPrefs.GetInt ("OveralCoinsEarned")>=500 && PlayerPrefs.GetInt ("Coins Earned") < 500)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQDw", "Coins Earned", 500); //500 Coins Earned
		if (PlayerPrefs.GetInt ("OveralCoinsEarned")>=1000 && PlayerPrefs.GetInt ("Coins Earned") < 1000)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQEQ", "Coins Earned", 1000); //1000 Coins Earned
		

		//	COINS SPENT
		if (PlayerPrefs.GetInt ("OveralCoinsSpent")>=100 && PlayerPrefs.GetInt ("Coins Spent") < 100)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQDQ", "Coins Spent", 100); //100 Coins Used
		if (PlayerPrefs.GetInt ("OveralCoinsSpent")>=500 && PlayerPrefs.GetInt ("Coins Spent") < 500)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQDg", "Coins Spent", 500); //500 Coins Used
		if (PlayerPrefs.GetInt ("OveralCoinsSpent")>=1000 && PlayerPrefs.GetInt ("Coins Spent") < 1000)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQEA", "Coins Spent", 1000); //1000 Coins Used
		if (PlayerPrefs.GetInt ("OveralCoinsSpent")>=2500 && PlayerPrefs.GetInt ("Coins Spent") < 2500)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQEw", "Coins Spent", 2500); //2500 Coins Used	
		if (PlayerPrefs.GetInt ("OveralCoinsSpent")>=5000 && PlayerPrefs.GetInt ("Coins Spent") < 5000)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQFA", "Coins Spent", 5000); //5000 Coins Used
		
		
		//	ROCKETS USED
		if (PlayerPrefs.GetInt ("OveralRockets")>=10 && PlayerPrefs.GetInt ("Rockets Used") < 10)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQBQ", "Rockets Used", 10); //10 Rockets Used
		if (PlayerPrefs.GetInt ("OveralRockets")>=50 && PlayerPrefs.GetInt ("Rockets Used") < 50)
				Achievements.giveAchievement(" CgkIzZ7Nn4gBEAIQFw", "Rockets Used", 50); //50 Rockets Used
		if (PlayerPrefs.GetInt ("OveralRockets")>=100 && PlayerPrefs.GetInt ("Rockets Used") < 100)
				Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQGA", "Rockets Used", 100); //100 Rockets Used		
		
		
		
		//	CLOUDS USED
		if (PlayerPrefs.GetInt ("OveralSaves")>=10 && PlayerPrefs.GetInt ("Clouds Used") < 10)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQBg", "Clouds Used", 10); //10 Saves
		if (PlayerPrefs.GetInt ("OveralSaves")>=25 && PlayerPrefs.GetInt ("Clouds Used") < 25)
			Achievements.giveAchievement("CgkIzZ7Nn4gBEAIQCA", "Clouds Used", 25); //25 Saves
		
		
		
		return true;
	}
}
}