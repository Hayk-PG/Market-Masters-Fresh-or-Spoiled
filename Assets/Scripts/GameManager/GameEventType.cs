
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
   UpdateStorageRentalFee,

   CalculateStorageSpaceFee,

   SelectInventoryItemForSale,
   ConfirmInventoryItemForSale,
   PublishSellingItemQuantity,
   PublishTeamCombinedSellingItemQuantity,

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

   RestrictSaleAbility,

   DisplayErrorMessage,
   CloseErrorMessage,

   QueueNotification,
   DisplayNotification,
   DisplayNextNotification,
   DisplayPopupNotification,
   OnPopupNotificationClosed
}