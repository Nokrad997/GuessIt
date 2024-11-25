import { api } from "./axios";
export async function GetStatistics(){
    try {
        const response = await api.get('Statistics/user-stats');
        console.log(response.data)
        return response.data;
    } catch (error: any) {
        console.error("Failed in retrieving statistics:", error.response.status, error.response.data);
        throw new Error(error.response.data.error || "Failed in retrieving statistics");
    }
}