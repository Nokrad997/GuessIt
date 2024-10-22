import { api } from "./axios";
export async function retrieveLeaderboard(){
    try {
        const response = await api.get('Leaderboard/');
        console.log(response.data)
        return response.data;
    } catch (error: any) {
        console.error("Failed in retrieving leaderboard:", error.response.status, error.response.data);
        throw new Error(error.response.data.error || "Failed in retrieving leaderboard");
    }
}