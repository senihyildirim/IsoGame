using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class GateTrigger : MonoBehaviour
{
    [Header("Gate Parts")]
    public Transform gatePart1; // Part of the gate that will move up
    public Transform gatePart2; // Part of the gate that will move down

    [Header("Gate Movement Settings")]
    public float moveDistance = 5f; // Distance to move the gate parts
    public float moveDuration = 2f; // Duration of the animation

    [Header("Trigger Settings")]
    public bool isGateOpen = false; // Check if the gate is open or not

    private Vector3 gatePart1StartPos;
    private Vector3 gatePart2StartPos;

    private void Start()
    {
        // Save the initial positions of both gate parts
        gatePart1StartPos = gatePart1.position;
        gatePart2StartPos = gatePart2.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object triggering the gate is the player (or another object you specify)
        if (other.CompareTag("Player") && !isGateOpen)
        {
            OpenGate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Close the gate when the player exits the trigger area
        if (other.CompareTag("Player") && isGateOpen)
        {
            CloseGate();
        }
    }

    private void OpenGate()
    {
        isGateOpen = true;

        // Move gatePart1 upwards
        gatePart1.DOMoveY(gatePart1StartPos.y + moveDistance, moveDuration);

        // Move gatePart2 downwards
        gatePart2.DOMoveY(gatePart2StartPos.y - moveDistance, moveDuration);
    }

    private void CloseGate()
    {
        isGateOpen = false;

        // Move gatePart1 back to its original position
        gatePart1.DOMoveY(gatePart1StartPos.y, moveDuration);

        // Move gatePart2 back to its original position
        gatePart2.DOMoveY(gatePart2StartPos.y, moveDuration);
    }
}
