using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPlayer : MonoBehaviour {

    public Text displayNameInput, userNameInput, passwordInput; // Set in editor

    public void RegisterPlayerButton()
    {
        Debug.Log("Registering Player...");
        new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(displayNameInput.text)
            .SetUserName(userNameInput.text)
            .SetPassword(passwordInput.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Registered \n User Name: " + response.DisplayName);
                }
                else
                {
                    Debug.Log("Error Registering Player... \n" + response.Errors.JSON.ToString());
                }
            });
    }
}
