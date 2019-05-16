using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct ArmorSet
{
    public string Name;
    public Sprite HeadSprite;
    public Sprite BodySprite;
    public Sprite LeftArm;
    public Sprite RightArm;
    public Sprite LeftLeg;
    public Sprite RightLeg;
}

public class FightSceneScript : MonoBehaviour
{
    public GameObject _playerObject;
    public GameObject _enemyObject;
    public List<GameObject> _enemies;
    public List<Sprite> _weapons;
    public List<ArmorSet> _armorSets;

    private PersistentScript _persistent;

	void Start ()
    {
        _persistent = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>();
    }
	
	void Update ()
    {
	
	}

    public void SetupFight(PlayerData player, Enemy enemy)
    {
        GameObject newPlayer = Instantiate(_playerObject) as GameObject;
        newPlayer.transform.position = new Vector3(-5f, 0f);
        newPlayer.transform.GetChild(0).GetComponent<PlayerScript>()._player = player;

        Sprite sprite = GetWeaponSprite(player.Weapon.Name);
        if(sprite != null)
        {
            newPlayer.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = sprite;
        }

        GameObject newEnemy = Instantiate(GetObject(enemy.PrefabName)) as GameObject;
        newEnemy.transform.position = new Vector3(5f, 0f);
        newEnemy.transform.GetChild(0).GetComponent<EnemyScript>()._enemy = enemy;
    }

    GameObject GetObject(string name)
    {
        foreach(GameObject obj in _enemies)
        {
            if(obj.name == name)
            {
                return obj;
            }
        }
        return null;
    }

    Sprite GetWeaponSprite(string name)
    {
        foreach(Sprite sprite in _weapons)
        {
            if(sprite.name == name)
            {
                return sprite;
            }
        }
        return null;
    }

    public void EntityDeath(string name)
    {
        switch(name)
        {
            case "Player":
                GameObject.FindGameObjectWithTag("Persistent").GetComponent<ChangeScene>().OnClick(0);
                _persistent._player.Health = _persistent._player.MaxHealth;
                break;
            case "Enemy":
                GameObject.FindGameObjectWithTag("Persistent").GetComponent<ChangeScene>().OnClick(0);
                break;
        }
    }
}
