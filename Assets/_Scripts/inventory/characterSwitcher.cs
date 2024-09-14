using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSwitcher : MonoBehaviour
{
    public GameObject[] characters; // Array to store character prefabs
    private int currentCharacterIndex = 0; // Index of the currently active character
    public GameObject characterFollowTarget;

    private void Start()
    {
        // Activate the initial character
        ActivateCharacter(currentCharacterIndex);
    }

    private void Update()
    {
        // Check for player input to switch characters (e.g., a button press)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Deactivate the current character
            DeactivateCharacter(currentCharacterIndex);

            // Switch to the next character
            currentCharacterIndex = (currentCharacterIndex + 1) % characters.Length;

            // Activate the new character
            ActivateCharacter(currentCharacterIndex);
        }
    }

    private void ActivateCharacter(int index)
    {
        characters[index].SetActive(true);
        characterFollowTarget.transform.position = characters[index].transform.position;
    }

    private void DeactivateCharacter(int index)
    {
        characters[index].SetActive(false);
        // You can also perform other character-specific cleanup here
    }
}
