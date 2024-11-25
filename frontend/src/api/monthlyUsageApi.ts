import { api } from "./axios";
export async function updateUsage(userUsage: number){
    try {
        const response = await api.post(`MonthlyUsage/${userUsage}`);
        console.log(response.data)
        return response.data;
    } catch (error: any) {
        console.error("Failed in updating monthly usage:", error.response.status, error.response.data);
        throw new Error(error.response.data.error || "Failed in updating monthly usage");
    }
}

export async function getUsage(){
    try {
        const response = await api.get('MonthlyUsage');
        console.log(response.data)
        return response.data;
    } catch (error: any) {
        console.error("Failed in retrieving monthly usage:", error.response.status, error.response.data);
        throw new Error(error.response.data.error || "Failed in retrieving monthly usage");
    }
}