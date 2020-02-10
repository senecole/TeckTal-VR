using UnityEditor;
using UnityEngine;

namespace Weelco.VRAuthorization {

    [ExecuteInEditMode]
    [CustomEditor(typeof(VRAuthSettings))]
    public class VRAutSettingsEditor : Editor {

        bool showErrorsSettings = true;

        // Use this for initialization
        void Start() {

        }

        public override void OnInspectorGUI() {

            VRAuthSettings myTarget = (VRAuthSettings)target;

            if (target == null) return;

            // Initial settings
            GUI.changed = false;
            EditorGUILayout.Space();
            EditorGUIUtility.labelWidth = 180;

            // Settings
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            myTarget.PasswordMinLength = EditorGUILayout.IntField("Password Min Length", myTarget.PasswordMinLength);
            myTarget.SecurePassword = EditorGUILayout.Toggle("Secure Password", myTarget.SecurePassword);

            // Mail
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUIStyle style = EditorStyles.foldout;
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            showErrorsSettings = EditorGUILayout.Foldout(showErrorsSettings, "Errors", style);
            style.fontStyle = previousStyle;
            GUILayout.EndHorizontal();
            if (showErrorsSettings) {
                myTarget.EmptyEmailMessage = EditorGUILayout.TextField("   Empty Email", myTarget.EmptyEmailMessage);
                myTarget.InvalidEmailMessage = EditorGUILayout.TextField("   Invalid Email", myTarget.InvalidEmailMessage);
                myTarget.EmptyPasswordMessage = EditorGUILayout.TextField("   Empty Password", myTarget.EmptyPasswordMessage);
                myTarget.ShortPasswordErrorMessage = EditorGUILayout.TextField("   Short Password", myTarget.ShortPasswordErrorMessage);
            }


            // Notifications
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Notifications", EditorStyles.boldLabel);

            SerializedProperty loginEvent = serializedObject.FindProperty("LoginEvent");
            EditorGUILayout.PropertyField(loginEvent);

            SerializedProperty registerEvent = serializedObject.FindProperty("RegisterEvent");
            EditorGUILayout.PropertyField(registerEvent);

            SerializedProperty selectEvent = serializedObject.FindProperty("SelectEvent");
            EditorGUILayout.PropertyField(selectEvent);

            serializedObject.ApplyModifiedProperties();


            // Final settings
            if (GUI.changed && myTarget != null)
                EditorUtility.SetDirty(myTarget);
        }
    }
}