using System;
using System.Collections.Generic;
using Soomla;

public class ExampleEventHandler
{
		private const string TAG = "SOOMLA ExampleEventHandler";

		public ExampleEventHandler ()
		{
				Events.OnMarketPurchase += onMarketPurchase;
				Events.OnMarketRefund += onMarketRefund;
				Events.OnItemPurchased += onItemPurchased;
				Events.OnGoodEquipped += onGoodEquipped;
				Events.OnGoodUnEquipped += onGoodUnequipped;
				Events.OnGoodUpgrade += onGoodUpgrade;
				Events.OnBillingSupported += onBillingSupported;
				Events.OnBillingNotSupported += onBillingNotSupported;
				Events.OnMarketPurchaseStarted += onMarketPurchaseStarted;
				Events.OnItemPurchaseStarted += onItemPurchaseStarted;
				Events.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
				Events.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
				Events.OnGoodBalanceChanged += onGoodBalanceChanged;
				Events.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
				Events.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
				Events.OnRestoreTransactions += onRestoreTransactions;
				Events.OnStoreControllerInitialized += onStoreControllerInitialized;
#if UNITY_ANDROID && !UNITY_EDITOR
			Events.OnIabServiceStarted += onIabServiceStarted;
			Events.OnIabServiceStopped += onIabServiceStopped;
#endif
		}
		
		public void onMarketPurchase (PurchasableVirtualItem pvi)
		{
			
		}
		
		public void onMarketRefund (PurchasableVirtualItem pvi)
		{

		}
		
		public void onItemPurchased (PurchasableVirtualItem pvi)
		{
				try {
						GoogleAnalytics.instance.LogScreen ("Bought " + pvi.Name);

						UnityEngine.PlayerPrefs.SetInt ("OveralCoinsSpent", 
							UnityEngine.PlayerPrefs.GetInt ("OveralCoinsSpent") + (((PurchaseWithVirtualItem)pvi.PurchaseType).Amount));
		
						GK.Achievements.updateAchievements ();
				} catch (Exception e) {
				}
		}
		
		public void onGoodEquipped (EquippableVG good)
		{
			
		}
		
		public void onGoodUnequipped (EquippableVG good)
		{
			
		}
		
		public void onGoodUpgrade (VirtualGood good, UpgradeVG currentUpgrade)
		{
			
		}
		
		public void onBillingSupported ()
		{
				StoreUtils.LogDebug (TAG, "SOOMLA/UNITY onBillingSupported");
		}
		
		public void onBillingNotSupported ()
		{
				StoreUtils.LogDebug (TAG, "SOOMLA/UNITY onBillingNotSupported");
		}
		
		public void onMarketPurchaseStarted (PurchasableVirtualItem pvi)
		{
			
		}
		
		public void onItemPurchaseStarted (PurchasableVirtualItem pvi)
		{
			
		}
		
		public void onMarketPurchaseCancelled (PurchasableVirtualItem pvi)
		{
			
		}
		
		public void onUnexpectedErrorInStore (string message)
		{
			
		}
		
		public void onCurrencyBalanceChanged (VirtualCurrency virtualCurrency, int balance, int amountAdded)
		{
				ExampleLocalStoreInfo.UpdateBalances ();
		}
		
		public void onGoodBalanceChanged (VirtualGood good, int balance, int amountAdded)
		{
				ExampleLocalStoreInfo.UpdateBalances ();		
		}
		
		public void onRestoreTransactionsStarted ()
		{
			
		}
		
		public void onRestoreTransactions (bool success)
		{
			
		}
		
		public void onStoreControllerInitialized ()
		{
				ExampleLocalStoreInfo.Init ();
				
			if (StoreInventory.GetItemBalance(CVStore.SAMIJA_ITEM_ID) > 0)
				StoreInventory.EquipVirtualGood(CVStore.SAMIJA_ITEM_ID);
				
		//StoreInventory.GiveItem (CVStore.WATER_DROP_ITEM_ID, 100);
			
				// some usage examples for add/remove currency
				// some examples
				/* if (ExampleLocalStoreInfo.VirtualCurrencies.Count>0) {
				if (StoreInventory.GetItemBalance(ExampleLocalStoreInfo.VirtualCurrencies[0].ItemId)<0){
	                try {
	                    StoreInventory.GiveItem(ExampleLocalStoreInfo.VirtualCurrencies[0].ItemId,100);
	                    StoreUtils.LogDebug("SOOMLA ExampleEventHandler", "Currency balance:" + StoreInventory.GetItemBalance(ExampleLocalStoreInfo.VirtualCurrencies[0].ItemId));
	                } catch (VirtualItemNotFoundException ex){
	                    StoreUtils.LogError("SOOMLA ExampleEventHandler", ex.Message);
	                }
				}
            }
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onStoreControllerInitialized");*/
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public void onIabServiceStarted() {
			
		}
		public void onIabServiceStopped() {
			
		}
#endif
}
