using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace AbsentUtilities;

[PublicAPI]
public static class StatusIconHelper
{
    public static StringTable KeyCollection => LocalizationHelper.GetCollection("Tooltips", SystemLanguage.English);

    // Creates the GameObject that is the icon
    // Make sure type is set to the same string as what you set type to for your status effect
    // copyTextFrom copies the text formating from an existing icon
    // textBoxLocation: -1 left, 0 center, +1 right of icon
    public static GameObject CreateIcon(string name, Sprite sprite, string type, int textBoxLocation, string copyTextFrom, Color textColor,
        KeywordData[] keys)
    {
        GameObject gameObject = new(name);
        Object.DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
        StatusIcon icon = gameObject.AddComponent<StatusIconExt>();
        var cardIcons = CardManager.cardIcons;
        if (!copyTextFrom.IsNullOrEmpty())
        {
            var text = cardIcons[copyTextFrom].GetComponentInChildren<TextMeshProUGUI>().gameObject
                .InstantiateKeepName();
            text.transform.SetParent(gameObject.transform);
            icon.textElement = text.GetComponent<TextMeshProUGUI>();
            icon.textColour = textColor;
            icon.textColourAboveMax = textColor;
            icon.textColourBelowMax = textColor;
        }

        icon.onCreate = new UnityEvent();
        icon.onDestroy = new UnityEvent();
        icon.onValueDown = new UnityEventStatStat();
        icon.onValueUp = new UnityEventStatStat();
        icon.afterUpdate = new UnityEvent();
        var image = gameObject.AddComponent<Image>();
        image.sprite = sprite;
        var cardHover = gameObject.AddComponent<CardHover>();
        cardHover.enabled = false;
        cardHover.IsMaster = false;
        var cardPopUp = gameObject.AddComponent<CardPopUpTarget>();
        cardPopUp.keywords = keys;
        cardHover.pop = cardPopUp;
        cardPopUp.posX = textBoxLocation; // Set keyword box location (-1 left, 0 center, +1 right)
        var rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.sizeDelta *= 0.01f;
        gameObject.SetActive(true);
        icon.type = type;
        cardIcons[type] = gameObject;

        return gameObject;
    }

    //This creates the keyword
    public static KeywordData CreateIconKeyword(string name, string title, string desc, string icon, Color titleC, Color body,
        Color note, Color? panel = null)
    {
        var data = ScriptableObject.CreateInstance<KeywordData>();
        data.name = name;
        KeyCollection.SetString(data.name + "_text", title);
        data.titleKey = KeyCollection.GetString(data.name + "_text");
        KeyCollection.SetString(data.name + "_desc", desc);
        data.descKey = KeyCollection.GetString(data.name + "_desc");
        data.showIcon = true;
        data.showName = false;
        data.iconName = icon;
        data.ModAdded = AbsentUtils.GetModInfo(Assembly.GetCallingAssembly()).Mod;
        data.bodyColour = body;
        data.titleColour = titleC;
        data.noteColour = note;
        data.panelColor = panel ?? Color.black;
        AddressableLoader.AddToGroup("KeywordData", data);
        return data;
    }

    //This custom class extends the StatusIcon class to automatically add listeners so that the number on the icon will update automatically
    public class StatusIconExt : StatusIcon
    {
        public override void Assign(Entity entity)
        {
            base.Assign(entity);
            SetText();
            onValueDown.AddListener(delegate { Ping(); });
            onValueUp.AddListener(delegate { Ping(); });
            afterUpdate.AddListener(SetText);
            onValueDown.AddListener(CheckDestroy);
        }
    }
}