using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Certifique-se de incluir este namespace para usar TextMeshPro

public class TaskManager : MonoBehaviour
{
    public List<Quest> quests;
    public MoneyManager moneyManager;
    public Canvas questCanvas;
    public GameObject questUIPrefab;
    public GameObject spawnTask;
    public GameObject spawnTask2;
    public GameObject spawnTask3;

    void Start()
    {
        // Primeira Missão: já existente
        Quest simpleQuest = new Quest
        {
            title = "Primeira Missão",
            description = "Colete 5 recursos para completar esta missão.",
            isCompleted = false,
            objectives = new List<Objective>
        {
            new Objective
            {
                description = "Coletar 5 recursos",
                isCompleted = false,
                type = ObjectiveType.Collect,
                targetAmount = 5,
                currentAmount = 0
            }
        },
            reward = new Reward(moneyAmountFun: 100, ecologicoValorFun: 10, economicoValorFun: 5, socialValorFun: 8)
        };
        AddQuest(simpleQuest);

        // Nova Missão 1: Colher plantas medicinais
        Quest harvestMedicinalPlants = new Quest
        {
            title = "Colheita Medicinal",
            description = "Colete 10 plantas medicinais para curar os aldeões.",
            isCompleted = false,
            objectives = new List<Objective>
        {
            new Objective
            {
                description = "Coletar 10 plantas medicinais",
                isCompleted = false,
                type = ObjectiveType.Collect,
                targetAmount = 10,
                currentAmount = 0
            }
        },
            reward = new Reward(moneyAmountFun: 50, ecologicoValorFun: 8, economicoValorFun: 5, socialValorFun: 12)
        };
        AddQuest(harvestMedicinalPlants);

        // Nova Missão 2: Colher ervas aromáticas
        Quest harvestAromaticHerbs = new Quest
        {
            title = "Colheita Aromática",
            description = "Colete 15 ervas aromáticas para vender no mercado.",
            isCompleted = false,
            objectives = new List<Objective>
        {
            new Objective
            {
                description = "Coletar 15 ervas aromáticas",
                isCompleted = false,
                type = ObjectiveType.Collect,
                targetAmount = 15,
                currentAmount = 0
            }
        },
            reward = new Reward(moneyAmountFun: 70, ecologicoValorFun: 5, economicoValorFun: 10, socialValorFun: 10)
        };
        AddQuest(harvestAromaticHerbs);

        // Nova Missão 3: Colher frutas silvestres
        Quest harvestWildFruits = new Quest
        {
            title = "Colheita de Frutas Silvestres",
            description = "Colete 20 frutas silvestres para o festival da vila.",
            isCompleted = false,
            objectives = new List<Objective>
        {
            new Objective
            {
                description = "Coletar 20 frutas silvestres",
                isCompleted = false,
                type = ObjectiveType.Collect,
                targetAmount = 20,
                currentAmount = 0
            }
        },
            reward = new Reward(moneyAmountFun: 90, ecologicoValorFun: 12, economicoValorFun: 7, socialValorFun: 15)
        };
        AddQuest(harvestWildFruits);

        // Nova Missão 4: Arar a terra
        Quest plowField = new Quest
        {
            title = "Preparação do Solo",
            description = "Arar 5 áreas de terra para plantio.",
            isCompleted = false,
            objectives = new List<Objective>
        {
            new Objective
            {
                description = "Arar 5 áreas de terra",
                isCompleted = false,
                type = ObjectiveType.Plow,
                targetAmount = 5,
                currentAmount = 0
            }
        },
            reward = new Reward(moneyAmountFun: 60, ecologicoValorFun: 10, economicoValorFun: 6, socialValorFun: 5)
        };
        AddQuest(plowField);

        // Nova Missão 5: Colher flores silvestres
        Quest harvestWildflowers = new Quest
        {
            title = "Flores Silvestres",
            description = "Colete 8 flores silvestres para o festival de primavera.",
            isCompleted = false,
            objectives = new List<Objective>
        {
            new Objective
            {
                description = "Coletar 8 flores silvestres",
                isCompleted = false,
                type = ObjectiveType.Collect,
                targetAmount = 8,
                currentAmount = 0
            }
        },
            reward = new Reward(moneyAmountFun: 40, ecologicoValorFun: 7, economicoValorFun: 3, socialValorFun: 10)
        };
        AddQuest(harvestWildflowers);
    }
    public void ShowQuestCanvas()
    {
        questCanvas.gameObject.SetActive(true); // Ativa o Canvas

        // Lista de spawns disponíveis
        List<GameObject> spawnPoints = new List<GameObject> { spawnTask, spawnTask2, spawnTask3 };

        // Limita a quantidade de missões ao número de spawns
        int questsToShow = Mathf.Min(quests.Count, spawnPoints.Count);

        // Embaralha as missões para garantir a aleatoriedade
        List<Quest> shuffledQuests = new List<Quest>(quests);
        System.Random rnd = new System.Random();
        shuffledQuests.Sort((a, b) => rnd.Next(-1, 2));

        // Atribui missões aleatórias para cada spawn
        for (int i = 0; i < questsToShow; i++)
        {
            Quest randomQuest = shuffledQuests[i];
            GameObject questUIInstance = Instantiate(questUIPrefab, spawnPoints[i].transform);

            // Atribui as informações da missão ao prefab de UI
            TextMeshProUGUI questTitleText = questUIInstance.transform.Find("EventName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI questDescriText = questUIInstance.transform.Find("EventDescription").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI questRewardText = questUIInstance.transform.Find("EventReward").GetComponent<TextMeshProUGUI>();

            // Define o texto da missão
            questTitleText.text = randomQuest.title;
            questDescriText.text = randomQuest.description;
            questRewardText.text = $"Recompensa: {randomQuest.reward.moneyAmount} moedas, " +
                                   $"Ecológico: {randomQuest.reward.ecologicoValor}, " +
                                   $"Econômico: {randomQuest.reward.economicoValor}, " +
                                   $"Social: {randomQuest.reward.socialValor}";
        }
    }
    public void HideQuestCanvas()
    {
        questCanvas.gameObject.SetActive(false); // Desativa o Canvas
    }

    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
    }

    public void UpdateQuestProgress(Objective objective)
    {
        // Atualize o progresso de objetivos, se necessário
    }

    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;

        // Concede a recompensa ao jogador
        if (quest.reward != null && moneyManager != null)
        {
            quest.reward.GiveReward(moneyManager);
        }
    }
}
