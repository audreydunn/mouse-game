using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheesePerRoomTracker : MonoBehaviour
{

    private Dictionary<string, int> cheesesCollected = new Dictionary<string, int>();
    private Dictionary<string, int> totalCheesesInRoom = new Dictionary<string, int>();

    public TextMeshProUGUI countText;

    private string currentRoom;

    void Start()
    {
        currentRoom = "Kitchen";
        totalCheesesInRoom["Kitchen"] = 21;
        totalCheesesInRoom["CratesRoom"] = 3;
        totalCheesesInRoom["HauntedRoom"] = 7;

        if (!cheesesCollected.ContainsKey(currentRoom))
        {
            cheesesCollected[currentRoom] = 0;
        }

        UpdateDisplay();
    }

    public void CollectCheese()
    {
        cheesesCollected[currentRoom]++;
        UpdateDisplay();
    }

    public void SwitchRoom(string newRoom)
    {
        currentRoom = newRoom;

        if (!cheesesCollected.ContainsKey(currentRoom))
        {
            cheesesCollected[currentRoom] = 0;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int collected = cheesesCollected[currentRoom];
        int total = totalCheesesInRoom[currentRoom];

        countText.text = $"{collected} / {total}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoomEntrance"))
        {
            string roomName = other.gameObject.name;
            SwitchRoom(roomName);
        }
    }
}
