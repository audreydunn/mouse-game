using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrackCatDistances : MonoBehaviour
{

    public List<GameObject> aiAgents; // List of AI agents to track
    public GameObject player;
    public float displayDistanceRange = 25f; // Range within which to display distance
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        System.Text.StringBuilder displayText = new System.Text.StringBuilder();

        foreach (GameObject aiAgent in aiAgents)
        {
            float distanceToPlayer = Vector3.Distance(aiAgent.transform.position, player.transform.position);

            // Check if the distance is within the specified range
            if (distanceToPlayer <= displayDistanceRange)
            {
                displayText.AppendLine($"distance to {aiAgent.name} is: {distanceToPlayer:F2} meters");
            }
        }
        textMeshPro.text = displayText.ToString();
    }
}
