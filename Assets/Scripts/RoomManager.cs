/* Universidade de Sao Paulo - ICMC - Outubro de 2017
   Autores: Klinsmann Hengles, Felipe Torres e Gabriel Carvalho
   Script desenvolvido para o jogo Infinite Dungeon
   Descrição: Script que gera as salas, seus detalhes e a porta.*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour {

	public Text heroText; // nome do heroi
	private int dieHero; // variavel randomica que determina de que classe sera o heroi encontrado
	private float dieRoom; // variavel randomica que determina se a proxima sala sera de batalha, pocao ou heroi
    private float newPlayerThreshold = 50.0f;
	private Vector2 initialPosition; // posicao inicial do pegar e arrastar
	private Vector2 finalPosition; // posicao final do clicar e arrastar
	private float distanceX; // distancia entre dois pontos no eixo X
	private float distanceY; // distancia entre dois pontos no eixo Y
	private float totalDistanceX; // distancia percorrida total em x
	private float totalDistanceY; // distancia percorrida total em Y

	public List<Sprite> rooms;
	public List<GameObject> heroes;

	public Text potionTxt;

	// Use this for initialization
	void Start () {
		initialPosition = new Vector2 (0f, 0f);
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = rooms[0];
		dieRoom = (int) Random.Range (0f, 2.9f);
        heroes = new List<GameObject>(GameManager.players);
        foreach (GameObject playerObject in heroes) {
            switch (playerObject.GetComponent<PlayerBehaviour>().Name) {
                case "Guerreiro":
                    playerObject.transform.position = new Vector3(0f, 0f, 0f);
                    break;
                case "Arqueiro":
                    playerObject.transform.position = new Vector3(-1f, 0f, 0f);
                    break;
                case "Mago":
                    playerObject.transform.position = new Vector3(1f, 0f, 0f);
                    break;
                default:
                    throw new System.Exception("This class does not exist");
            }
        }
	}

	void OnMouseDrag() {
		if (initialPosition == new Vector2(0f, 0f)) {
			initialPosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		}

		finalPosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
	}

	void OnMouseUp() {
		distanceX = finalPosition.x - initialPosition.x;
		distanceY = finalPosition.y - initialPosition.y;
		totalDistanceX = Mathf.Sqrt (Mathf.Pow (initialPosition.x - finalPosition.x, 2));
		totalDistanceY = Mathf.Sqrt (Mathf.Pow (initialPosition.y - finalPosition.y, 2));

		// Se a distancia percorrida horizontalmente e' maior que a distancia percorrida na vertical
		if (totalDistanceX > totalDistanceY) {
			// Se a distancia em x for positiva o movimento e' para a direita
			if (distanceX > 0) {
				print ("DIREITA");
			} 
			// Se a distancia em x for negativa o movimento e' para a esquerda
			else {
				print ("ESQUERDA");
			}
		}

		// Se a distancia percorrida verticalmente e' maior que a distancia percorrida na horizontal
		else {
			// Se a distancia em y for positiva o movimento e' para cima
			if (distanceY > 0) {
				print ("CIMA");
			} 
			// Se a distancia em y for negativa o movimento e' para baixo
			else {
				print ("BAIXO");
			}
				
		}
			
		dieRoom = Random.Range (0.0f, 100.0f);
        newPlayerThreshold = 60.0f - (25 * Mathf.Pow(1.4f, (GameManager.players.Count)) );

        if (dieRoom < newPlayerThreshold) { // cena de encontrar aventureiro
            GameManager.state = GameManager.STATE.EXPLORATION;
            bool success = true;

            do {
                dieHero = Random.Range(0, 3);
                success = true;
                string heroName = (dieHero == 0)? "Guerreiro" : (dieHero == 1)? "Arqueiro" : "Mago";

                foreach (GameObject player in GameManager.players) {
                    if(heroName == player.GetComponent<PlayerBehaviour>().Name) {
                        success = false;
                        break;
                    }
                }

                if (success) {
                    heroText.text = "Parabéns! Você encontrou um novo aventureiro para a sua equipe da classe " + heroName;
                    switch (heroName) {
                        case "Arqueiro":
                            GameManager.InstantiateArcher(-1f, 0f, 0f);
                            break;
                        case "Mago":
                            GameManager.InstantiateMage(1f, 0f, 0f);
                            break;
                        case "Guerreiro":
                            GameManager.InstantiateWarrior(0f, 0f, 0f);
                            break;
                        default:
                            throw new System.Exception("Essa classe não existe");
                    }
                }
            } while (!success);
#if !UNITY_WEBGL
            GameManager.SaveGame();
#endif
        } else if (dieRoom < 95.0f) { // cena de batalha
            heroText.text = "";
            UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
            GameManager.state = GameManager.STATE.BATTLE;
        } else { // cena de exploração
            heroText.text = "Você ganhou mais uma poção!";
            GameManager.RewardPotion(1);
#if !UNITY_WEBGL
            GameManager.SaveGame();
#endif
        }

		initialPosition = new Vector2 (0f, 0f);
	}

	public void OnClickPotion(){
		GameManager.OnClickPotion ();
	}

}
