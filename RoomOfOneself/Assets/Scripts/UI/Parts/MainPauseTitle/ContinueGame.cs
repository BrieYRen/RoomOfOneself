using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this script should be attach to a UI button
/// when player click the button, they'll exit main menu and can play the game from their previous progression
/// the method will be added automatically when start, no need to configurate it in OnClick area
/// </summary>
[RequireComponent(typeof(Button))]
public class ContinueGame : MonoBehaviour
{
    [SerializeField]
    UIPanel toClosePanel;

    Button button;


    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }


    public void OnClickButton()
    {
        toClosePanel.Close();
    }
}