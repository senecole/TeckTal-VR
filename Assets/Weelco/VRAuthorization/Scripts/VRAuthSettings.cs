using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Weelco.VRAuthorization {

    public class VRAuthSettings : MonoBehaviour {

        [SerializeField]
        private int passwordMinLength = 8;
        [SerializeField]
        private bool securePassword = true;

        [SerializeField]
        private string emptyEmailMessage = "Email cannot be empty";
        [SerializeField]
        private string invalidEmailMessage = "Email is invalid";
        [SerializeField]
        private string emptyPasswordMessage = "Password cannot be empty";
        [SerializeField]
        private string shortPasswordErrorMessage = "Password too short";


        public UnityEvent LoginEvent;
        public UnityEvent RegisterEvent;
        public UnityEvent SelectEvent;

        private Button loginButton;
        private Button registerButton;

        private VRAuthInputField mailField;
        private VRAuthInputField passField;
        private VRAuthInputField selectedField;

        private VRAuthInputField[] fields;

        public delegate void SelectFieldHandler();
        public SelectFieldHandler OnSelectFieldHandler;

        // Use this for initialization
        void Start() {

            mailField = transform.Find("MailBlock").GetComponent<VRAuthInputField>();
            passField = transform.Find("PasswordBlock").GetComponent<VRAuthInputField>();

            passField.IsSecure = securePassword;

            fields = new VRAuthInputField[] { mailField, passField };

            for (int i = 0; i < fields.Length; i++) {
                fields[i].OnInputFieldSelect += HandleSelect;
            }

            loginButton = transform.Find("LoginButton").GetComponent<Button>();
            registerButton = transform.Find("RegButton").GetComponent<Button>();

            loginButton.onClick.AddListener(onLoginHandler);
            registerButton.onClick.AddListener(onRegisterHandler);
        }

        void OnDestroy() {
            foreach (VRAuthInputField field in fields) {
                field.OnInputFieldSelect -= HandleSelect;
            }

            loginButton.onClick.RemoveAllListeners();
            registerButton.onClick.RemoveAllListeners();
        }

        public void HandleInput(string text) {
            if (selectedField)
                selectedField.HandleInput(text);
        }

        public VRAuthInputField GetEmailField() {
            return mailField;
        }

        public VRAuthInputField GetPasswordField() {
            return passField;
        }

        private bool CheckFields() {
            string mail = mailField.text;
            string pass = passField.text;
            bool isMailEmpty = string.IsNullOrEmpty(mail);
            bool isPassEmpty = string.IsNullOrEmpty(pass);
            bool isMailValid = mail.Contains("@") && mail.Contains(".");
            bool isPassShort = (pass.Length < passwordMinLength);
            if (isMailEmpty) {
                mailField.SetErrorState(emptyEmailMessage);
                return false;
            }
            else if (!isMailValid) {
                mailField.SetErrorState(invalidEmailMessage);
                return false;
            }
            else {
                mailField.SetSuccessState();
                if (selectedField == mailField) {
                    selectedField.Unselect();
                    selectedField = null;
                }
            }

            if (isPassEmpty) {
                passField.SetErrorState(emptyPasswordMessage);
                return false;
            }
            else if (isPassShort) {
                passField.SetErrorState(shortPasswordErrorMessage);
                return false;
            }
            else {
                passField.SetSuccessState();
                if (selectedField == passField) {
                    selectedField.Unselect();
                    selectedField = null;
                }
            }

            return true;
        }

        private void onLoginHandler() {
            if (CheckFields()) {
                if (LoginEvent != null) {
                    LoginEvent.Invoke();
                }
            }
        }

        private void onRegisterHandler() {
            if (CheckFields()) {
                if (RegisterEvent != null) {
                    RegisterEvent.Invoke();
                }
            }
        }

        private void HandleSelect(VRAuthInputField clicked) {
            selectedField = clicked;
            foreach (VRAuthInputField field in fields) {
                if (!field.Equals(selectedField))
                    field.Unselect();
            }
            selectedField.Select();

            if (SelectEvent != null)
                SelectEvent.Invoke();
        }


        public int PasswordMinLength {
            get { return passwordMinLength; }
            set { passwordMinLength = value; }
        }
        public bool SecurePassword {
            get { return securePassword; }
            set { securePassword = value; }
        }

        public string EmptyEmailMessage {
            get { return emptyEmailMessage; }
            set { emptyEmailMessage = value; }
        }

        public string InvalidEmailMessage {
            get { return invalidEmailMessage; }
            set { invalidEmailMessage = value; }
        }

        public string EmptyPasswordMessage {
            get { return emptyPasswordMessage; }
            set { emptyPasswordMessage = value; }
        }

        public string ShortPasswordErrorMessage {
            get { return shortPasswordErrorMessage; }
            set { shortPasswordErrorMessage = value; }
        }
    }
}