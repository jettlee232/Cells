using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInteraction_MitoTuto : MonoBehaviour
{
    public static CheckInteraction_MitoTuto Instance { get; private set; }

    private Dictionary<string, bool> descriptionsShown = new Dictionary<string, bool>();
    private Dictionary<string, bool> itemsGrabbed = new Dictionary<string, bool>();

    // 필수 설명 및 아이템 이름 리스트
    public List<string> requiredDescriptions = new List<string> { "Adenine", "Ribose", "Phosphate" };
    public List<string> requiredItems = new List<string> { "Adenine", "Ribose", "Phosphate" };
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var desc in requiredDescriptions)
        {
            descriptionsShown[desc] = false;
        }

        foreach (var item in requiredItems)
        {
            itemsGrabbed[item] = false;
        }
    }

    void Update()
    {
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        bool allDescriptionsShown = true;
        bool allItemsGrabbed = true;

        foreach (var desc in requiredDescriptions)
        {
            if (!descriptionsShown[desc])
            {
                allDescriptionsShown = false;
                break;
            }
        }

        foreach (var item in requiredItems)
        {
            if (!itemsGrabbed[item])
            {
                allItemsGrabbed = false;
                break;
            }
        }

        if (allDescriptionsShown && allItemsGrabbed)
        {
            DialogueToMyATPMix();
        }
    }

    private void DialogueToMyATPMix()
    {
        DialogueController_MitoTuto.Instance.ActivateDST(11);
    }

    public void SetDescriptionShown(string itemName)
    {
        if (descriptionsShown.ContainsKey(itemName))
        {
            descriptionsShown[itemName] = true;
        }
    }

    public void SetItemGrabbed(string itemName)
    {
        if (itemsGrabbed.ContainsKey(itemName))
        {
            itemsGrabbed[itemName] = true;
        }
    }
}