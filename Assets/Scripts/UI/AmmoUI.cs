using UnityEngine;

public class AmmoUIObjects : MonoBehaviour
{
    public PlayerGun gun; // Arrastra aquí tu script Gun
    public GameObject[] bulletObjects; // Asigna aquí los GameObjects que representan las balas

    void Update()
    {
        if (gun == null) return;

        for (int i = 0; i < bulletObjects.Length; i++)
        {
            // Activa los objetos según la munición actual
            bulletObjects[i].SetActive(i < gun.CurrentAmmo);
        }
    }
}