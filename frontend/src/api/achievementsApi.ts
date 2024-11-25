import { api } from "./axios";
export async function GetAchievements(){
    try {
        const response = await api.get('UserAchievements/getUserAssociatedAchievements');
        console.log(response.data)
        return response.data;
    } catch (error: any) {
        console.error("Failed in retrieving achievements:", error.response.status, error.response.data);
        throw new Error(error.response.data.error || "Failed in retrieving achievements");
    }
}