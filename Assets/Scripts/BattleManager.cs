using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public Enemy[] enemyLineup;
    public List<Move> playerMovePool = new List<Move>();
    public WorldManager worldManager;

    public TextMeshProUGUI playerHPText, enemyHPText, battleText, environmentShiftText;

    public Transform moveMenu, commandMenu, gameOverPanel, victoryPanel;
    public Image enemySprite;

    int enemyIndex = 0;
    int turnsUntilEnvironmentChange = 3;
    Move selectedMove;
    bool gameOver = false, victory = false;

    
    

    // Start is called before the first frame update
    void Start()
    {
        LoadEnemy(0);
        playerMovePool.Shuffle();
        turnsUntilEnvironmentChange = 3;
    }

    // Update is called once per frame
    void Update()
    {
        player.hp = Mathf.Clamp(player.hp, 0, player.maxHP);
        enemy.hp = Mathf.Clamp(enemy.hp, 0, enemy.maxHP);
        playerHPText.text = "HP: " + player.hp;
        enemyHPText.text = "HP: " + enemy.hp;
        environmentShiftText.text = "Turns until the environment shifts: " + turnsUntilEnvironmentChange;
        if ((gameOver || victory) && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ChooseMove(Move move)
    {
        selectedMove = move;
        playerMovePool.Shuffle();
        StartCoroutine(ExecuteTurn());
    }

    void LoadEnemy(int index)
    {
        commandMenu.gameObject.SetActive(true);
        enemy = Instantiate(enemyLineup[index]);
        battleText.text = enemy.name + " appears!";
        enemySprite.sprite = enemy.sprite;
    }

    IEnumerator ExecuteTurn()
    {
        Move enemyMove = enemy.moveList[Random.Range(0, enemy.moveList.Length)];
        

        //player action
        battleText.text = "You use " + selectedMove.name + ".";
        yield return null;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));


        int damage = Mathf.FloorToInt(Random.Range(selectedMove.damageMin, selectedMove.damageMax + 1) * 
            (selectedMove.animalScaler == null ? 1f : Mathf.Max(0.5f, worldManager.animalAmounts[selectedMove.animalScaler] / selectedMove.animalScaler.startAmount)));
        if (damage > 0)
        {
            enemy.hp -= damage;
            battleText.text = enemy.name + " takes " + damage + "!";
            yield return null;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        int selfHP = selectedMove.hpChange;

        if (selfHP != 0)
        {
            player.hp += selfHP;
            battleText.text = "You restore " + selfHP + " health!";
            yield return null;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }
        
        
        foreach (AnimalChangeEffect animalChange in selectedMove.animalChanges)
        {
            foreach (Animal animal in animalChange.animals)
            {
                int difference = (Mathf.RoundToInt(worldManager.animalAmounts[animal] * animalChange.rate) + animalChange.change - worldManager.animalAmounts[animal]);
                worldManager.animalAmounts[animal] += difference;
                if (difference > 0)
                {
                    battleText.text = animal.name + " population is increased by " + difference + ".";
                }
                else if (difference < 0)
                {
                    battleText.text = animal.name + " population is decreased by " + Mathf.Abs(difference) + ".";
                }
                else
                {
                    continue;
                }
                yield return null;
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }
        }

        if (enemy.hp <= 0)
        {
            battleText.text = "You defeated " + enemy.name + "!";
            yield return null;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            enemyIndex++;
            if (enemyIndex >= enemyLineup.Length)
            {
                victory = true;
                victoryPanel.gameObject.SetActive(true);
                
            }
            else
            {
                LoadEnemy(enemyIndex);
            }
            yield break;
        }

        //Enemy action

        battleText.text = enemy.name + " uses " + enemyMove.name + ".";
        yield return null;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        damage = Mathf.FloorToInt(Random.Range(enemyMove.damageMin, enemyMove.damageMax + 1) *
            (enemyMove.animalScaler == null ? 1f : Mathf.Max(0.5f, worldManager.animalAmounts[enemyMove.animalScaler] / enemyMove.animalScaler.startAmount))
            * (selectedMove.cutsDamage ? 0.5f : 1f));
        if (damage > 0)
        {
            player.hp -= damage ;
            battleText.text = "You take " + damage + "!";
            if (player.hp <= 0)
            {
                gameOverPanel.gameObject.SetActive(true);
                gameOver = true;
                yield break;
            }

            yield return null;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }
        selfHP = enemyMove.hpChange;

        if (selfHP != 0)
        {
            enemy.hp += selfHP;
            battleText.text = enemy.name + " restores " + selfHP + " health!";
            yield return null;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }


        foreach (AnimalChangeEffect animalChange in enemyMove.animalChanges)
        {
            foreach (Animal animal in animalChange.animals)
            {
                int difference = (Mathf.RoundToInt(worldManager.animalAmounts[animal] * animalChange.rate) + animalChange.change - worldManager.animalAmounts[animal]);
                worldManager.animalAmounts[animal] += difference;
                if (difference > 0)
                {
                    battleText.text = animal.name + " population is increased by " + difference + ".";
                }
                else if (difference < 0)
                {
                    battleText.text = animal.name + " population is decreased by " + Mathf.Abs(difference) + ".";
                }
                else
                {
                    continue;
                }
                yield return null;
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }
        }
        if (turnsUntilEnvironmentChange <= 0)
        {
            
            battleText.text = "The woods shift... time passes... creatures grow... and die...";
            worldManager.ProcessEnvironmentChange();
            yield return null;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            turnsUntilEnvironmentChange = UnityEngine.Random.Range(2, 5);
        }
        else
        {
            turnsUntilEnvironmentChange--;
        }
        

        commandMenu.gameObject.SetActive(true);


        yield return null;
    }

}

