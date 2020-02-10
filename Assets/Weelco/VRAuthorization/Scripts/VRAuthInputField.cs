using UnityEngine;
using UnityEngine.UI;
using Weelco.VRKeyboard;

namespace Weelco.VRAuthorization {

    public class VRAuthInputField : MonoBehaviour {

        public delegate void InputFieldSelect(VRAuthInputField clicked);
        public InputFieldSelect OnInputFieldSelect;  

        private Text label;
        private Button button;
        
        private VRAuthInputBase input;
        
        public string text { get; private set; }

        private bool isSecure = false;
        private bool isSelect = false;

        private string cachedText;        
        
        void Start() {
            input = transform.Find("Base").GetComponent<VRAuthInputBase>();
            label = transform.Find("Text").GetComponent<Text>();
            cachedText = label.text;
            text = string.Empty;

            button = transform.GetComponent<Button>();
            if (!button) {
                button = gameObject.AddComponent<Button>();
                button.transition = Selectable.Transition.None;
            }
            button.onClick.AddListener(HandleClick);

            input.Init();
        }

        void OnDestroy() {
            button.onClick.RemoveListener(HandleClick);
        }

        public void Select() {
            isSelect = true;
            button.interactable = false;
            input.SetFocusedState();
            if (label.text.Equals(cachedText)) {
                label.text = string.Empty;
            }
        }

        public void Unselect() {
            isSelect = false;
            button.interactable = true;
            if (input.IsFocusedState) {
                input.SetDefaultState();
            }
            if (label.text.Equals(string.Empty)) {
                label.text = cachedText;
            }
        }

        public void SetErrorState(string errorCode) {
            input.SetErrorState(errorCode);
        }

        public void SetSuccessState() {
            input.SetSuccessState();
        }

        public void HandleInput(string value) {
            if (input.IsErrorState) {
                if (isSelect) {
                    input.SetFocusedState();
                } else {
                    input.SetDefaultState();
                }
            }
            if (!value.Equals(VRKeyboardBase.ENTER)) {                
                if (value.Equals(VRKeyboardBase.BACK)) {
                    BackspaceKey();
                }
                else {
                    TypeKey(value);
                }

                FormatLabel();
            }
        }

        private void BackspaceKey() {
            if (label) {
                if (text.Length >= 1)
                    text = text.Remove(text.Length - 1, 1);
            }
        }

        private void TypeKey(string value) {
            char[] letters = value.ToCharArray();
            for (int i = 0; i < letters.Length; i++) {
                TypeKey(letters[i]);
            }
        }

        private void TypeKey(char key) {
            if (label) {
                text += key.ToString();
            }
        }

        private void FormatLabel() {
            if (!label.text.Equals(cachedText)) { 

                string formattedText = text;

                if (isSecure) {
                    var count = formattedText.Length;
                    formattedText = string.Empty;
                    while (count > 0) {
                        formattedText += "*";
                        count--;
                    }
                }

                label.text = formattedText;
            }
        }

        private void HandleClick() {
            if (OnInputFieldSelect != null)
                OnInputFieldSelect(this);
        }
        
        public bool IsSecure {
            set { isSecure = value; }
        }
    }
}