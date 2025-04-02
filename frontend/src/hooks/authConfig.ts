import { AccountInfo, PublicClientApplication } from "@azure/msal-browser";

const authConfig = {
    auth: {
        clientId: import.meta.env.VITE_AZURE_CLIENT_ID,
        authority: "https://login.microsoftonline.com/" + import.meta.env.VITE_AZURE_TENANT_ID,
        redirectUri: window.location.origin + "/auth/login",
    }
};

const data = {
    account: null as AccountInfo | null,
    msalInstance: new PublicClientApplication(authConfig),
    token: "",
    clearMsalState: function() {
        this.account = null;
        this.token = "";
        try {
            if (this.msalInstance) {
                const accounts = this.msalInstance.getAllAccounts();
                if (accounts.length > 0) {
                    this.msalInstance.logoutPopup({
                        mainWindowRedirectUri: window.location.origin + "/auth/login",
                        postLogoutRedirectUri: window.location.origin + "/auth/login"
                    }).catch(error => {
                        console.error("Error during MSAL logout popup:", error);
                        // Try to clear the cache even if logout fails
                        try {
                            this.msalInstance.clearCache();
                            // Remove all accounts as a fallback
                            accounts.forEach(account => {
                                try {
                                    // The correct method in newer MSAL versions
                                    this.msalInstance.logoutRedirect({ account });
                                } catch (removeError) {
                                    console.error("Error removing account:", removeError);
                                }
                            });
                        } catch (cacheError) {
                            console.error("Error clearing MSAL cache:", cacheError);
                        }
                    });
                } else {
                    // Just clear the cache if no accounts
                    this.msalInstance.clearCache();
                }
            }
        } catch (error) {
            console.error("Error during MSAL logout:", error);
            try {
                this.msalInstance.clearCache();
            } catch (e) {
                console.error("Error during MSAL cache clear:", e);
            }
        }
    }
};

export function useMsal() {
    return data;
}



