using Narratore;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RecordLevelModeButton : MonoBehaviour
{
    [SerializeField] private GameLoop _loop;
    [SerializeField] private LevelModeKey _recordLevelModeKey;


    private Button _button;
    
    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _loop.BeginLevel(_recordLevelModeKey);
    }
}
