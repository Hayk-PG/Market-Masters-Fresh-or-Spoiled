
public enum GameEventType 
{
   SelectOfflineGame,
   SelectOnlineGame,

   OnConnectedToMaster,
   OnJoinedLobby,
   OnJoinedRoom,

   StartGame,
   EndGame,
   SetPlayerIndex,  

   OverrideGameTime,
   UpdateGameTime,
   UpdateGameTurn,

   MeetsPurchaseRequirements,

   RequestStorageUIOpen,
   OpenStorageUI,
   CloseStorageUI,
   SelectStorageItem,
   SendStorageItemForVerification,
   SubmitStorageItem,
   UpdateStorageRentalFee,

   CalculateStorageSpaceFee,

   InventoryItemDragNDrop,
   SelectInventoryItemForSale,
   ConfirmInventoryItemForSale,
   PublishSellingItemQuantity,
   PublishTeamCombinedSellingItemQuantity,
   PublishInventoryData,

   UpdateShopItems,
   OnShopItemButtonSelect,
   OnShopItemButtonDeselect,
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
   UpdateReputationForItemExchange,
   UpdateReputationOnSanitationInspection,

   RestrictSaleAbility,

   DisplayErrorMessage,
   CloseErrorMessage,

   QueueNotification,
   DisplayNotification,
   DisplayNextNotification,
   DisplayPopupNotification,
   OnPopupNotificationClosed
}