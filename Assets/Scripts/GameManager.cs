using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour{

    public static GameManager instance = null;

	public enum STATE {BATTLE, EXPLORATION} // possiveis estados do jogo

	public static List<GameObject> players = new List<GameObject>();
	public static List<GameObject> enemies = new List<GameObject>();
    
	public static int potions = 0;
	public static bool isPotionSelected = false;

	public static GameObject prefabWarrior;
	public static GameObject prefabMage;
	public static GameObject prefabArcher;
	public static GameObject prefabOgre;

    public GameObject prefabWarriorReference;
    public GameObject prefabMageReference;
    public GameObject prefabArcherReference;
    public GameObject prefabOgreReference;

    public static STATE state = STATE.EXPLORATION;

    private static SaveFile saveFile;
    private static string saveFileLocation;

    private void Awake() {
        if (instance == null) {
            instance = this;
            prefabArcher = prefabArcherReference;
            prefabMage = prefabMageReference;
            prefabWarrior = prefabWarriorReference;
            prefabOgre = prefabOgreReference;
#if !UNITY_WEBGL
            saveFileLocation = Path.Combine(Application.persistentDataPath, "SaveFiles");
            DirectoryInfo dirInfo = new DirectoryInfo(saveFileLocation);
            if (!dirInfo.Exists)
                dirInfo.Create();
            saveFileLocation = Path.Combine(saveFileLocation, "savefile.data");
            if (File.Exists(saveFileLocation)) {
                FileStream fs = new FileStream(saveFileLocation, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                saveFile = (SaveFile)bf.Deserialize(fs);
                fs.Close();
            } else
                saveFile = new SaveFile();
#endif
        } else if (instance != this)
            Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    public static void OnClickPotion(){
		if (potions > 0) {
			if (!isPotionSelected)
				isPotionSelected = true;
			else
				isPotionSelected = false;
		}
	}

	// Instancia um warrior, com DontDestroyOnLoad
	public static void InstantiateWarrior(float x, float y, float z){
		GameObject warrior = GameObject.Instantiate (prefabWarrior, new Vector3 (x, y, z), Quaternion.identity);
		warrior.name = "Warrior";
		players.Add (warrior);
		GameObject.DontDestroyOnLoad (warrior);
	}

	// Instancia um mage, com DontDestroyOnLoad
	public static void InstantiateMage(float x, float y, float z){
		GameObject mage = GameObject.Instantiate (prefabMage, new Vector3 (x, y, z), Quaternion.identity);
		mage.name = "Mage";
		players.Add (mage);
        GameObject.DontDestroyOnLoad (mage);
	}

	// Instancia um archer, com DontDestroyOnLoad
	public static void InstantiateArcher(float x, float y, float z){
		GameObject archer = GameObject.Instantiate (prefabArcher, new Vector3 (x, y, z), Quaternion.identity);
		archer.name = "Archer";
		players.Add (archer);
        GameObject.DontDestroyOnLoad (archer);
	}

	public static void InstantiateOgre(float x, float y, float z, int attackType){
		GameObject ogre = GameObject.Instantiate (prefabOgre, new Vector3 (x, y, z), Quaternion.identity);
		ogre.name = "Ogre";
		ogre.GetComponent<EnemyBehaviour> ().AttackType = attackType;
		enemies.Add (ogre);
	}

	public static void RewardPotion(int n){
		potions += n;
		GameObject potionTxt = GameObject.Find ("TxtPotion");
		potionTxt.GetComponent<Text> ().text = "" + potions + "x";
	}

    public static void SaveGame() {
        saveFile.Reset();
        foreach (GameObject obj in players) {
            PlayerBehaviour player = obj.GetComponent<PlayerBehaviour>();
            switch (player.Name) {
                case "Guerreiro":
                    saveFile.warriorHP = player.Life;
                    saveFile.warriorSpecial = player.Special;
                    break;
                case "Mago":
                    saveFile.mageHP = player.Life;
                    saveFile.mageSpecial = player.Special;
                    break;
                case "Arqueiro":
                    saveFile.archerHP = player.Life;
                    saveFile.archerSpecial = player.Special;
                    break;
            }
        }
        saveFile.Exists = true; // The property setter is actually an update function, any value will do
        saveFile.nPotions = potions;
        FileStream fs = new FileStream(saveFileLocation, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, saveFile);
        fs.Close();
    }

    public static void LoadGame() {
        if(saveFile.warriorHP > -1) {
            GameObject warrior = GameObject.Instantiate(prefabWarrior, new Vector3(), Quaternion.identity);
            warrior.name = "Warrior";
            players.Add(warrior);
            GameObject.DontDestroyOnLoad(warrior);
            PlayerBehaviour behaviour = warrior.GetComponent<PlayerBehaviour>();
            behaviour.Life = saveFile.warriorHP;
            behaviour.Special = saveFile.warriorSpecial;
        }
        if (saveFile.archerHP > -1) {
            GameObject archer = GameObject.Instantiate(prefabArcher, new Vector3(), Quaternion.identity);
            archer.name = "Archer";
            players.Add(archer);
            GameObject.DontDestroyOnLoad(archer);
            PlayerBehaviour behaviour = archer.GetComponent<PlayerBehaviour>();
            behaviour.Life = saveFile.archerHP;
            behaviour.Special = saveFile.archerSpecial;
        }
        if(saveFile.mageHP > -1) {
            GameObject mage = GameObject.Instantiate(prefabMage, new Vector3(), Quaternion.identity);
            mage.name = "Mage";
            players.Add(mage);
            GameObject.DontDestroyOnLoad(mage);
            PlayerBehaviour behaviour = mage.GetComponent<PlayerBehaviour>();
            behaviour.Life = saveFile.mageHP;
            behaviour.Special = saveFile.mageSpecial;
        }
        potions = saveFile.nPotions;
        UnityEngine.SceneManagement.SceneManager.LoadScene("ExplorationScene");
    }

    public static bool hasSaveFile() {
#if UNITY_WEBGL
        return false;
#else
        return saveFile.Exists;
#endif
    }
}