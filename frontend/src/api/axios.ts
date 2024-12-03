import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import { decryptData, encryptData } from "./encryptData";

interface CustomAxiosRequestConfig extends AxiosRequestConfig {
    _retry?: boolean;
}

const api = axios.create({
    baseURL: "http://localhost:5027/api/",
    headers: {
        "Content-Type": "application/json",
    },
});

const signUp = axios.create({
    baseURL: "http://localhost:5027/api/Auth/register",
    headers: {
        "Content-Type": "text/plain",
    },
});

const signIn = axios.create({
    baseURL: "http://localhost:5027/api/Auth/login",
    headers: {
        "Content-Type": "text/plain",
    },
});

const refresh = axios.create({
    baseURL: "http://localhost:5027/api/Token/refresh",
    headers: {
        "Content-Type": "application/json",
    },
});

const validateToken = axios.create({
    baseURL: "http://localhost:5027/api/Token/validate",
    headers: {
        "Content-Type": "application/json",
    }
});

api.interceptors.request.use(
    (config: any) => {

        const token = localStorage.getItem('accessToken');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }

        if (config.data) {
            config.data = encryptData(config.data);
            config.headers['Content-Type'] = 'text/plain';
        }

        return config
    },
    (error: AxiosError): Promise<AxiosError> => Promise.reject(error)
);

api.interceptors.response.use(
    (response: AxiosResponse) => {
        if (response.data && typeof response.data === "string") {
            try {
                response.data = decryptData(response.data);
            } catch (error) {
                console.error("Błąd deszyfrowania odpowiedzi:", error);
            }
        }

        return response;
    },
    async (error: AxiosError) => {
        const originalRequest = error.config as CustomAxiosRequestConfig;

        if (originalRequest == undefined) return Promise.reject(error);
        if (error.response && error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            try {

                const refreshToken = localStorage.getItem('refreshToken');
                const response = await refresh.post('', { 'refreshToken': refreshToken });

                const access = response.data.accessToken;
                localStorage.setItem('accessToken', access);

                originalRequest.headers = originalRequest.headers || {};
                originalRequest.headers['Authorization'] = `Bearer ${access}`;

                return api(originalRequest);
            } catch (refreshError: unknown) {

                if (refreshError instanceof AxiosError && refreshError.response) {

                    if ([401, 403, 406].includes(refreshError.response.status)) {

                        console.error("nie ma autoryzacji");
                    } else {
                        console.error("Nie udało się odświeżyć tokena z innego powodu", refreshError);
                        localStorage.clear();
                    }
                } else {
                    console.error("Unhandled error type during token refresh", refreshError);
                    localStorage.clear();
                }
            }
        }

        return Promise.reject(error);
    }
);

const addSecurityInterceptors = (client: any) => {
    client.interceptors.request.use((config: any) => {
        if (config.data) {
            config.data = encryptData(config.data);
        }
        return config;
    });

    client.interceptors.response.use((response: any) => {
        if (response.data && typeof response.data === "string") {
            response.data = decryptData(response.data);
        }
        return response;
    });
};

addSecurityInterceptors(signUp);
addSecurityInterceptors(signIn);
addSecurityInterceptors(refresh);
addSecurityInterceptors(validateToken);

export { api, signUp, signIn, validateToken };