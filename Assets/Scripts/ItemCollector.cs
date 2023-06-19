using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] Text pointsText;

    private void Update()
    {
        pointsText.text = "Points: " + PlayerLife.points;
    }
}

