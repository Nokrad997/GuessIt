import RegistrationData from "../interfaces/RegistrationData";
import LoginData from "../interfaces/LoginData";
import { signUp, signIn, api } from "./axios";
import { useNavigate } from "react-router-dom";

type RegistrationPromise = {
    id: number;
    Username: string;
    Email: string;
    is_admin: boolean;
};

type LoginPromise = {
    accessToken: string;
    refreshToken: string;
};

export async function registration(
    customerData: RegistrationData,
    triggerError: (message: string) => void
): Promise<RegistrationPromise> {
    try {
        console.log("customerData", customerData);
        const response = await signUp.post("", customerData);

        return response.data;
    } catch (error: any) {
        if (error.response) {
            console.error("Server responded with an error:", error.response.status, error.response.data);

            if (error.response.data.errorMessages) {
                const errorMessage = error.response.data.errorMessages.error;
                triggerError(errorMessage);
                throw new Error(errorMessage);
            } else {
                const errorMessage = error.response.data.error;
                triggerError(errorMessage);
                throw new Error(errorMessage);
            }
        } else {
            console.error("Error setting up the request:", error.message);
            const errorMessage = "Error setting up registration request";
            triggerError(errorMessage);
            throw new Error(errorMessage);
        }
    }
}

export async function login(
    customerData: LoginData,
    triggerError: (message: string) => void
): Promise<LoginPromise> {
    try {
        console.log("customerData", customerData);
        const response = await signIn.post("", customerData);

        return response.data;
    } catch (error: any) {
        if (error.response) {
            console.error("Server responded with an error:", error.response.status, error.response.data);

            if (error.response.data.errorMessages) {
                const errorMessage = error.response.data.errorMessages.error;
                triggerError(errorMessage);
                throw new Error(errorMessage);
            } else {
                const errorMessage = error.response.data.error;
                triggerError(errorMessage);
                throw new Error(errorMessage);
            }
        } else {
            console.error("Error setting up the request:", error.message);
            const errorMessage = "Error setting up login request";
            triggerError(errorMessage);
            throw new Error(errorMessage);
        }
    }
}

export async function isAdmin(triggerError: (message: string) => void): Promise<boolean> {
    try {
        const response = await api.post("auth/checkifadmin");
        return response.data;
    } catch (error: any) {
        const errorMessage = "Error checking admin status";
        triggerError(errorMessage);
        return false;
    }
}

export async function validateToken(triggerError: (message: string) => void): Promise<boolean> {
    try {
        const response = await api.post("Token/validate");
        return response.status === 200;
    } catch (error: any) {
        if(error.response.data !== "Token is invalid"){
            const errorMessage = "Error validating token";
            triggerError(errorMessage);
        }
        
        return false;
    }
}

export function isLoggedIn(): boolean {
    const token = getAccessToken();
    return token !== null;
}

export function saveTokens(accessToken: string, refreshToken: string) {
    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);
}

export function clearTokens() {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
}

export function getAccessToken(): string | null {
    return localStorage.getItem("accessToken");
}

export function logout() {
    clearTokens();
}