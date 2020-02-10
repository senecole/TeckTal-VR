using UnityEngine;
using Weelco.VRKeyboard;
using Weelco.VRAuthorization;

namespace Weelco {

    public class VRRegistrationDemo : MonoBehaviour {

        public VRAuthSettings authSettings;
        public VRKeyboardFull keyboard;

        public float emergenceSpeed = 75.0f;        
               
        private float startTime;
        private float journeyLength;
        private Vector3 regPosition;

        private enum KeyboardState {
            Hide, Process, Shown
        }
        private KeyboardState keyboardState;
        
        void Start() {            
            if (authSettings) {
                regPosition = authSettings.transform.localPosition;
                authSettings.transform.localPosition = Vector3.zero;
                journeyLength = Vector3.Distance(regPosition, Vector3.zero);
            }

            if (keyboard) {
                keyboard.OnVRKeyboardBtnClick += onKeyboardBtnClick;
                keyboard.Init();
                keyboard.gameObject.SetActive(false);
            }

            keyboardState = KeyboardState.Hide;
        }

        void Update() {            
            if (keyboardState.Equals(KeyboardState.Process)) {
                if (Mathf.Round(authSettings.transform.localPosition.y) < regPosition.y) {
                    float distCovered = (Time.time - startTime) * emergenceSpeed;
                    float fracJourney = distCovered / journeyLength;
                    authSettings.transform.localPosition = Vector3.Lerp(authSettings.transform.localPosition, Vector3.up * regPosition.y, fracJourney);                    
                }
                else {
                    keyboard.Init();
                    keyboard.gameObject.SetActive(true);
                    authSettings.transform.localPosition = regPosition;
                    keyboardState = KeyboardState.Shown;
                }
            }
        }
        
        void OnDestroy() {
            if (keyboard) {
                keyboard.OnVRKeyboardBtnClick -= onKeyboardBtnClick;
            }            
        }

        private void onKeyboardBtnClick(string value) {
            authSettings.HandleInput(value);
        }

        public void LoginHandler() {
            string mail = authSettings.GetEmailField().text;
            string pass = authSettings.GetPasswordField().text;
            Debug.Log("Login invoke!\nmail:" + mail + "\npass:" + pass);
        }

        public void RegistrationHandler() {
            string mail = authSettings.GetEmailField().text;
            string pass = authSettings.GetPasswordField().text;
            Debug.Log("Registration invoke!\nmail:" + mail + "\npass:" + pass);
        }

        public void SelectHandler() {
            startTime = Time.time;            
            if (keyboardState.Equals(KeyboardState.Hide)) {
                keyboardState = KeyboardState.Process;
            }
        }      
    }
}
