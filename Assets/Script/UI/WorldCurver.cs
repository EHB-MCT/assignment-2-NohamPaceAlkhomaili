using UnityEngine;

[ExecuteInEditMode] // Permet d'exécuter ce script même lorsque l'application n'est pas en mode de lecture.
public class WorldCurver : MonoBehaviour
{
    // Définit une force de courbure réglable avec une plage limitée dans l'éditeur.
    [Range(-0.1f, 0.1f)]
    public float curveStrength = 0.01f;

    // Identifiant de la propriété shader utilisée pour appliquer la courbure.
    int m_CurveStrengthID;

    /// <summary>
    /// Appelé lorsque l'objet est activé. Initialise l'ID de la propriété shader.
    /// </summary>
    private void OnEnable()
    {
        m_CurveStrengthID = Shader.PropertyToID("_CurveStrength"); // Obtient l'ID unique de la propriété "_CurveStrength".
    }

    /// <summary>
    /// Met à jour la valeur globale de la courbure dans le shader à chaque frame.
    /// </summary>
    void Update()
    {
        Shader.SetGlobalFloat(m_CurveStrengthID, curveStrength); // Définit la valeur de courbure globale pour tous les shaders utilisant "_CurveStrength".
    }
}
