using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla {
public class CVStore : IStoreAssets {

	public int GetVersion() {
		return 5;
	}
	
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{COIN_CURRENCY};
	}
	
	public VirtualGood[] GetGoods() {
			return new VirtualGood[] {ROCKET_GOOD, PAVLOVA_GOOD, SAMIJA_GOOD};
	}
	
	public VirtualCurrencyPack[] GetCurrencyPacks() {
			return new VirtualCurrencyPack[] {HUNCOINS_PACK, TREHUNCOINS_PACK, SEVHUNCOINS_PACK, FIFTHUNCOINS_PACK};
	}
	
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{GENERAL_CATEGORY};
	}
	
	public NonConsumableItem[] GetNonConsumableItems() {
		return new NonConsumableItem[]{NO_ADDS_NONCONS};
	}
	
	/** Static Final members **/
	
	public const string COIN_CURRENCY_ITEM_ID      = "currency_coin";
		public const string HUNCOINS_PACK_PRODUCT_ID      = "ten_coins";
		public const string TREHUNCOINS_PACK_PRODUCT_ID    = "coins_300";
		public const string SEVHUNCOINS_PACK_PRODUCT_ID = "coins_700";
		public const string FIFTHUNCOINS_PACK_PRODUCT_ID = "coins_1500";
	public const string NO_ADDS_NONCONS_PRODUCT_ID   = "no_ads";
	
		public const string ROCKET_ITEM_ID   = "rockets";
		public const string WATER_DROP_ITEM_ID   = "water_drop";
		public const string SAMIJA_ITEM_ID   = "ninja_belt";
	
	
	/** Virtual Currencies **/
	public static VirtualCurrency COIN_CURRENCY = new VirtualCurrency(
		"Coins",
		"",
		COIN_CURRENCY_ITEM_ID
		);
	
	
	/** Virtual Currency Packs **/
	
	public static VirtualCurrencyPack HUNCOINS_PACK = new VirtualCurrencyPack(
		"100 Coins",                                   // name
		"Test refund of an item",                       // description
		"coins_100",                                   // item id
		100,												// number of currencies in the pack
		COIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
			new PurchaseWithMarket(HUNCOINS_PACK_PRODUCT_ID, 1.99)
		);
	
		public static VirtualCurrencyPack TREHUNCOINS_PACK = new VirtualCurrencyPack(
			"300 Coins",                                   // name
		"Test cancellation of an item",                 // description
			"coins_300",                                   // item id
		300,                                             // number of currencies in the pack
		COIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
			new PurchaseWithMarket(TREHUNCOINS_PACK_PRODUCT_ID, 4.99)
		);
	
		public static VirtualCurrencyPack SEVHUNCOINS_PACK = new VirtualCurrencyPack(
			"700 Coins",                                  // name
		"Test purchase of an item",                 	// description
			"coins_700",                                  // item id
			700,                                            // number of currencies in the pack
		COIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
			new PurchaseWithMarket(SEVHUNCOINS_PACK_PRODUCT_ID, 9.99)
		);
	
		public static VirtualCurrencyPack FIFTHUNCOINS_PACK = new VirtualCurrencyPack(
			"1500 Coins",                                 // name
		"Test item unavailable",                 		// description
			"coins_1500",                                 // item id
		1500,                                           // number of currencies in the pack
		COIN_CURRENCY_ITEM_ID,                        // the currency associated with this pack
			new PurchaseWithMarket(FIFTHUNCOINS_PACK_PRODUCT_ID, 19.99)
		);
	
	/** Virtual Goods **/
	
	public static VirtualGood ROCKET_GOOD = new SingleUseVG(
		"Rocket",                                       // name
		"Use rockets to blast away obstacles in the game", // description
			ROCKET_ITEM_ID,                                       // item id
			new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID, 10)); // the way this virtual good is purchased
	
	public static VirtualGood PAVLOVA_GOOD = new SingleUseVG(
			"Water Drop",                                          // name
		"Continue where you left of buy reviving your character with this magical water drop",    // description
		"water_drop",                                          // item id
			new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID, 25)); // the way this virtual good is purchased

		public static VirtualGood SAMIJA_GOOD = new EquippableVG(
			EquippableVG.EquippingModel.CATEGORY,
			"Ninja Belt",                                          // name
			"Give your character ninja look",    // description
			"ninja_belt",                                          // item id
			new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID, 85)); // the way this virtual good is purchased
	
	
	/** Virtual Categories **/
	// The muffin rush theme doesn't support categories, so we just put everything under a general category.
	public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
			"General", new List<string>(new string[] { ROCKET_ITEM_ID, WATER_DROP_ITEM_ID, SAMIJA_ITEM_ID })
		);
	
	
	/** Google MANAGED Items **/
	
	public static NonConsumableItem NO_ADDS_NONCONS  = new NonConsumableItem(
		"No Ads",
		"Test purchase of MANAGED item.",
		"no_ads",
		new PurchaseWithMarket(new MarketItem(NO_ADDS_NONCONS_PRODUCT_ID, MarketItem.Consumable.NONCONSUMABLE , 1.99))
		);
	}
}
