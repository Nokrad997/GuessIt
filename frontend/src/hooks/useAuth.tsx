import { registration, login, isAdmin, saveTokens, clearTokens, isLoggedIn, logout, validateToken } from '../api/auth';
import RegistrationData from '../interfaces/RegistrationData';
import LoginData from '../interfaces/LoginData';
import { useError } from '../components/ErrorContext/ErrorContext';

const useAuth = () => {
    const { triggerError } = useError();

    const registerCustomer = async (userData: RegistrationData) => {
        try {
            await registration(userData, triggerError);
            return true;
        } catch (error: any) {
            console.log("Registration failed: ", error);
            throw new Error(error.message || "Registration failed");
        }
    };

    const loginCustomer = async (userData: LoginData) => {
        try {
            const response = await login(userData, triggerError);
            console.log("response", response);
            saveTokens(response.accessToken, response.refreshToken);
            return true;
        } catch (error: any) {
            console.error("Login failed:", error);
            throw new Error(error.message || "Login failed");
        }
    };

    const checkIfAdmin = async () => {
        try {
            const response = await isAdmin(triggerError);
            return response;
        } catch (error: any) {
            return false;
        }
    };

    const checkIfLoggedIn = async () => {
        try {
            if (isLoggedIn() && await validateToken(triggerError)) {
                return true;
            } else {
                return false;
            }
        } catch (error: any) {
            console.error("Token validation failed:", error);
            clearTokens();
            return false
        }
    };

    const logoutCustomer = () => {
        logout();
    };

    return {
        registerCustomer,
        loginCustomer,
        checkIfAdmin,
        checkIfLoggedIn,
        logoutCustomer
    };
};

export default useAuth;
