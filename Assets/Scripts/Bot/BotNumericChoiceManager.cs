using System.Collections;
using UnityEngine;

public class BotNumericChoiceManager : PlayerNumericChoiceManager
{
    protected override void PublishConfirmedNumber(GameEventType gameEventType, object[] data)
    {
        if (!CanPublishConfirmedNumber(gameEventType))
        {
            return;
        }

        int selectedNumber = Random.Range(1, 7);
        ToggleConfirmedNumberPublished();
        StartCoroutine(PublishBotSelectedNumberAfterRandomDelay(selectedNumber));
    }

    private IEnumerator PublishBotSelectedNumberAfterRandomDelay(int selectedNumber)
    {
        float randomDelay = Random.Range(1f, 10f);
        yield return new WaitForSeconds(randomDelay);
        GameSceneReferences.Manager.RemoteRPCWrapper.PublishBotSelectedNumber(_entityManager.EntityName, _entityManager.EntityActorNumber, selectedNumber);
    }

    protected override bool CanPublishConfirmedNumber(GameEventType currentGameEventType)
    {
        return currentGameEventType == GameEventType.UpdateGameTurn && !_isConfirmedNumberPublished;
    }
}