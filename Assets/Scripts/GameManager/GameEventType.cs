
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
   PublishStorageDiscountRemainingTime,

   CalculateStorageSpaceFee,

   ActivateItemsDroppingHelicopter,

   InventoryItemDragNDrop,
   SelectInventoryItemForSale,
   ConfirmInventoryItemForSale,
   PublishSellingItemQuantity,
   PublishTeamCombinedSellingItemQuantity,
   PublishInventoryData,
   ChangeInventoryItem,
   RecordSaleAttempts,

   UpdateShopItems,
   OnShopItemButtonSelect,
   OnShopItemButtonDeselect,
   SellingBuyingTabActivity,
   TryBuySelectedShopItem,

   SellSpoiledItems,
   GetMoneyFromSellingSpoiledItems,
   DestroySpoiledItem,
   ItemSpoilageSurge,
   UpdateSpoilageRate,

   UpdateStockUI,
   UpdateMoneyRegardlessOfSale,

   InventoryItemHoverInfoDisplay,

   SendReputationPoints,
   UpdateReputationOnSale,
   UpdateReputation,

   RestrictSaleAbility,

   DisplayErrorMessage,
   CloseErrorMessage,

   QueueNotification,
   DisplayNotification,
   DisplayNextNotification,
   RemoveNotificationCallback,
   DisplayPopupNotification,
   OnPopupNotificationClosed,

   TriggerItemRecallAlert,
   DispatchRecallItems
}