
public enum GameEventType 
{
   OnConnectedToMaster,
   OnJoinedLobby,
   OnJoinedRoom,

   StartGame,
   SetPlayerIndex,  

   OverrideGameTime,
   UpdateGameTime,
   UpdateGameTurn,

   MeetsPurchaseRequirements,

   SelectInventoryItemForSale,
   ConfirmInventoryItemForSale,
   PublishSellingItemQuantity,
   PublishTeamCombinedSellingItemQuantity,

   OnShopItemButtonSelect,
   SellingBuyingTabActivity,
   TryBuySelectedShopItem,

   SellSpoiledItems,
   DestroySpoiledItem,

   UpdateStockUI,
   UpdateMoneyRegardlessOfSale,

   InventoryItemHoverInfoDisplay,

   UpdateReputationOnSale,
   UpdateReputationOnBuy,

   DisplayPopupNotification,
   OnPopupNotificationClosed
}