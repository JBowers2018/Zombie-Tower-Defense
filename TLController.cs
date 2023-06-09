using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TLController : MonoBehaviour
{
    private bool hasTower = false;
    private GameObject tower = null;
    private int price, basePrice, upgradePrice, baseUpgradePrice;
    private string colorNtype = "";

    public void SetUp()
    {
        if(tower)
        {
            switch (tower.name)
            {
                case "Red Tower(Clone)": colorNtype = "redField"; basePrice = 150; price = basePrice;
                    baseUpgradePrice = 225; upgradePrice = baseUpgradePrice; break;
                case "Blue Tower(Clone)": colorNtype = "blueField"; basePrice = 100; price = basePrice;
                    baseUpgradePrice = 150; upgradePrice = baseUpgradePrice; break;
                case "Green Tower(Clone)": colorNtype = "greenField"; basePrice = 50; price = basePrice;
                    baseUpgradePrice = 75; upgradePrice = baseUpgradePrice; break;
                default: break;
            }
        }
        else
        {
            switch (name)
            {
                case "Red Tower": colorNtype = "redInventory"; basePrice = 150; price = basePrice;
                    baseUpgradePrice = 225;  upgradePrice = baseUpgradePrice; break;
                case "Blue Tower": colorNtype = "blueInventory"; basePrice = 100; price = basePrice;
                    baseUpgradePrice = 150; upgradePrice = baseUpgradePrice; break;
                case "Green Tower": colorNtype = "greenInventory"; basePrice = 50; price = basePrice;
                    baseUpgradePrice = 75; upgradePrice = baseUpgradePrice; break;
                case "Red Tower(Clone)":
                    colorNtype = "redField"; basePrice = 150; price = basePrice;
                    baseUpgradePrice = 225; upgradePrice = baseUpgradePrice; break;
                case "Blue Tower(Clone)":
                    colorNtype = "blueField"; basePrice = 100; price = basePrice;
                    baseUpgradePrice = 150; upgradePrice = baseUpgradePrice; break;
                case "Green Tower(Clone)":
                    colorNtype = "greenField"; basePrice = 50; price = basePrice;
                    baseUpgradePrice = 75; upgradePrice = baseUpgradePrice; break;
                default: break;
            }
        }   
    }

    public void Clear()
    {
        if(tower)
        {
            Destroy(tower);
            hasTower = false;
        }
    }

    public void SetHasTower(bool tf)
    { hasTower = tf; }

    public void SetHasTower(bool tf, GameObject t)
    { hasTower = tf;  tower = t; SetUp(); }

    public bool GetHasTower()
    { return hasTower; }

    public GameObject GetTower()
    { return tower; }

    public void SetPrice(int p)
    { price = p; }

    public void SetPrices()
    {
        price = basePrice + 25 * (GetComponent<TowerController>().GetLevel() - 1);
        upgradePrice = baseUpgradePrice + 50 * (GetComponent<TowerController>().GetLevel() - 1);
    }

    public int GetPrice()
    { return price; }

    public string GetColorNType()
    { return colorNtype; }

    public void SetUpgradePrice(int up)
    { upgradePrice = up; }

    public int GetUpgradePrice()
    { return upgradePrice; }
}
