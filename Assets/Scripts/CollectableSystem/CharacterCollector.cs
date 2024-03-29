using UnityEngine;

public class CharacterCollector : ElementOf<Application>
{
    private void OnTriggerEnter(Collider other)
    {
        var collectable = other.GetComponent<Collectable>();
        if (collectable != null)
        {
            CollectCollectable(collectable);
        }
    }

    /// <summary>
    /// Sends notification by collectable type.
    /// </summary>
    /// <param name="collectable"></param>
    private void CollectCollectable(Collectable collectable)
    {
        collectable.OnCollect();
        var stackable = collectable as Stackable;
        if (stackable)
        {
            Master.Notify(CollectorNotification._CollectStackable, stackable);
            return;
        }
        var currency = collectable as Currency;
        if (currency)
        {
            Master.Notify(CollectorNotification._CollectCurrency, currency);
            return;
        }

        var obstacle = collectable as Obstacle;
        if (obstacle)
        {
            Master.Notify(CollectorNotification._CollectObstacle,obstacle);
        }

    }
}