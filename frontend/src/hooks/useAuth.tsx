import { registration, login, isAdmin, saveTokens, clearTokens, isLoggedIn, logout } from '../api/auth';
import RegistrationData from '../interfaces/RegistrationData';
import LoginData from '../interfaces/LoginData';
import { useError } from '../components/ErrorContext/ErrorContext';

const useAuth = () => {
    const { triggerError } = useError();

    // Rejestracja użytkownika
    const registerCustomer = async (userData: RegistrationData) => {
        try {
            await registration(userData, triggerError);
            return true;
        } catch (error: any) {
            console.log("Registration failed: ", error);
            throw new Error(error.message || "Registration failed");
        }
    };

    // Logowanie użytkownika
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

    // Sprawdzanie, czy użytkownik jest administratorem
    const checkIfAdmin = async () => {
        try {
            const response = await isAdmin(triggerError);
            return response;
        } catch (error: any) {
            return false;
        }
    };

    // Sprawdzanie, czy użytkownik jest zalogowany
    const checkIfLoggedIn = () => {
        return isLoggedIn(); // Zwraca true, jeśli użytkownik ma token
    };

    // Wylogowanie użytkownika
    const logoutCustomer = () => {
        logout(); // Usuwa tokeny z localStorage
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
