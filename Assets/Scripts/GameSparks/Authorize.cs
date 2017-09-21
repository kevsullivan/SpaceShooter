using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Authorize : MonoBehaviour {

    public InputField usernameInput, passwordInput; 

	public void AuthorizePlayer()
    {
        Debug.Log("Authorizing Player...");
        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(usernameInput.text)
            .SetPassword(passwordInput.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log(string.Format("Player Authenticated... \n User Name: {0}", response.DisplayName));
                }
                else
                {
                    Debug.Log(string.Format("Error Authenticating Player... \n{0}", response.Errors.JSON.ToString()));
                }
            });
    }
}
