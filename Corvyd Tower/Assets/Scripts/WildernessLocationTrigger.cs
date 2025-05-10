using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WildernessLocationTrigger : MonoBehaviour
{
   [SerializeField] private GMStateMachine _gameManager;
   [SerializeField] private TextMeshProUGUI _wildernessText;

   public void Awake()
   {
      _wildernessText.GameObject().SetActive(false);
   }
   public void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         _gameManager.atKeep = false;
         AkSoundEngine.SetState("Location", "Wilderness");
         _wildernessText.GameObject().SetActive(true);
      }
   }
}
