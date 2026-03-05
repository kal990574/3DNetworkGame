using ExitGames.Client.Photon;
using UnityEngine;

public class PlayerModelAbility : PlayerAbility
{
    [SerializeField] private GameObject[] _maleParts;
    [SerializeField] private GameObject[] _femaleParts;

    private void Start()
    {
        ApplyModel();
    }

    private void ApplyModel()
    {
        bool isMale = GetGenderFromProperties() == ECharacterType.Male.ToString();

        foreach (GameObject part in _maleParts)
            part.SetActive(isMale);

        foreach (GameObject part in _femaleParts)
            part.SetActive(!isMale);
    }

    private string GetGenderFromProperties()
    {
        Hashtable props = photonView.Owner.CustomProperties;

        if (props.TryGetValue(UI_Lobby.PLAYER_GENDER_KEY, out object genderObj))
        {
            return genderObj as string ?? ECharacterType.Male.ToString();
        }

        return ECharacterType.Male.ToString();
    }
}
