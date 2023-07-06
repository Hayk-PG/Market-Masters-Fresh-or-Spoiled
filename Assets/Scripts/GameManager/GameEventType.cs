
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

   InventoryItemDragNDrop,

   RequestStorageUIOpen,
   OpenStorageUI,
   CloseStorageUI,
   SelectStorageItem,
   SendStorageItemForVerification,
   SubmitStorageItem,

   CalculateStorageSpaceFee,

   SelectInventoryItemForSale,
   ConfirmInventoryItemForSale,
   PublishSellingItemQuantity,
   PublishTeamCombinedSellingItemQuantity,

   UpdateShopItems,
   OnShopItemButtonSelect,
   SellingBuyingTabActivity,
   TryBuySelectedShopItem,

   SellSpoiledItems,
   DestroySpoiledItem,

   UpdateStockUI,
   UpdateMoneyRegardlessOfSale,

   InventoryItemHoverInfoDisplay,

   SendReputationPoints,
   UpdateReputationOnSale,
   UpdateReputationOnBuy,
   UpdateReputationOnEmptyInventory,

   RestrictSaleAbility,

   QueueNotification,
   DisplayNotification,
   DisplayNextNotification,
   DisplayPopupNotification,
   OnPopupNotificationClosed
}