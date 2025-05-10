using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KeepLocationTrigger : MonoBehaviour
{
   [SerializeField] private GMStateMachine _gameManager;
   //[SerializeField] private TextMeshProUGUI _wildernessText;
   public void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         _gameManager.atKeep = true;
         AkSoundEngine.SetState("Location", "Keep");
         //_wildernessText.GameObject().SetActive(false);
      }
   }
}
