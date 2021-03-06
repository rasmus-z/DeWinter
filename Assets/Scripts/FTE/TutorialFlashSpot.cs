using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace Ambition
{
    public class TutorialFlashSpot : MonoBehaviour
    {
        private const float FLASH_TIME = 1f;
        private const int FLASH_REPEAT = 3;
        private const float WAIT_TIME = 2f;
     
        private bool _paused=false;
        private SpotlightView _script;

        void OnEnable()
        {
            _script = GetComponent<SpotlightView>();
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleGuest);
            StopAllCoroutines();
            StartCoroutine(FlashCR());
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleGuest);
            StopAllCoroutines();
            _script.On = false;
        }
        
        private void HandleGuest(GuestVO guest)
        {
            _paused = guest != null && _script.Guest == guest;
        }

        IEnumerator FlashCR()
        {
            float time = 0;
            float period = FLASH_TIME/(FLASH_REPEAT + FLASH_REPEAT - 1);
            while (true)
            {
                _script.On = _paused || (time < FLASH_TIME && ((int)(time/period))%2 == 0);
                if (!_paused)
                {
                    time += Time.deltaTime;
                    if (time > FLASH_TIME + WAIT_TIME) time = 0f;
                }
                yield return null;
            }
        }
    }
}
