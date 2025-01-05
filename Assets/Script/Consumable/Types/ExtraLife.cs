using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// ExtraLife is a consumable item that grants the player an additional life if they have fewer than the maximum number of lives. 
/// If the maximum number of lives is already reached, it rewards the player with coins instead.
/// </summary>
public class ExtraLife : Consumable
{
    // Maximum number of lives a player can have
    protected const int k_MaxLives = 3;
    
    // Value in coins awarded if the player has the maximum number of lives
    protected const int k_CoinValue = 10;

    /// <summary>
    /// Returns the name of this consumable.
    /// </summary>
    public override string GetConsumableName()
    {
        return "Life";
    }

    /// <summary>
    /// Returns the type of this consumable.
    /// </summary>
    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.EXTRALIFE;
    }

    /// <summary>
    /// Returns the price of the consumable in standard currency.
    /// </summary>
    public override int GetPrice()
    {
        return 2000;
    }

    /// <summary>
    /// Returns the premium cost of the consumable.
    /// </summary>
    public override int GetPremiumCost()
    {
        return 5;
    }

    /// <summary>
    /// Checks if the consumable can be used by the player.
    /// </summary>
    /// <param name="c">The character's input controller</param>
    /// <returns>False if the player already has the maximum number of lives; true otherwise.</returns>
    public override bool CanBeUsed(CharacterInputController c)
    {
        if (c.currentLife == c.maxLife)
            return false;

        return true;
    }

    /// <summary>
    /// Activates the consumable. Adds one life to the player if under the maximum, otherwise rewards coins.
    /// </summary>
    /// <param name="c">The character's input controller</param>
    /// <returns>An IEnumerator for coroutine functionality.</returns>
    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c); // Calls base functionality

        // Adds a life if below the max, otherwise awards coins
        if (c.currentLife < k_MaxLives)
            c.currentLife += 1;
        else
            c.coins += k_CoinValue;
    }
}
