import { AccountInfo, PublicClientApplication } from "@azure/msal-browser";

const authConfig = {
    auth: {
        clientId: import.meta.env.VITE_AZURE_CLIENT_ID,
        authority: "https://login.microsoftonline.com/" + import.meta.env.VITE_AZURE_TENANT_ID,
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
                        mainWindowRedirectUri: window.location.origin,
                    }).catch(error => {
                        console.error("Error during MSAL logout popup:", error);
                        this.msalInstance.clearCache();
                    });
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



