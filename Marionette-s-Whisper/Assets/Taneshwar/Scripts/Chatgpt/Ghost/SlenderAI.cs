using UnityEngine;
using System.Collections;
public class SlenderAI : MonoBehaviour
{
    public float maxSoundDistance = 15f;
    public float maxLookDuration = 5f;
    public GameObject player;
    public AudioClip[] teleportSoundClips;
    public int slenderIntensity = 0; // Public integer variable to control Slender Man's occurrence

    [System.Serializable]
    public struct OccurrenceTimePeriod
    {
        public float minOccurrenceTime;
        public float maxOccurrenceTime;
    }

    public OccurrenceTimePeriod[] intensityTimePeriods; // Array to define occurrence time periods for different intensity levels

    private AudioSource teleportSound;
    private bool isTeleporting = false;
    private bool isLookingAtSlender = false;
    private float initialVolume;
    private int lastPlayedSoundIndex = -1;
    private float lookTimer = 0f;
    private float currentTeleportDelay = 0f;

    public float initialTeleportDelay = 5f; // Initial teleport delay for default intensity

    private void Start()
    {
        teleportSound = gameObject.AddComponent<AudioSource>();
        teleportSound.loop = true;

        if (player == null)
        {
            Debug.LogError("SlenderAI: Player GameObject not assigned!");
        }

        if (teleportSoundClips == null || teleportSoundClips.Length == 0)
        {
            Debug.LogError("SlenderAI: Teleport Sound AudioClips not assigned!");
        }

        initialVolume = teleportSound.volume;
        currentTeleportDelay = GetRandomTeleportDelayForIntensity(slenderIntensity);
    }

    private void Update()
    {
        // Check if the player is looking at the Slender Man
        isLookingAtSlender = IsPlayerLookingAtSlender();

        // Calculate the fade-out volume based on the player's distance from the Slender Man
        float fadeOutVolume = Mathf.Clamp01(1f - (Vector3.Distance(transform.position, player.transform.position) / maxSoundDistance));

        // Play or fade out the teleport sound based on whether the player is looking at the Slender Man and within the maxSoundDistance
        if (isLookingAtSlender && !teleportSound.isPlaying && IsPlayerWithinMaxSoundDistance())
        {
            PlayRandomTeleportSound();
        }
        else if ((!isLookingAtSlender || !IsPlayerWithinMaxSoundDistance()) && teleportSound.isPlaying)
        {
            teleportSound.volume = fadeOutVolume * initialVolume;
            if (fadeOutVolume <= 0.05f) // If volume is very low, stop the sound
            {
                teleportSound.Stop();
            }
        }

        // Check if the player is looking away from the Slender Man
        if (!isLookingAtSlender && !isTeleporting)
        {
            isTeleporting = true;
            lookTimer = 0f; // Reset the look timer when the player looks away
            StartCoroutine(TeleportAndReset());
        }

        // Check if the player is continuously looking at the Slender Man for more than maxLookDuration seconds
        if (isLookingAtSlender && !isTeleporting)
        {
            lookTimer += Time.deltaTime;
            if (lookTimer >= maxLookDuration)
            {
                PlayerDied();
            }
        }
    }

    private bool IsPlayerLookingAtSlender()
    {
        // Get the direction from the camera to the Slender Man
        Vector3 directionToSlender = transform.position - Camera.main.transform.position;

        // Calculate the angle between the camera's forward direction and the direction to the Slender Man
        float angleToSlender = Vector3.Angle(Camera.main.transform.forward, directionToSlender);

        // If the angle is within a certain threshold, the player is looking at the Slender Man
        return angleToSlender < 30f; // Adjust the threshold to your preference
    }

    private bool IsPlayerWithinMaxSoundDistance()
    {
        // Check if the distance between the player and the Slender Man is within the maxSoundDistance
        return Vector3.Distance(transform.position, player.transform.position) <= maxSoundDistance;
    }

    private void PlayRandomTeleportSound()
    {
        if (teleportSoundClips.Length == 0)
            return;

        int randomIndex = GetRandomSoundIndex();
        if (randomIndex == -1)
            return;

        // Play the selected teleport sound clip
        teleportSound.clip = teleportSoundClips[randomIndex];
        teleportSound.volume = initialVolume; // Reset volume to its initial value
        teleportSound.Play();

        // Start a coroutine to wait before allowing the sound to play again
        StartCoroutine(DelayNextTeleportSound());

        // Adjust the teleport delay based on the value of the slenderIntensity variable
        currentTeleportDelay = GetRandomTeleportDelayForIntensity(slenderIntensity);
    }

    private int GetRandomSoundIndex()
    {
        if (teleportSoundClips.Length == 1)
            return 0;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, teleportSoundClips.Length);
        } while (randomIndex == lastPlayedSoundIndex);

        lastPlayedSoundIndex = randomIndex;
        return randomIndex;
    }

    private System.Collections.IEnumerator DelayNextTeleportSound()
    {
        yield return new WaitForSeconds(teleportSound.clip.length);
        lastPlayedSoundIndex = -1; // Allow the same sound to be played again after the delay
    }

    private IEnumerator TeleportAndReset()
    {
        // Teleport the Slender Man to a new random position within the teleport distance
        Vector3 teleportPosition = player.transform.position + Random.insideUnitSphere.normalized * maxSoundDistance;
        transform.position = new Vector3(teleportPosition.x, player.transform.position.y, teleportPosition.z);

        // Wait for a short delay before re-enabling teleportation
        yield return new WaitForSeconds(currentTeleportDelay);

        isTeleporting = false;
    }

    private float GetRandomTeleportDelayForIntensity(int intensityLevel)
    {
        if (intensityLevel >= 0 && intensityLevel < intensityTimePeriods.Length)
        {
            float minOccurrenceTime = intensityTimePeriods[intensityLevel].minOccurrenceTime;
            float maxOccurrenceTime = intensityTimePeriods[intensityLevel].maxOccurrenceTime;
            return Random.Range(minOccurrenceTime, maxOccurrenceTime);
        }
        else
        {
            Debug.LogWarning("SlenderAI: Intensity level out of range. Using default teleport delay.");
            return initialTeleportDelay;
        }
    }

    // Placeholder method for handling player death
    private void PlayerDied()
    {
        // You can add the logic for player death here.
        // For example, show a game over screen, restart the level, etc.
        Debug.Log("Player Died!");
    }
}
