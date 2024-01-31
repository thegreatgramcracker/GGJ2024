using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegenMenu : MonoBehaviour
{
    public Slider regenAmountSlider;
    public TextMeshProUGUI amountText;
    public BattleManager battleManager;
    public WorldManager worldManager;
    public Move regenMoveTemplate;
    int regenAmount = 0;
    int maxHPRestore = 0;


    private void OnEnable()
    {
        regenAmountSlider.value = 0;
        RecalculateRegenEffects();
    }

    // Update is called once per frame
    void Update()
    {
        if (regenAmount > maxHPRestore)
        {
            amountText.color = Color.red;
        }
        else
        {
            amountText.color = Color.white;
        }
        amountText.text = maxHPRestore.ToString();
    }

    public void RecalculateRegenEffects()
    {
        regenAmount = Mathf.FloorToInt(Mathf.Lerp(battleManager.player.hp, battleManager.player.maxHP, regenAmountSlider.value)) - battleManager.player.hp;
        

        maxHPRestore = 0;

        Dictionary<Animal, int> animalChanges = new Dictionary<Animal, int>();
        List<AnimalChangeEffect> moveAnimalChanges = new List<AnimalChangeEffect>();
        for (int hp = 0; hp < regenAmount; hp++)
        {

            Animal animalToLeech = worldManager.GetHighestLifeEnergy();
            int leechAmount = Mathf.CeilToInt(1 / animalToLeech.lifeEnergy);
            if (leechAmount > worldManager.animalAmounts[animalToLeech])
            {
                maxHPRestore = hp;
                break;
            }
            if (!animalChanges.ContainsKey(animalToLeech))
            {
                animalChanges[animalToLeech] = 0;
            }
            animalChanges[animalToLeech] += leechAmount;
            maxHPRestore++;
        }
        regenMoveTemplate.animalChanges.Clear();
        foreach (Animal animal in animalChanges.Keys)
        {
            AnimalChangeEffect currentChangeEffect = new AnimalChangeEffect();
            currentChangeEffect.animals.Add(animal);
            currentChangeEffect.change = -1 * animalChanges[animal];
            regenMoveTemplate.animalChanges.Add(currentChangeEffect);
        }

        regenMoveTemplate.hpChange = maxHPRestore;

    }

}
