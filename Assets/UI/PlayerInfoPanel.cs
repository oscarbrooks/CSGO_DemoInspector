using DemoInfo;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    public Text NameText;
    public Text HealthText;
    public Text MoneyText;
    public RectTransform Background;

    public bool IsAlive { get; set; }
    
    private float _startWidth;

    public Image WeaponIcon;
    private LayoutElement _weaponLayoutElement;

    [SerializeField]
    private string _steamId;

    [SerializeField]
    private SpectateController _spectateController;

    private void Start()
    {
        _weaponLayoutElement = WeaponIcon.GetComponent<LayoutElement>();

        _startWidth = Background.sizeDelta.x;

        _spectateController = Camera.main.GetComponent<SpectateController>();

        transform.Find("LowerSection/SpectateButton").GetComponent<Button>().onClick.AddListener(() => OnSpectate());

        StartCoroutine(UpdateVisuals());
    }

    private IEnumerator UpdateVisuals()
    {
        while (true)
        {
            if (PlaybackManager.Instance.Frames.Count == 0) yield return null;

            var players = PlaybackManager.Instance.Frames[PlaybackManager.Instance.CurrentFrame].Players;

            var player = players.FirstOrDefault(p => p.SteamID == _steamId);

            if (player.SteamID == null) yield return new WaitForSeconds(0.1f);

            UpdateHealth(player);

            MoneyText.text = "$" + player.Money;

            UpdateWeaponIcon(player.Weapon);

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void Initialize(PartialPlayer player, Transform parentPanel)
    {
        _steamId = player.SteamID;
        NameText.text = player.Name;
    }

    private void OnSpectate()
    {
        var player = GameObject.Find("Player " + _steamId);

        if (player == null) return;

        _spectateController.SetPlayer(player.GetComponent<PlayerGraphics>());
    }

    private void UpdateWeaponIcon(PartialWeapon weapon)
    {
        if (weapon == null || weapon.Name == "NONE") return;

        var folder = GetWeaponFolder(weapon.EquipmentClass);

        var weaponSprite = Resources.Load<Sprite>($"icons/{folder}/{weapon.Name.ToLowerInvariant()}");

        if(weaponSprite == null)
        {
            Debug.Log($"Can't load {weapon.Name.ToLowerInvariant()} from {folder}");
            return;
        }

        WeaponIcon.sprite = weaponSprite;
        _weaponLayoutElement.preferredWidth = WeaponIcon.sprite.rect.size.x / WeaponIcon.sprite.rect.size.y * WeaponIcon.rectTransform.sizeDelta.y;
    }

    private string GetWeaponFolder(EquipmentClass equipmentClass)
        => equipmentClass == EquipmentClass.Equipment
            || equipmentClass == EquipmentClass.Grenade
            ? "utility" : "weapons";

    private void UpdateHealth(PartialPlayer player)
    {
        HealthText.text = player.Health.ToString();
        Background.sizeDelta = new Vector2((float)player.Health / 100 * _startWidth, Background.sizeDelta.y);
    }
}
