using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelSelectorManager : MonoBehaviour
{
    [SerializeField] private WaveList waveList;
    [SerializeField] private LevelSelectorItem levelItem;
    [SerializeField] private Transform content;
    [SerializeField] private ScrollRect wavesScroll;

    private List<LevelSelectorItem> levelItems = new List<LevelSelectorItem>();
    private WaveSO currentWaveSelected;

    private void Start()
    {
        InitiateLevelItems();
        Invoke("FocusItemContent", .1f);
    }

    private void FocusItemContent()
    {
        ScrollViewFocusFunctions.FocusOnItem(wavesScroll, levelItems[DataManager.instance.gameData.CurrentGameLevel].GetComponent<RectTransform>());
    }

    private void InitiateLevelItems()
    {
        for (int i = 0; i < waveList.waveList.Count; i++)
        {
            LevelSelectorItem currentItem = Instantiate(levelItem, content);
            currentItem.UpdateSelectorItem(i);
            levelItems.Add(currentItem);

            AddItemWaveEvent(i);
            UnlockItem(i);
        }
    }

    private void AddItemWaveEvent(int index)
    {
        UnityAction selectWaveAction = () => SelectWave(index);
        levelItems[index].GetComponent<Button>().onClick.AddListener(selectWaveAction);
    }

    private void UnlockItem(int index)
    {
        if (index <= DataManager.instance.gameData.MaxGameLevel)
            levelItems[index].UnlockLevel();
    }

    public void SelectWave(int itemIndex)
    {
        currentWaveSelected = waveList.waveList[itemIndex];
        DataManager.instance.gameData.CurrentGameLevel = itemIndex;

        ChangeSceneToGame();
    }

    private void ChangeSceneToGame()
    {
        TransitionController.instance.TransitionToSceneName(currentWaveSelected.SceneName);
    }
}
