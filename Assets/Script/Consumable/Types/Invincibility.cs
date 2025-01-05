using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// The Invincibility consumable makes the player temporarily immune to obstacles and damage.
/// </summary>
public class Invincibility : Consumable
{
    /// <summary>
    /// Returns the name of the consumable.
    /// </summary>
    public override string GetConsumableName()
    {
        return "Invincible";
    }

    /// <summary>
    /// Returns the type of the consumable.
    /// </summary>
    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.INVINCIBILITY;
    }

    /// <summary>
    /// Returns the price of the consumable in standard currency.
    /// </summary>
    public override int GetPrice()
    {
        return 1500;
    }

    /// <summary>
    /// Returns the premium cost of the consumable.
    /// </summary>
    public override int GetPremiumCost()
    {
        return 5;
    }

    /// <summary>
    /// Called every frame while the consumable is active. Ensures the character remains invincible.
    /// </summary>
    /// <param name="c">The character's input controller</param>
    public override void Tick(CharacterInputController c)
    {
        base.Tick(c);

        // Keeps the character explicitly invincible
        c.characterCollider.SetInvincibleExplicit(true);
    }

    /// <summary>
    /// Activates the invincibility effect. Sets the character as invincible for a defined duration.
    /// </summary>
    /// <param name="c">The character's input controller</param>
    /// <returns>An IEnumerator for coroutine functionality.</returns>
    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);

        // Makes the character invincible for the duration of the consumable
        c.characterCollider.SetInvincible(duration);
    }

    /// <summary>
    /// Ends the invincibility effect. Removes the invincible status from the character.
    /// </summary>
    /// <param name="c">The character's input controller</param>
    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);

        // Disables the explicit invincibility
        c.characterCollider.SetInvincibleExplicit(false);
    }
}
