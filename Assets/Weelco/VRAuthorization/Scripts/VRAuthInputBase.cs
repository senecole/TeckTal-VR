using UnityEngine;
using UnityEngine.UI;

namespace Weelco.VRAuthorization {

    public class VRAuthInputBase : MonoBehaviour {

        private enum States {
            Default, Focused, Error, Success
        }

        private GameObject _defaultState;
        private GameObject _focusedState;
        private GameObject _errorState;
        private GameObject _successState;

        private Text _errorMessage;

        private States _currentState;

        // Use this for initialization
        public void Init() {
            SetDefaultState();
        }

        public void SetDefaultState() {
            clear();
            if (_defaultState == null) {
                _defaultState = transform.Find("DefaultState").gameObject;
            }
            _defaultState.SetActive(true);
            _currentState = States.Default;
        }

        public void SetFocusedState() {
            clear();
            if (_focusedState == null) {
                _focusedState = transform.Find("FocusedState").gameObject;
            }
            _focusedState.SetActive(true);
            _currentState = States.Focused;
        }

        public void SetErrorState(string message) {
            clear();
            if (_errorState == null) {
                Transform error = transform.Find("ErrorState");
                _errorMessage = error.Find("Text").GetComponent<Text>();
                _errorState = error.gameObject;
            }
            _errorMessage.text = message;
            _errorState.SetActive(true);
            _currentState = States.Error;
        }

        public void SetSuccessState() {
            clear();
            if (_successState == null) {
                _successState = transform.Find("SuccessState").gameObject;
            }
            _successState.SetActive(true);
            _currentState = States.Success;
        }

        private void clear() {
            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        } 
    
        public bool IsDefaultState {
            get { return _currentState.Equals(States.Default); }
        }

        public bool IsFocusedState {
            get { return _currentState.Equals(States.Focused); }
        }

        public bool IsErrorState {
            get { return _currentState.Equals(States.Error); }
        }

        public bool IsSuccessState {
            get { return _currentState.Equals(States.Success); }
        }
    }
}